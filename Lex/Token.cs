using System;
using System.Collections.Generic;
using System.Text;

namespace Lex
{
    public enum TokenKind
    {
        Unknown,
        Number,
        Word,
        Plus,
        Minus,
        EOF
    }

    public class Token
    {
        private string value;
        private TokenKind kind;

        public Token(TokenKind kind, string value)
        {
            this.kind = kind;
            this.value = value;
        }

        public TokenKind Kind
        {
            get { return this.kind; }
        }

        public string Value
        {
            get { return this.value; }
        }
    }

}
