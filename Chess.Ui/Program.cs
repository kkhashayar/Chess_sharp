﻿using Chess.Boards;
using Chess.EngineCore;
using Chess.PieceBase;
using Chess.Parser;
using System.Text;

Board board = new Board();
var piecesInUnicode = board.unicodePiece;
Piece piece = new Piece();
Engine engine = new Engine(board, piece);

FenParser fenParser = new FenParser(board, engine);

fenParser.SetupBoard("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1"); // without parameter will use default fen --> Normal startup
engine.Run();

