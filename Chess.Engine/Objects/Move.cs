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
    }
}
