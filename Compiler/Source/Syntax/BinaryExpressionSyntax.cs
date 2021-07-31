using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(Position pos, ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Pos = pos;
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}
