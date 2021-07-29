using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Compiler.Source.Errors;
using Compiler.Source.Lib;

namespace Compiler.Source
{
    internal sealed class Lexer
    {
        private enum KeywordType
        {
            DeclareVariable
        }

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
                if (CurrentChar == '\t' || CurrentChar == ' ' || CurrentChar == '\n')
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

                    while (CurrentChar != null && char.IsLetterOrDigit((char)CurrentChar)) {
                        end_word += CurrentChar;
                        Next();
                    }

                    tokens.Add(new SyntaxToken(SyntaxType.IdentifierToken, end_word, null));
                }
                #endregion

                #region Operation Symbols [-, --, +, ++, *, /, ^] & Parenthesis
                else if (CurrentChar == '+')
                {
                    var _posStart = _position.Copy();
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
                    var _posStart = _position.Copy();
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
