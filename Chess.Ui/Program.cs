
using Chess.EngineCore;
using Chess.EngineCore.Objects;
using System.Text;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.Helpers.FenParser;


var board = new Chess.EngineCore.Boards.Board();

// var piecesInUnicode = board.unicodePiece; // For later implementation

Piece piece = new Piece();

Engine engine = new Engine(board, piece);
var fenParser = new Chess.EngineCore.Helpers.FenParser(board, engine, "");
var fenBuilder = new Chess.EngineCore.Helpers.FenBuilder(board, engine);

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

//PrintBoard();


fenParser.SetupBoard("");

//fenBuilder.GnerateFen();

engine.History = new List<string>();
engine.Run();