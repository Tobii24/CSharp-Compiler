using System;

using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;

namespace Compiler.Source.Datatypes
{
    public class NumberType : Datatype
    {
        #nullable enable
        //Public Variables & Abstract overrides
        public override Datatypes Type => Datatypes.Number;
        public SyntaxToken NumberToken { get; }
        public override dynamic Value { get; set; }

        //Constructor
        public NumberType(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
            Value = numberToken.Value;
        }
        #nullable disable

        //String representation
        public override string ToString() => Value.ToString();

        //Utils
        public void Negate()
        {
            Value = -Value;
        }

        #region Operations [+, -, *, /, ^]
        public static NumberType operator +(NumberType left, NumberType right)
        {
            var value = left.Value + right.Value;
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken, 
                $"{value}", value
              ));
        }

        public static NumberType operator -(NumberType left, NumberType right)
        {
            var value = left.Value - right.Value;
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }

        public static NumberType operator *(NumberType left, NumberType right)
        {
            var value = left.Value * right.Value;
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }

        public static NumberType operator /(NumberType left, NumberType right)
        {
            float value = (float)left.Value / (float)right.Value;
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }

        public static NumberType operator ^(NumberType left, NumberType right)
        {
            var value = Math.Pow(left.Value, right.Value);
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }
        #endregion
    }
}
