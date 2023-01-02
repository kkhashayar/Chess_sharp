
using Chess.EngineCore;
using Chess.EngineCore.EngineParts;
using Chess.EngineCore.Objects;
using System.Text;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.Helpers.FenParser;

// Not sure about this structure

var board = new Chess.EngineCore.Boards.Board();

Piece piece = new Piece();
Engine engine = new Engine(board, piece);

PieceAssigner pieceAssigner = new PieceAssigner(piece, engine);

var fenParser = new Chess.EngineCore.Helpers.FenParser(board, engine, pieceAssigner, "");
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


fenParser.SetupBoard("8/pppppppp/8/8/1P1P1P1P/8/8/8 w - - 0 1");

//fenBuilder.GnerateFen();

engine.History = new List<string>();
engine.Run();