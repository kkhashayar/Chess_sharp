
using Chess.EngineCore;
using System.Text;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.FenParser;


var board = new Chess.EngineCore.Boards.Board();

// var piecesInUnicode = board.unicodePiece; // For later implementation

Piece piece = new Piece();

Engine engine = new Engine(board, piece);


board.BuildChessBoard(64);


void PrintBoard()
{
    int index = 0;
    for (int square = 0; square < board.ChessBoard.Count; square++)
    {
        index++;
        Console.Write($" {(board.ChessBoard[square].Value)} ");
        if (index >= 8)
        {
            index = 0;
            Console.WriteLine();
        }
    }
}

var fenParser = new Chess.EngineCore.FenParser(board, engine, "");
fenParser.SetupBoard("");
engine.History = new List<string>();
engine.Run();