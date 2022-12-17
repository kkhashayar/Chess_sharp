using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore
{
    public class Square
    {
        public int SquareNumber { get; set; }
        public string? SquareName { get; set; }
        public int Value { get; set; } = 0;
    }
}
