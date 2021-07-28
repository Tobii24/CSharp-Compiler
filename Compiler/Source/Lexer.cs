using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Compiler.Source
{
    class Lexer
    {
        private readonly string _text;
        private readonly string _filename;
        private Position _position;

        public Lexer(string filename, string text)
        {
            _text = text;
            _filename = filename;
            _position = new Position(0, 0, 0, filename, text);
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

        public (SyntaxToken[], Error) GetTokens()
        {
            var tokens = new List<SyntaxToken>();

            while (CurrentChar != null)
            {
                #region Ignore indentation
                if (CurrentChar == '\t' || CurrentChar == ' ')
                {
                    Next();
                }
                #endregion

                #region Numbers
                else if (char.IsDigit((char)CurrentChar))
                {
                    var pos_start = _position.Copy();
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
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, val, pos_start, _position));
                    }
                    else
                    {
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, int.Parse(end_num)));
                    }
                }
                #endregion

                #region Operation Symbols [-, +, *, /, ^] & Parenthesis
                else if (CurrentChar == '+')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.PlusToken, "+", null, _position));
                    Next();
                }
                else if (CurrentChar == '-')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.MinusToken, "-", null, _position));
                    Next();
                }
                else if (CurrentChar == '*')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.StarToken, "*", null, _position));
                    Next();
                }
                else if (CurrentChar == '/')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.SlashToken, "/", null, _position));
                    Next();
                }
                else if (CurrentChar == '^')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.PowToken, "^", null, _position));
                    Next();
                }
                else if (CurrentChar == '(')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.OPToken, "(", null, _position));
                    Next();
                }
                else if (CurrentChar == ')')
                {
                    tokens.Add(new SyntaxToken(SyntaxType.CPToken, ")", null, _position));
                    Next();
                }
                #endregion

                #region Unknown Character
                else
                {
                    var pos_start = _position.Copy();
                    char? Char = CurrentChar;
                    Next();
                    return (null, new IllegalCharError(pos_start, _position, $"'{Char}'"));
                }
                #endregion
            }

            tokens.Add(new SyntaxToken(SyntaxType.EndOfFile, "\0", null, _position));
            return (tokens.ToArray(), null);
        }
    }
}
