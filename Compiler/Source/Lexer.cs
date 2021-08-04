using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Compiler.Source.Errors;
using Compiler.Source.Lib;

namespace Compiler.Source
{
    public enum KeywordType
    {
        _,
        DeclareVariable,
        IfStatement,
        WhileStatement,
        BreakStatement,
        ContinueStatement,
        AndStatement,
        OrStatement,
        NotStatement
    }

    internal sealed class Lexer
    {
        private readonly string _text;
        private Position _position;
        private Dictionary<string, KeywordType> keywordRef;
        public DiagnosticBag _diagnostics;

        public Lexer(string filename, string text)
        {
            _text = text;
            _position = new Position(0, 0, 0, filename);
            _diagnostics = new DiagnosticBag();
            keywordRef = new Dictionary<string, KeywordType>();

            keywordRef.Add("declare", KeywordType.DeclareVariable);
            keywordRef.Add("and", KeywordType.AndStatement);
            keywordRef.Add("or", KeywordType.OrStatement);
            keywordRef.Add("not", KeywordType.NotStatement);
            keywordRef.Add("if", KeywordType.IfStatement);
            keywordRef.Add("while", KeywordType.WhileStatement);
            keywordRef.Add("continue", KeywordType.ContinueStatement);
            keywordRef.Add("break", KeywordType.BreakStatement);
        }

        private char? CurrentChar
        {
            get
            {
                if (_position.Index >= _text.Length)
                    return null;

                return _text[_position.Index];
            }
        }

        private void Next()
        {
            _position.Advance(CurrentChar);
        }

        public SyntaxToken[] Lex()
        {
            var tokens = new List<SyntaxToken>();

            while (CurrentChar != null)
            {
                #region Ignore indentation
                if (char.IsControl((char)CurrentChar) || char.IsWhiteSpace((char)CurrentChar))
                {
                    Next();
                }
                #endregion

                #region Numbers
                else if (char.IsDigit((char)CurrentChar))
                {
                    string end_num = "";
                    int dot_count = 0;

                    while (CurrentChar != null && Regex.IsMatch(CurrentChar.ToString(), @"^[0-9]+$") || CurrentChar == '.')
                    {
                        if (CurrentChar == '.')
                        {
                            if (dot_count == 1)
                                break;
                            dot_count++;
                            end_num += '.';
                        }
                        else { end_num += CurrentChar; }

                        Next();
                    }

                    if (dot_count == 1)
                    {
                        float val = float.Parse(end_num, CultureInfo.InvariantCulture);
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, val));
                    }
                    else
                    {
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, int.Parse(end_num)));
                    }
                }
                #endregion

                #region Identifiers & Keyword
                else if (char.IsLetter((char)CurrentChar))
                {
                    string end_word = "";

                    while (CurrentChar != null && char.IsLetterOrDigit((char)CurrentChar))
                    {
                        end_word += CurrentChar;
                        Next();
                    }

                    bool isKeyword = false;
                    KeywordType keywordt = KeywordType._;

                    foreach (var val in keywordRef.Keys)
                    {
                        if (end_word == val)
                        {
                            isKeyword = true;
                            isKeyword = keywordRef.TryGetValue(val, out keywordt);
                        }
                    }

                    if (isKeyword)
                        tokens.Add(new SyntaxToken(SyntaxType.KeywordToken, end_word, keywordt));
                    else
                        tokens.Add(new SyntaxToken(SyntaxType.IdentifierToken, end_word, null));
                }
                #endregion

                #region Operation Symbols [-, --, +, ++, *, /, ^] & Parenthesis
                else if (CurrentChar == '+')
                {
                    Next();
                    if (CurrentChar == '+')
                    {
                        tokens.Add(new SyntaxToken(SyntaxType.PlusPlusToken, "++", null));
                        continue;
                    }
                    tokens.Add(new SyntaxToken(SyntaxType.PlusToken, "+", null));
                }
                else if (CurrentChar == '-')
                {
                    Next();
                    if (CurrentChar == '-')
                    {
                        tokens.Add(new SyntaxToken(SyntaxType.MinusMinusToken, "--", null));
                        continue;
                    }
                    tokens.Add(new SyntaxToken(SyntaxType.MinusToken, "-", null));
                }
                else if (CurrentChar == '*')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.StarToken, "*", null));
                }
                else if (CurrentChar == '/')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.SlashToken, "/", null));
                }
                else if (CurrentChar == '^')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.PowToken, "^", null));
                }
                else if (CurrentChar == '(')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.OpenParenthesisToken, "(", null));
                }
                else if (CurrentChar == ')')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.CloseParenthesisToken, ")", null));
                }
                #endregion

                #region Comparison Symbols [=, ==, <, >, <=, >=, !=] & Arrow => (for inline functions)
                else if (CurrentChar == '=')
                {
                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();
                        tokens.Add(new SyntaxToken(SyntaxType.EqualsEqualsToken, "==", null));
                    }
                    else if (CurrentChar == '>')
                    {
                        Next();
                        tokens.Add(new SyntaxToken(SyntaxType.ArrowToken, "=>", null));
                    }
                    else
                        tokens.Add(new SyntaxToken(SyntaxType.EqualsToken, "=", null));
                }
                else if (CurrentChar == '<')
                {
                    Next();
                    if (CurrentChar == '=')
                    {
                        Next();
                        tokens.Add(new SyntaxToken(SyntaxType.LessThanEqualsToken, "<=", null));
                    }
                    else
                        tokens.Add(new SyntaxToken(SyntaxType.LessThanToken, "<", null));
                }
                else if (CurrentChar == '>')
                {
                    Next();
                    if (CurrentChar == '=')
                    {
                        Next();
                        tokens.Add(new SyntaxToken(SyntaxType.GreaterThanEqualsToken, ">=", null));
                    }
                    else
                        tokens.Add(new SyntaxToken(SyntaxType.GreaterThanToken, ">", null));
                }
                else if (CurrentChar == '!')
                {
                    Next();
                    if (CurrentChar == '=')
                    {
                        Next();
                        tokens.Add(new SyntaxToken(SyntaxType.GreaterThanEqualsToken, ">=", null));
                    }
                    else
                        _diagnostics.Append(new ExpectedCharError(_position, $"'>', not '{CurrentChar}'"));
                }
                #endregion

                #region Extra Symbols [(,), {, }]
                else if (CurrentChar == ',')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.CommaToken, ",", null));
                    Next();
                }
                else if (CurrentChar == '{')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.OpenKeyToken, "{", null));
                    Next();
                }
                else if (CurrentChar == '}')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.CloseKeyToken, "}", null));
                    Next();
                }
                #endregion

                #region Semicolon (newline ref)
                else if (CurrentChar == ';')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.SemicolonToken, ";", null));
                }
                #endregion

                #region Unknown Character
                else
                {
                    _diagnostics.Append(new IllegalCharError(_position, $"'{CurrentChar}'"));
                    Next();
                }
                #endregion
            }

            Next();

            tokens.Add(new SyntaxToken(SyntaxType.EndOfFileToken, "\0", null));
            return tokens.ToArray();
        }
    }
}
