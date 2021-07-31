using System;
using Compiler.Source.Syntax;
using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public abstract class Error
    {
        public Position Pos { get; }
        public string ErrorName { get; }
        public string Details { get; }
        public string Context { get; }

        public Error(Position pos, string errorName, string details, string context="")
        {
            Pos = pos;
            ErrorName = errorName;
            Details = details;
            Context = context;
        }

        public void Throw()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(AsString());
            Console.ResetColor();
        }

        private string AsString()
        {
            var res = $"{ErrorName}: {Details}\n";
            if (Context != "")
                res += $"Context -> {Context}";

            return res;
        }
    }
}
