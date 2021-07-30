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

        public Error(Position pos, string errorName, string details)
        {
            Pos = pos;
            ErrorName = errorName;
            Details = details;
        }

        public ErroredExpression Throw()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(AsString());
            Console.ResetColor();

            return new ErroredExpression();
        }

        private string AsString()
        {
            var res = $"{ErrorName}: {Details}\n";

            return res;
        }
    }
}
