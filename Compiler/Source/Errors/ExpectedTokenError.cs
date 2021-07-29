using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public class ExpectedTokenError : Error
    {
        public ExpectedTokenError(Position pos, string details)
            : base(pos, "Expected Token", details) { }
    }
}
