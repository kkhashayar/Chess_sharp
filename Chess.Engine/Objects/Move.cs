using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore.Objects
{
    public struct MoveObject
    {
        public MoveObject() { }
        public int StartIndex = 0;
        public int EndIndex = 0;
        public string StartCoordinate = string.Empty;
        public string EndCoordinate = string.Empty;
        public int difference = 0;
        public int[] LegalMoves; 
        public string OnBoardStartSquare = string.Empty;
        public string OnBoardEndSquare = string.Empty;
        public string pieceSYmbol = string.Empty;
    }
}
