using System;

using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;

namespace Compiler.Source.Datatypes
{
    public class BooleanType : Datatype
    {
        //Public Variables & Abstract overrides
        public override Datatypes Type => Datatypes.Boolean;
        public SyntaxToken BoolToken { get; }
        public override dynamic Value { get; set; }

        //Constructor
        public BooleanType(SyntaxToken boolToken)
        {
            BoolToken = boolToken;
            Value = boolToken.Value;
        }

        //Utils
        public void Negate()
        {
            Value = !Value;
        }

        //String representation
        public override string ToString() => Value.ToString();
    }
}
