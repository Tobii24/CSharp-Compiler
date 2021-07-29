using System;

namespace Compiler.Source.Errors
{
    abstract class Error
    {
        public Position PosStart { get; }
        public Position PosEnd { get; }
        public string ErrorName { get; }
        public string Details { get; }

        private string StringWithArrows(string text, Position pos_start, Position pos_end)
        {
            var res = "";

            //Console.WriteLine($"{pos_start} {pos_end}");

            //Calculate indices

            int idx_start = Math.Max(text.LastIndexOf('\n', pos_start.Index-1), 0);
            int idx_end = text.IndexOf('\n', idx_start+1);
            if (idx_end < 0)
                idx_end = text.Length;

            //Generate lines
            int line_count = pos_end.Line - pos_start.Line + 1;
            for(var i=0; i < line_count; i++)
            {
                // Calculate line columns
                var line = text.Substring(idx_start, idx_end);

                var col_start = 0;
                if (i == 0)
                    col_start = pos_end.Column - 1;
                var col_end = line.Length - 1;
                if (i == line_count - 1)
                    col_end = pos_end.Column - col_start;

                //Console.WriteLine($"{col_start} {col_end}");

                //Append to result
                res += line + "\n";
                res += new string(' ', col_start) + new string('^', col_end);

                //Re-Calculate indices (?)
                if (idx_end != text.Length)
                {
                    idx_start = idx_end;
                    idx_end = text.IndexOf('\n', idx_start + 1);
                    if (idx_end < 0)
                        idx_end = text.Length;
                }
            }
            return res.Replace('\t', '\0');
        }

        public Error(Position posStart, Position posEnd, string errorName, string details)
        {
            PosStart = posStart;
            PosEnd = posEnd;
            ErrorName = errorName;
            Details = details;
        }

        public void Throw()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(AsString());
            Console.ResetColor();
        }

        private string AsString()
        {
            var res = $"{ErrorName}: {Details}\n";
            res += $"File {PosStart.Filename}, line {PosStart.Line + 1}\n\n";
            res += StringWithArrows(PosStart.Filetext, PosStart, PosEnd);

            return res;
        }
    }
}
