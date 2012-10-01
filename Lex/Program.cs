using System;
using System.Collections.Generic;
using System.Text;

namespace Lex
{
    class Program
    {
        public static void Main(string[] args)
        {
            string str = "3 + 2 - 5";
            SyntaxAnalyzer syntax = new SyntaxAnalyzer(new LexicalAnalyzer(str));
            if (syntax.IsValid)
            {
                Console.WriteLine(new SemanticAnalyzer(syntax.Tokens).IsValid);
            }
            else
            {
                Console.WriteLine("Invalid syntax.");
            }
            Console.ReadLine();
        }
    }
}