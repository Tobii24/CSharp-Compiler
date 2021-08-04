using System;

using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;
using System.Xml.Linq;

namespace Compiler.Source.Datatypes
{
    public class NumberType : Datatype
    {
        #nullable enable
        //Public Variables & Abstract overrides
        public override Datatypes Type => Datatypes.Number;
        public SyntaxToken NumberToken { get; }
        public override dynamic Value { get; set; }
        public override bool CanArithmetic => true;
        public override (Datatype, Error?) Notted(Position pos, Context currentContext)
        {
            Value = -Value;
            return (this, null);
        }

        //Constructor
        public NumberType(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
            Value = numberToken.Value;
        }
        #nullable disable

        //String representation
        public override string ToString() => Value.ToString();

        #region Arithmetic Operations
        public NumberType Add(NumberType right)
        {
            var res = this.Value + right.Value;
            return new NumberType(
                new SyntaxToken(
                    SyntaxType.NumberToken,
                    $"{res}",
                    res
                )
            );
        }

        public NumberType Minus(NumberType right)
        {
            var res = this.Value - right.Value;
            return new NumberType(
                new SyntaxToken(
                    SyntaxType.NumberToken,
                    $"{res}",
                    res
                )
            );
        }

        public NumberType Times(NumberType right)
        {
            var res = this.Value * right.Value;
            return new NumberType(
                new SyntaxToken(
                    SyntaxType.NumberToken,
                    $"{res}",
                    res
                )
            );
        }

        public (NumberType, Error) Divided(NumberType right, Position pos, Context context)
        {
            if (right.Value == 0)
            {
                return (null,
                    new RuntimeError(
                    pos,
                    "Cannot divide by '0'",
                    context.ContextString
                ));
            }

            var res = this.Value / right.Value;
            return (new NumberType(
                new SyntaxToken(
                    SyntaxType.NumberToken,
                    $"{res}",
                    res
                )
            ), null);
        }

        public NumberType Pow(NumberType right)
        {
            var res = (float)Math.Pow((float)this.Value, (float)right.Value);
            return new NumberType(
                new SyntaxToken(
                    SyntaxType.NumberToken,
                    $"{res}",
                    res
                )
            );
        }
        #endregion
    }
}
