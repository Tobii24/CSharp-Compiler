using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public class ExpectedCharError : Error
    {
        public ExpectedCharError(Position pos, string details)
            : base(pos, "Expected Char", details) { }
    }
}
