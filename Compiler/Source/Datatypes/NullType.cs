using System;

using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;

namespace Compiler.Source.Datatypes
{
    public class NullType : Datatype
    {
        //Public Variables & Abstract overrides
        public override Datatypes Type => Datatypes.Null;
        public override dynamic Value { get { return null; } set { } }

        //String representation
        public override string ToString() => "null";
    }
}
