namespace Compiler.Source
{
    class Position
    {
        public int Index { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Filename { get; }
        public string Filetext { get; }

        public Position(int index, int line, int column, string filename, string filetext)
        {
            Index = index;
            Line = line;
            Column = column;
            Filename = filename;
            Filetext = filetext;
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
            return new Position(Index, Line, Column, Filename, Filetext);
        }
    }
}
