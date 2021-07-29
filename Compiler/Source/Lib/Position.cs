namespace Compiler.Source.Lib
{
    public class Position
    {
        public int Index { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Filename { get; }

        public Position(int index, int line, int column, string filename)
        {
            Index = index;
            Line = line;
            Column = column;
            Filename = filename;
        }

        public void Advance(char? currentChar=null)
        {
            Index++;
            Column++;

            if (currentChar == '\n')
            {
                Line++;
                Column = 0;
            }
        }

        public Position Copy()
        {
            return new Position(Index, Line, Column, Filename);
        }

        public override string ToString()
        {
            return $"Pos(Idx: {Index}, Ln: {Line}, Col: {Column})";
        }
    }
}
