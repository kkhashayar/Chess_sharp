using Chess.Boards;
using Chess.EngineCore;
using Chess.PieceBase;
using Chess.Parser;
using System.Text;

Board board = new Board();
var piecesInUnicode = board.unicodePiece; // For later implementation
Piece piece = new Piece();
Engine engine = new Engine(board, piece);

FenParser fenParser = new FenParser(board, engine);
engine.History = new List<string>();
fenParser.SetupBoard(""); // without parameter will use default fen --> Normal startup
engine.Run();

