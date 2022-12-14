using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Boards
{
    public class Board
    {
        public string CurrentSquare { get; set; } = String.Empty;

        //public List<string> board = new List<string>
        //{
        //    "br", "bn", "bb", "bq", "bk", "bb", "bn", "br",
        //    "bp", "bp", "bp", "bp", "bp", "bp", "bp", "bp",
        //    "..", "..", "..", "..", "..", "..", "..", "..",
        //    "..", "..", "..", "..", "..", "..", "..", "..",
        //    "..", "..", "..", "..", "..", "..", "..", "..",
        //    "..", "..", "..", "..", "..", "..", "..", "..",
        //    "wP", "wP", "wP", "wP", "wP", "wP", "wP", "wP",
        //    "wR", "wN", "wB", "wQ", "wK", "wB", "wN", "wR"
        //};

        /*
         
           "00", "01", "02", "03", "04", "05", "06", "07",
           "08", "09", "10", "11", "12", "13", "14", "15",
           "16", "17", "18", "19", "20", "21", "22", "23",
           "24", "25", "26", "27", "28", "29", "30", "31",
           "32", "33", "34", "35", "36", "37", "38", "39",
           "40", "41", "42", "43", "44", "45", "46", "47",
           "48", "49", "50", "51", "52", "53", "54", "55",
           "56", "57", "58", "59", "60", "61", "62", "63"
        
         */

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
