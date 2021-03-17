using System;

namespace TrilComp
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer();

            lexer.lex(new string[]{"{", "a++11", "}"});
        }
    }
}
