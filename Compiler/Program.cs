using System;
using Compiler.Source;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                var lexer = new Lexer("<stdin>", line);
                var lexerRes = lexer.GetTokens();
                if (lexerRes.Item2 != null)
                { 
                    Console.WriteLine(lexerRes.Item2); 
                }
                else if (lexerRes.Item1 != null)
                {
                    foreach (var tok in lexerRes.Item1)
                    {
                        Console.WriteLine(tok);
                    }
                }
            }
        }
    }
}
