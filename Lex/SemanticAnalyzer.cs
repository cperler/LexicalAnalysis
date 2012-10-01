using System;
using System.Collections.Generic;
using System.Text;

namespace Lex
{
    public enum Types
    {
        Number,
        Word,
        Invalid
    }

    public class SemanticAnalyzer
    {
        private static Dictionary<TokenKind, Dictionary<Types[], Types>> rules = new Dictionary<TokenKind, Dictionary<Types[], Types>>();

        private Queue<Token> m_tokens;
        private List<Token> m_postfix;
        private Stack<Types> m_types;
        private bool m_isValid;
        private int m_index;

        static SemanticAnalyzer()
        {
            rules.Add(TokenKind.Plus, new Dictionary<Types[], Types>());
            rules.Add(TokenKind.Minus, new Dictionary<Types[], Types>());

            rules[TokenKind.Plus].Add(new Types[] { Types.Number, Types.Number }, Types.Number);
            rules[TokenKind.Minus].Add(new Types[] { Types.Number, Types.Number }, Types.Number);
            rules[TokenKind.Plus].Add(new Types[] { Types.Word, Types.Word}, Types.Word);
        }

        public SemanticAnalyzer(Queue<Token> tokens)
        {
            m_tokens = tokens;
            m_postfix = new List<Token>();
            m_types = new Stack<Types>();
            m_index = 0;
            m_isValid = Start();
        }

        private void ToPostfix()
        {
            Stack<Token> operators = new Stack<Token>();

            Token t = m_tokens.Dequeue();
            while (t.Kind != TokenKind.EOF)
            {
                switch (t.Kind)
                {
                    case TokenKind.Minus:
                    case TokenKind.Plus:
                        if (operators.Count == 0)
                        {
                            operators.Push(t);
                        }
                        else
                        {
                            m_postfix.Add(operators.Pop());
                            operators.Push(t);
                        }
                        break;
                    default:
                        m_postfix.Add(t);
                        break;
                }
                t = m_tokens.Dequeue();
            }

            while (operators.Count > 0)
            {
                m_postfix.Add(operators.Pop());
            }
            m_postfix.Add(t);
        }

        public bool IsValid
        {
            get
            {
                return m_isValid;
            }
        }

        private Token Next()
        {
            Token next = m_postfix[m_index];
            m_index++;
            return next;
        }

        private bool Start()
        {
            ToPostfix();
            return E(Next());
        }

        private bool E(Token t)
        {
            switch (t.Kind)
            {
                case TokenKind.Number:
                    m_types.Push(Types.Number);
                    return E(Next());
                case TokenKind.Word:
                    m_types.Push(Types.Word);
                    return E(Next());
                case TokenKind.Plus:
                case TokenKind.Minus:
                    Types type = CheckOp(t);
                    if (type != Types.Invalid)
                    {
                        m_types.Push(type);
                        return E(Next());
                    }
                    else
                    {
                        return false;
                    }
                case TokenKind.EOF:
                    return true;
                default:
                    return false;
            }
        }

        private Types CheckOp(Token t)
        {
            if (rules.ContainsKey(t.Kind))
            {
                Dictionary<Types[], Types> stub = rules[t.Kind];
                Types[] parameters = new Types[] { m_types.Pop(), m_types.Pop() };

                if (stub.ContainsKey(parameters))
                {
                    return stub[parameters];
                }
            }

            return Types.Invalid;
        }
    }
}
