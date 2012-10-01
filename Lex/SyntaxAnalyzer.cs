using System;
using System.Collections.Generic;
using System.Text;

namespace Lex
{
    public class SyntaxAnalyzer
    {
        private Queue<Token> m_tokens;
        private LexicalAnalyzer m_lexicalAnalyzer;
        private bool m_isValid;

        public SyntaxAnalyzer(LexicalAnalyzer lexicalAnalyzer)
        {
            m_tokens = new Queue<Token>();
            m_lexicalAnalyzer = lexicalAnalyzer;
            m_isValid = Start();
        }

        public bool IsValid
        {
            get
            {
                return m_isValid;
            }
        }

        public Queue<Token> Tokens
        {
            get
            {
                return m_tokens;
            }
        }

        private Token Next()
        {
            Token next = m_lexicalAnalyzer.Next();
            m_tokens.Enqueue(next);
            return next;
        }

        private bool Start()
        {
            return E(Next());
        }

        private bool E(Token t)
        {
            switch (t.Kind)
            {
                case TokenKind.Number:
                case TokenKind.Word:
                    Token next = Next();
                    return CheckEOF(next) || CheckOp(next);
                default:
                    return false;
            }
        }

        private bool CheckOp(Token t)
        {
            switch (t.Kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return E(Next());
                default:
                    return false;
            }
        }

        private bool CheckEOF(Token t)
        {
            switch (t.Kind)
            {
                case TokenKind.EOF:
                    return true;
                default:
                    return false;
            }
        }
    }
}
