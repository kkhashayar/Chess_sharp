using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.EngineCore.Boards;
using Chess.EngineCore.EngineParts;

namespace Chess.EngineCore.Helpers
{
    public class FenParser
    {
        private readonly Board _board;
        private readonly Engine _engine;
        private readonly string _fen;
        private readonly PieceAssigner _pieceAssigner;

        public FenParser(Board board, Engine engine, PieceAssigner pieceAssigner, string fen = "")
        {
            _board = board;
            _engine = engine;
            _fen = fen;
            _pieceAssigner = pieceAssigner;
        }

        public void SetupBoard(string fen = "")
        {
            // "5k2/1r6/3n4/8/4P3/8/1K4P1/8 w - - 0 1" test
            // Reads the Fen to board
            if (fen.Length == 0 || fen == null)
            {
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }

            string[] fenList = fen.Split(" ");
            string[] rows = fenList[0].Split("/");
            if (fenList[2].Contains("K"))
            {
                _engine.WhiteKingCastle = true;
            }
            if (fenList[2].Contains("Q"))
            {
                _engine.WhiteQueenCastle = true;
            }
            if (fenList[2].Contains("k"))
            {
                _engine.BlackKingCastle = true;
            }
            if (fenList[2].Contains("q"))
            {
                _engine.BlackQueenCastle = true;
            }
            _engine.Turn = fenList[1];


            int index = 0;
            foreach (var row in rows)
            {
                foreach (var square in row)
                {
                    if (char.IsDigit(square))
                    {
                        int emptySquare = int.Parse(square.ToString());
                        for (int i = 0; i < emptySquare; i++)
                        {
                            _board.board[index] = ".";
                            index++;
                        }
                    }
                    else
                    {
                        string piece = square.ToString();
                        string actualPiece;

                        actualPiece = piece;

                        _board.board[index] = actualPiece;
                        index++;
                    }
                }
            }
            _pieceAssigner.AsignThepieces(); // Reads from the board
        }
    }

}
