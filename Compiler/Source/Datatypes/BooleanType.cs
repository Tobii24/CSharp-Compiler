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
        public override bool CanArithmetic => false;
        public override (Datatype, Error) Notted(Position pos, Context currentContext)
        {
            Value = !Value;
            return (this, null);
        }

        //Constructor
        public BooleanType(SyntaxToken boolToken)
        {
            BoolToken = boolToken;
            Value = boolToken.Value;
        }

        //String representation
        public override string ToString() => Value.ToString();
    }
}
