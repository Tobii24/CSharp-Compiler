using Compiler.Source.Lib;

namespace Compiler.Source.Errors
{
    public class RuntimeError : Error
    {
        public RuntimeError(string details)
            : base(new Position(0, 0, 0, ""), "Runtime Error", details) { }
    }
}
