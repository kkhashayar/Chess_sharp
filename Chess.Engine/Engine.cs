﻿using Chess.Boards;
using Chess.PieceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Chess.EngineCore
{
    public class Engine
    {
        private Board _board;
        private Piece _piece;
        public Engine(Board board, Piece piece)
        {
            _board = board;
            _piece = piece;
        }

        public List<string> _pathelements = new List<string>();
        public string HumanColor { get; set; } = string.Empty; // Not sure about these two properties1
        public string MachineColor { get; set; } = string.Empty;
        public bool Playing { get; set; } = false;
        public int Ply { get; set; }
        public string? Turn { get; set; }
        public List<string> History { get; set; }
    
        public List<string> Move { get; set; } // Useful in move history
        public string SSquare { get; set; } = string.Empty;
        public string TSquare { get; set; } = string.Empty;


        // Castle rules
        public bool WhiteKingCastle { get; set; } = false;
        public bool WhiteQueenCastle { get; set; } = false;
        public bool BlackKingCastle { get; set; } = false;
        public bool BlackQueenCastle { get; set; } = false;

        // Rook conditions to remove castle rights 
        


        // read the board, and creates a piece object for each piece,
        // the rest of the data will be awailable on class level 
        public List<Piece> Position = new List<Piece>();


        public bool IsMoved = false; // To keep a track of En passnt, Castelling

        // Enumerable ranks in reverese, comes handy while looping the board
        public int[] ranks = new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 };

        // All pieces as a string
        public List<string> allPieces = new List<string> { "wP", "wN", "wB", "wR", "wQ", "wK", "bp", "bn", "bb", "br", "bq", "bk" };

        // Jagged array, holding directions of pieces, match based on indexes.
        public int[][] LegalMoves = new int[][]
        {
            new int[] {-8,-16,-7,-9}, // white Pawn
            new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // White Knight
            new int[] {-9, -7, 9, 7}, // white Bishop
            new int[] {-1, -8, +1, +8}, // White Rook
            new int[] {-10, 1, 10, -1, -9, -11, 11, 9}, // White Queen --> Not in use
            new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // White king 
            new int[] {+8,+16,+7,+9}, // will be changed
            new int[] {-6,-10,-15,-17,+6,+10,+15,+17 }, // Knight
            new int[] {+8, +16, +7, +9}, // Black pawn
            new int[] {-1, -8, +1, +8}, //  Black Rook
            new int[] {-10, 1, 10, -1, -9, -11, 11, 9},// Black Queen --> Not in use
            new int[] {-1, -8, -2, +1, +8, +2, -9, -7, +9, +7}, // Black king
        };

        public int GetPieceIndex(string piece)
        {
            return allPieces.IndexOf(piece);
        }


        public void SetSideColor()
        {
            if (!Playing)
            {
                Console.WriteLine("Choose color");
                var color = Console.ReadLine();
                // Data checking will be here, for now assuming inputs are correct
                if (color == "white")
                {
                    HumanColor = "white";
                    MachineColor = "Black";
                }
                else
                {
                    HumanColor = "black";
                    MachineColor = "white";
                }
                Playing = true;
            }
            // Will set a side color from Fen reader
        }

        // 1) Converts Fen to Board
        // 2) Reads the board and creates Instance of each pieces
        // 3) Rest of the Fen info will be saved in class level properties
       
        public void AsignThepieces()
        {
            for (int i = 0; i < _board.board.Count(); i++)
            {
                
                string positioinCoords = _board.GetCoordinates(i);
                var row = positioinCoords[1].ToString();
                if (_board.board[i] != "..")
                {
                    if (_board.board[i] == "wN")
                    {
                        var _piece = new Piece()
                        {
                            Name = "knight",
                            Sign = "N",
                            Color = "white",
                            Value = 3,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "bn")
                    {
                        var _piece = new Piece()
                        {
                            Name = "knight",
                            Sign = "n",
                            Color = "black",
                            Value = 3,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "wR")
                    {
                        var _piece = new Piece()
                        {
                            Name = "rook",
                            Sign = "R",
                            Color = "white",
                            Value = 5,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "br")
                    {
                        var _piece = new Piece()
                        {
                            Name = "rook",
                            Sign = "r",
                            Color = "black",
                            Value = 5,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "wB")
                    {
                        var _piece = new Piece()
                        {
                            Name = "bishop",
                            Sign = "B",
                            Color = "white",
                            Value = 3.1m,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "bb")
                    {
                        var _piece = new Piece()
                        {
                            Name = "bishop",
                            Sign = "b",
                            Color = "black",
                            Value = 3.1m,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "wQ")
                    {
                        var _piece = new Piece()
                        {
                            Name = "queen",
                            Sign = "Q",
                            Color = "white",
                            Value = 9,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "bq")
                    {
                        var _piece = new Piece()
                        {
                            Name = "qeen",
                            Sign = "q",
                            Color = "black",
                            Value = 9,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "wP")
                    {
                        var _piece = new Piece()
                        {
                            Name = "pawn",
                            Sign = "P",
                            Color = "white",
                            Value = 1,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords,
                            Direction = -1
                        };
                        if (row == "2")
                        {
                            _piece.IsMoved = false;
                        }
                        else
                        {
                            _piece.IsMoved = true;
                        }

                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "bp")
                    {
                        var _piece = new Piece()
                        {
                            Name = "pawn",
                            Sign = "p",
                            Color = "black",
                            Value = 1,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords,
                            Direction = +1
                        };
                        if(row == "7")
                        {
                            _piece.IsMoved = false;
                        }
                        else
                        {
                            _piece.IsMoved = true;  
                        }
                        Position.Add(_piece);
                    }

                    else if (_board.board[i] == "wK")
                    {
                        var _piece = new Piece()
                        {
                            Name = "king",
                            Sign = "K",
                            Color = "white",
                            Value = 9999,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                    else if (_board.board[i] == "bk")
                    {
                        var _piece = new Piece()
                        {
                            Name = "king",
                            Sign = "k",
                            Color = "black",
                            Value = 9999,
                            IsMoved = false,
                            PositionIndex = i,
                            PositionCoords = positioinCoords
                        };
                        Position.Add(_piece);
                    }
                }
            }
            Console.WriteLine($"Pieces in position: {Position.Count}");
            Console.WriteLine("Press a key");
            Console.ReadKey();
            Console.WriteLine("Asigned pieces: \n");
            foreach (var piece in Position)
            {
                Console.WriteLine($"Name:" +
                    $"{piece.Name} Sign:{piece.Sign} Value:{piece.Value}, " +
                    $"PIndex:{piece.PositionIndex} PCoords:{piece.PositionCoords}");
            }
            Console.ReadKey();
        }

        // Functions responsible for returning each piece moves

        public bool GetRook(int sourceIndex, int targetIndex, int dif, string sSquare, string tSquare)
        {
            // Checks for Rook color and position to change the correspondence castle right
            _pathelements.Clear();
            // Move in files
            if (sSquare.Substring(0, 1) == tSquare.Substring(0, 1))
            {
                if (sourceIndex > targetIndex)
                {
                    // step is -8 --> moving up
                    for (int i = 1; i < 8; i++)
                    {
                        int path = sourceIndex - (i * 8);
                        if (path > targetIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == targetIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;

                }
                if (sourceIndex < targetIndex)
                {
                    // step is +8 --> moving down
                    for (int i = 1; i < 8; i++)
                    {
                        int path = sourceIndex + (i * 8);
                        if (path < targetIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }

                        if (path == targetIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
            }


            // Move in ranks
            else if (sSquare.Substring(1, 1) == tSquare.Substring(1, 1))
            {
                // step is -1 --> moving left
                if (sourceIndex > targetIndex)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        int path = sourceIndex - (i * 1);
                        if (path > targetIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == targetIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                if (sourceIndex < targetIndex)
                {
                    // step is +1 --> moving right
                    for (int i = 1; i < 8; i++)
                    {
                        int path = sourceIndex + (i * 1); // ref here 
                        if (path > targetIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == targetIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == "..") || _pathelements.Count == -0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            // Castle special move --> 
            return false;
        }
        public bool GetBishop(int sourceIndex, int targetIndex, int dif, int[] pieceLegalMoves, string sSquare, string tSquare)
        {
            _pathelements.Clear();
            int result_of_9 = dif % 9;
            int result_of_7 = dif % 7;

            if (dif % 9 == 0 || dif % 7 == 0 && sSquare.Substring(0, 1) != tSquare.Substring(0, 1)
                && sSquare.Substring(0, 2) != tSquare.Substring(0, 2)// preventing the piece to move cross the files
                && sSquare.Substring(1, 1) != tSquare.Substring(1, 1)) // preventing the piece to move cross the ranks
            { // I need to parameterize them to one function with, different difs, and side to move
              // +9 up right
              // +7 up left
                if (sourceIndex > targetIndex)
                {
                    if (dif % 9 == 0)
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            int path = sourceIndex - (9 * i);
                            if (path >= targetIndex && path <= _board.board.Count)
                            {
                                if (path != targetIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                        {
                            return true;
                        }
                    }
                    if (dif % 7 == 0)
                    {

                        for (int i = 1; i < 8; i++)
                        {
                            int path = sourceIndex - (7 * i);
                            if (path >= targetIndex && path <= _board.board.Count)
                            {
                                if (path != targetIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                        {

                            return true;
                        }
                    }
                }
                // -9 down left
                // -7 down right
                else
                {
                    if (dif % 9 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int path = sourceIndex + (9 * i);
                            if (path <= targetIndex && path <= _board.board.Count)
                            {
                                if (path != targetIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                        {
                            return true;
                        }
                    }
                    if (dif % 7 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int path = sourceIndex + (7 * i);
                            if (path <= targetIndex && path <= _board.board.Count)
                            {
                                if (path != targetIndex) // this condition is already applied! in line above
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                    }
                    if (_pathelements.All(x => x == "..") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool GetKnight(int sourceIndex, int targetIndex, int dif, int[] pieceLegalMoves)
        {
            if (pieceLegalMoves.Contains(dif))
            {
                return true;
            }
            return false;
        }
        public bool GetQueen(int sourceIndex, int targetIndex, int dif, string sSquare, string tSquare, int[] pieceLegalMoves)
        {
            // If in ranks or files GetRook()
            if (sSquare.Substring(0, 1) == tSquare.Substring(0, 1) || sSquare.Substring(1, 1) == tSquare.Substring(1, 1))
            {
                if (GetRook(sourceIndex, targetIndex, dif, sSquare, tSquare))
                {
                    return true;
                }
                return false;
            }

            // If diagonal GetBishop()
            else
            {
                if (GetBishop(sourceIndex, targetIndex, dif, pieceLegalMoves, sSquare, tSquare))
                {
                    return true;
                }
                return false;
            }
        }
        public bool GetKing(int sourceIndex, int targetIndex, int dif, string sSquare, int[] pieceLegalMoves)
        {   
            var piece = _board.board[sourceIndex];
            // Will use shadow board
            // Will add more filters her, 1)Will king be in check? 3)Is king in check? 3)Can castle?

            // For now just move in correct directions
            if (pieceLegalMoves.Contains(dif))
            {
                // White King
                if (piece[0].ToString() == "w")
                {
                    // Castling conditions --> 
                    Console.WriteLine($"King side castle:{WhiteKingCastle} Queen side castle:{WhiteQueenCastle}");
                    
                    // Conditions are growing, need to implement shadow board, "King will be in check next move, king is check"
                    Console.ReadKey();
                    if (dif == -2
                        && WhiteQueenCastle == true
                        && _board.board[sourceIndex - 1].ToString() == ".."
                        && _board.board[sourceIndex - 2].ToString() == ".."
                        && _board.board[sourceIndex - 3].ToString() == ".."
                        && _board.board[56] == "wR")
                    {
                        _board.board[56] = "..";
                        _board.board[59] = "wR";
                        // King side castle possible "short" 
                        // Moving the king, what a bout the Rook?
                        WhiteKingCastle = false;
                        WhiteQueenCastle = false;
                        return true;
                    }

                    else if (dif == +2
                        && WhiteKingCastle == true
                        && _board.board[sourceIndex + 1].ToString() == ".."
                        && _board.board[sourceIndex + 2].ToString() == ".."
                        && _board.board[63] == "wR")
                    {
                        _board.board[63] = "..";
                        _board.board[61] = "wR";
                        WhiteQueenCastle = false;
                        WhiteKingCastle = false;
                        // Queenside castle possible "long"
                        return true;
                    }
                    // Every other move
                    return false;
                }

                else if (piece[0].ToString() == "b")
                {
                    
                    // Castling conditions -->
                    // Queen side castle 
                    if (dif == -2
                        && BlackQueenCastle == true
                        && _board.board[sourceIndex - 1].ToString() == ".."
                        && _board.board[sourceIndex - 2].ToString() == ".."
                        && _board.board[sourceIndex - 3].ToString() == ".."
                        && _board.board[0] == "br")
                    {
                        _board.board[0] = "..";
                        _board.board[3] = "br";
                        BlackKingCastle = false;
                        BlackQueenCastle = false;   
                        return true;   
                    }

                    // King side castle
                    else if (dif == +2
                        && BlackKingCastle == true
                        && _board.board[sourceIndex + 1].ToString() == ".."
                        && _board.board[sourceIndex + 2].ToString() == ".."
                        && _board.board[7] == "br")
                    {
                        _board.board[7] = "..";
                        _board.board[5] = "br";
                        BlackKingCastle = false;
                        BlackQueenCastle = false;
                        return true;
                        // Kingside castle possible "short" 
                    }
                    return false;
                }
                return true;
            }
            return false;

            // Black KLing
        }
        public bool GetWhitePawn(int sourceIndex, int targetIndex, int dif, int[] pieceLegalMoves)
        {
            // -9 == right en passant and -7 == left en passant 
            
            if (pieceLegalMoves.Contains(dif))
            {
                if (dif == -7 || dif == -9)
                {
                    if (_board.board[targetIndex] != "..")
                    {
                        return true;
                    }
                    return false;
                }
                // Will add more filters here, 1)If it is a first move, 2)en passant with shadow pawn 
                return true;
            }
            return false;
        }

        public bool GetBlackPawn(int sourceIndex, int targetIndex, int dif, int[] pieceLegalMoves)
        {
            if (pieceLegalMoves.Contains(dif))
            {
                if (dif == +7 || dif == +9)
                {
                    if (_board.board[targetIndex] != "..")
                    {
                        return true;
                    }
                    return false;
                }
                // Will add more filters here, 1)If it is a first move, 2)en passant with shadow pawn
                return true;
            }
            return false;
        }


        // ply starts with fen, and will set based on history of moves.
        public string GetTurn()
        {
            if (Ply % 2 == 0)
            {
                return "white";
            }
            return "black";
        }

        // Test function
        void ShowCoordinates()
        {
            Console.WriteLine("Coordinates ");
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;
                    Console.Write(_board.coordinates[square] + "  ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }


        public void ShowBoard()
        {
            Console.Clear();
            Console.WriteLine("****************************************");
            try
            {
                if (History.Count > 0 || History != null)
                {
                    foreach (var moveNotation in History)
                    {
                        Console.WriteLine($"{moveNotation}");
                    }
                }
            }
            catch (NullReferenceException)
            { }
            Console.WriteLine();
            Console.WriteLine();
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;

                    Console.Write(_board.board[square] + "  ");
                }
                Console.WriteLine(ranks[rank]);
            }
            Console.WriteLine("\n A   B   C   D   E   F   G   H");
            Console.WriteLine();
            
            
            
        }

        /// <summary>
        /// Refactoring: 
        /// creating separate functions for piece moves
        /// </summary>

        /// <summary>
        /// Most of the logics / chess rules will be implemented here
        /// </summary>

        public bool MakeMove(string sSquare, string tSquare)
        {
            //Console.WriteLine($"Move is: {sSquare}-{tSquare}");
            int sourceIndex = _board.GetBoardIndex(sSquare);
            string piece = _board.board[sourceIndex];
            string pieceColor = piece.Substring(0, 1);
            int targetIndex = _board.GetBoardIndex(tSquare);
            string targetColor = _board.board[targetIndex].Substring(0, 1);

            //1) Checking if there is no same color piece at target square.
            if (pieceColor != targetColor)
            {
                int pieceIndex = GetPieceIndex(piece);

                // Getting list of legal moves based on type of the piece
                var pieceLegalMoves = LegalMoves[pieceIndex];
                int dif = targetIndex - sourceIndex;
                //2) piece rules
                switch (piece)
                {
                    case "wN" or "bn":
                        if (GetKnight(sourceIndex, targetIndex, dif, pieceLegalMoves))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    case "wB" or "bb":
                        // TODO: check for bug here --> :| 
                        if (GetBishop(sourceIndex, targetIndex, dif, pieceLegalMoves, sSquare, tSquare))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    case "wR" or "br":
                        if (GetRook(sourceIndex, targetIndex, dif, sSquare, tSquare))
                        {
                            if(BlackKingCastle == true || BlackQueenCastle == true)
                            {
                                if (sourceIndex == 0)
                                {
                                    BlackQueenCastle = false;
                                }
                                if (sourceIndex == 7)
                                {
                                    BlackKingCastle = false;
                                }
                            }
                            if(WhiteKingCastle == true || WhiteQueenCastle == true)
                            {
                                if (sourceIndex == 63)
                                {
                                    WhiteKingCastle = false;
                                }
                                if (sourceIndex == 56)
                                {
                                    WhiteQueenCastle = false;
                                }
                            }
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    // Easier to implement in separate functions.
                    case "wP":
                        if (GetWhitePawn(sourceIndex, targetIndex, dif, pieceLegalMoves))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;


                    case "bp":
                        if (GetBlackPawn(sourceIndex, targetIndex, dif, pieceLegalMoves))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;


                    case "wK" or "bk":
                        if (GetKing(sourceIndex, targetIndex, dif, sSquare, pieceLegalMoves))
                        {
                            Console.WriteLine($"Black --> KSide{BlackKingCastle}  QSide{BlackQueenCastle}");
                            Console.WriteLine($"White --> KSide{WhiteKingCastle}  QSide{WhiteQueenCastle}");
                            Console.ReadKey();
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    case "wQ" or "bq":
                        if (GetQueen(sourceIndex, targetIndex, dif, sSquare, tSquare, pieceLegalMoves))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;
                        break;
                }
            }
            return false; // Will be useful
        }

        public string GetMoveNotation(int sourceIndex, int targetIndex) 
        {
            string startSquare = _board.GetCoordinates(sourceIndex);
            string endSquare = _board.GetCoordinates(targetIndex);

            var move = ($"{startSquare}-{endSquare}");
            
            return move;
        }
        public void UpdateBoard(int sourceIndex, int targetIndex)
        {
             var moveNotation = GetMoveNotation(sourceIndex, targetIndex);
             History.Add((moveNotation));
            _board.board[targetIndex] = _board.board[sourceIndex];
            _board.board[sourceIndex] = "..";
        }
        // Getting input from user
        public string GetMove()
        {
            ShowBoard();
            Console.WriteLine("Enter your move: ");
            string move = Console.ReadLine();
            if (move != null)
            {
                try
                {
                    int sourceSquare = _board.GetBoardIndex(move.Substring(0, 2));
                    if (move.Count() == 4 &&
                        _board.coordinates.Contains(move.Substring(0, 2)) &&
                        _board.coordinates.Contains(move.Substring(0, 2)) &&
                        _board.board[sourceSquare] != "..")
                    { return move; }
                }
                catch (System.ArgumentOutOfRangeException) { }
            }
            return "Err";
        }
        public void HumanTurn()
        {
            string move = GetMove();
            if (move != "Err")
            {
                string sSquare = move.Substring(0, 2);
                string tSquare = move.Substring(2, 2);
                if (MakeMove(sSquare, tSquare))
                {
                    ShowBoard();
                }
            }

        }

        public void MachineTurn()
        {
            Console.WriteLine("Machine turn");
            Ply += 1;
            Console.ReadLine();
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                HumanTurn();
            }

        }
    }
}

/*
 * TODO:
 * Implement Fen parser --> Partially done: Piece positions, turn, castle rights.
 * Implement Castle rights for King and Rooks
 * 
 * Implement En passant, first 2square move for pawns
 * 
 * Implement Check
 * 
 * Implement King check in next move
 * 
 */
