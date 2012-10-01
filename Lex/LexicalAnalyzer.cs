using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Lex
{
    public class LexicalAnalyzer
    {
        private const char EOF = (char)0;

        private int m_column;
        private int m_pos;
        private string m_data;
        private int m_saveCol;
        private int m_savePos;

        public LexicalAnalyzer(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            m_data = data;

            Reset();
        }

        private void Reset()
        {
            m_column = 1;
            m_pos = 0;
        }

        protected char LookAhead(int count)
        {
            if (m_pos + count >= m_data.Length)
            {
                return EOF;
            }
            else
            {
                return m_data[m_pos + count];
            }
        }

        protected char Consume()
        {
            char ret = m_data[m_pos];
            m_pos++;
            m_column++;

            return ret;
        }

        protected Token CreateToken(TokenKind kind, string value)
        {
            return new Token(kind, value);
        }

        protected Token CreateToken(TokenKind kind)
        {
            string tokenData = m_data.Substring(m_savePos, m_pos - m_savePos);
            return new Token(kind, tokenData);
        }

        public Token Next()
        {
        ReadToken:

            char ch = LookAhead(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenKind.EOF, string.Empty);
                case ' ':
                case '\t':
                    {
                        Consume();
                        goto ReadToken;
                    }
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber();
                case '+':
                    Consume();
                    return CreateToken(TokenKind.Plus, new string(new char[] { ch }));
                case '-':
                    Consume();
                    return CreateToken(TokenKind.Minus, new string(new char[] { ch }));
                default:
                    {
                        if (char.IsLetter(ch))
                        {
                            return ReadWord();
                        }
                        else
                        {
                            StartRead();
                            Consume();
                            return CreateToken(TokenKind.Unknown);
                        }
                    }
            }
        }

        private void StartRead()
        {
            m_saveCol = m_column;
            m_savePos = m_pos;
        }

        protected Token ReadNumber()
        {
            StartRead();

            bool hadDot = false;

            Consume();

            while (true)
            {
                char ch = LookAhead(0);
                if (Char.IsDigit(ch))
                {
                    Consume();
                }
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    Consume();
                }
                else
                    break;
            }

            return CreateToken(TokenKind.Number);
        }

        protected Token ReadWord()
        {
            StartRead();
            Consume();
            while (true)
            {
                char ch = LookAhead(0);
                if (char.IsLetter(ch))
                {
                    Consume();
                }
                else
                {
                    break;
                }
            }

            return CreateToken(TokenKind.Word);
        }
    }
}