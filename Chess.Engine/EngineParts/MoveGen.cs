using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.Objects.Piece;

namespace Chess.EngineCore.EngineParts
{
    public class MoveGen
    {
        Board board = new Board();


        public List<int> GenAllMoves()
        {
            List<int> AllPOssibleMoves = new List<int>();
            return AllPOssibleMoves;
        }
    }
}
