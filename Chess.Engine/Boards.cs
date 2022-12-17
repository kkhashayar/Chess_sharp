using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.EngineCore
{
    public class Boards
    {
        
        public class Board
        {
            public List<Square> ChessBoard = new List<Square>();
            public List<string> board = new List<string>
        {
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", "..",
           "..", "..", "..", "..", "..", "..", "..", ".."
        };
            public List<string> coordinates = new List<string>
        {
            "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
            "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
            "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
            "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
            "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
            "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
            "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
            "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"
        };

            public List<Square> CreateBoard(int squareNumber)
            {
                ChessBoard.Clear();
                for (int i = 0; i <= squareNumber; i++)
                {
                    Square square = new Square();
                    square.SquareNumber = i;
                    square.SquareName = GetCoordinates(i);
                    ChessBoard.Add(square);
                }
                return ChessBoard;
            }
            public string GetCoordinates(int squareIndex)
            {
                if (IsValidIndex(squareIndex))
                {
                    return coordinates[squareIndex];
                }
                return 1.ToString();
            }

            public int GetBoardIndex(string squareName)
            {
                if (IsValidCoordinate(squareName))
                {
                    return coordinates.IndexOf(squareName);
                }
                return 1;
            }

            public bool IsValidCoordinate(string squareName)
            {
                if (coordinates.Contains(squareName))
                {
                    return true;
                }
                return false;
            }

            public bool IsValidIndex(int squareIndex)
            {
                if (board.Count >= squareIndex)
                {
                    return true;
                }
                return false;
            }

            //{ "wP", "wN", "wB", "wR", "wQ", "wK", "bp", "bn", "bb", "br", "bq", "bk" };
            public IDictionary unicodePiece = new Dictionary<int, Char>()
            {
                // Will be chess pieces in Unicode or hex

            };
        }
    }
}
