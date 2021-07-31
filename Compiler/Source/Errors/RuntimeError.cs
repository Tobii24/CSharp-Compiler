using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public class RuntimeError : Error
    {
        public RuntimeError(Position pos, string details, string context)
            : base(pos, "Runtime Error", details, context) { }
    }
}
