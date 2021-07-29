using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public class IllegalCharError : Error
    {
        public IllegalCharError(Position pos, string details)
            : base(pos, "Illegal Character", details) { }
    }
}
