namespace Compiler.Source.Errors
{
    class IllegalCharError : Error
    {
        public IllegalCharError(Position posStart, Position posEnd, string details)
            : base(posStart, posEnd, "Illegal Character", details) { }
    }
}
