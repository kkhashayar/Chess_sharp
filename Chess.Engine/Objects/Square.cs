using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore.Objects
{
    public struct Square
    {
        public Square() { }
        public int SquareNumber = 0;
        public string? SquareName = string.Empty;
        public int Value = 0;
    }
}
