using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess.PieceBase

{
    public class Piece
    {
        public string? Name { get; set; }
        public string? Sign { get; set; }
        public string? Color { get; set; }
        public decimal Value { get; set; }
        public bool IsMoved { get; set; } = false;
        public int Direction { get; set; }
        public int PositionIndex { get; set; }
        public string? PositionCoords { get; set; }

    }
}

