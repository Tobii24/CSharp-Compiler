using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;
using System.Globalization;
using System;

namespace Compiler.Source.Datatypes
{
    public class NumberType : Datatype
    {
        #nullable enable
        //Public Variables & Abstract overrides
        public override Datatypes Type => Datatypes.Number;
        public SyntaxToken NumberToken { get; }
        public dynamic Value { get; set; }

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

        //Operations
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
            var value = left.Value / right.Value;
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }

        public static NumberType operator ^(NumberType left, NumberType right)
        {
            var value = (float)Math.Pow((double)left.Value,  (double)right.Value);
            return new NumberType(
                new SyntaxToken(SyntaxType.NumberToken,
                $"{value}", value
              ));
        }
    }
}
