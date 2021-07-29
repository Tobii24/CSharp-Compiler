namespace Compiler.Source.Errors
{
    class ExpectedTokenError : Error
    {
        public ExpectedTokenError(Position posStart, Position posEnd, string details)
            : base(posStart, posEnd, "Expected Token", details) { }
    }
}
