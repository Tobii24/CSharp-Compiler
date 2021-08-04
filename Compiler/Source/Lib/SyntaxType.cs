﻿namespace Compiler.Source.Lib
{
    public enum SyntaxType
    {
        //Tokens
        EndOfFileToken,
        BadToken,
        SemicolonToken,
        NumberToken,
        KeywordToken,
        IdentifierToken,
        PlusToken,
        PlusPlusToken,
        MinusToken,
        MinusMinusToken,
        SlashToken,
        StarToken,
        PowToken,
        CommaToken,
        OpenKeyToken,
        CloseKeyToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        EqualsToken,
        EqualsEqualsToken,
        NotEquals,
        GreaterThanToken,
        LessThanToken,
        GreaterThanEqualsToken,
        LessThanEqualsToken,
        ArrowToken,

        //Expressions
        ErroredExpression,
        LiteralExpression,
        ParenthesizedExpression,
        BinaryExpression,
        UnaryExpression,
        ComparisonExpression,
        VarAssignExpression,
        VarAccessExpression,
        IfExpression
    }
}
