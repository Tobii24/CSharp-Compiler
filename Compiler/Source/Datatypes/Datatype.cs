using Compiler.Source.Lib;
using Compiler.Source.Syntax;

namespace Compiler.Source.Datatypes
{
    public enum Datatypes
    {
        Number
    }

    public abstract class Datatype
    {
        public abstract Datatypes Type { get; }
    }
}
