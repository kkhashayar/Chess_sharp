using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.EngineCore.Boards;

namespace Chess.EngineCore.Helpers
{
    public class FenBuilder
    {
        private readonly Board _board;
        private readonly Engine _engine;
        // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
        public FenBuilder(Board board, Engine engine)
        {
            _board = board;
            _engine = engine;
        }
        List<string> fenList = new List<string>();




        /// <summary>
        ///  Fen builder
        ///  Converting the position to Fen string, 
        /// </summary>
        public void GnerateFen()
        {
            Console.WriteLine("fen builder");
        }
    }
}
