using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore
{
    public class MoveGen
    {
        private readonly Engine _engine;
        private readonly Boards _boards;
        

        public MoveGen(Engine engine, Boards boards)
        {
            _engine = engine;
            _boards = boards;
            
        }


        public List<int> GenAllMoves()
        {
            List<Piece> position = new List<Piece>();
            


            List<int> move = new List<int>();
            return move;
        }
    }
}
