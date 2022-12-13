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

fenParser.SetupBoard("r1q1k2r/8/8/8/8/8/8/R1Q1K2R w KQkq - 0 1"); // without parameter will use default fen --> Normal startup
engine.Run();

