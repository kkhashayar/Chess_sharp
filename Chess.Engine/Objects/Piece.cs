using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore.Objects
{
    public struct Piece
    {
        public Piece(){}
        public string Name = string.Empty;
        public string Sign = string.Empty;
        public string Color = string.Empty;
        public decimal Value = 0;   
        public bool IsMoved = false; //Only for en passant.
        public int Direction = 0;
        public int PositionIndex = 0;   
        public string PositionCoords = string.Empty;
    }

   
}
