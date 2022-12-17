
using Chess.EngineCore;
using System.Text;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.FenParser;


var board = new Chess.EngineCore.Boards.Board();

// var piecesInUnicode = board.unicodePiece; // For later implementation

Piece piece = new Piece();

Engine engine = new Engine(board, piece);

var fenParser = new Chess.EngineCore.FenParser(board, engine, "");
fenParser.SetupBoard("");
engine.History = new List<string>();


// without parameter will use default fen --> Normal startup

engine.Run();

