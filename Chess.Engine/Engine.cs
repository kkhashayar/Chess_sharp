using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Chess.EngineCore.Objects;
using static Chess.EngineCore.Boards;
using static Chess.EngineCore.EngineParts.MoveGen;

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

        // In Bishop and Rook moves
        public List<string> _pathelements = new List<string>();
        public string HumanColor { get; set; } = string.Empty; // Not sure about these two properties1
        public string MachineColor { get; set; } = string.Empty;
        public bool FenFlag { get; set; } = true;
        public int Ply { get; set; }
        public string? Turn { get; set; }
        public List<string>? History { get; set; }
        
        // En Passant Flag
        public bool LastMoveWasPawn { get; set; } = true;
        public bool PawnFirstMove { get; set; } = false;
        public int TargetSourceIndex { get; set; }
        // Castle rules
        public bool WhiteKingCastle { get; set; } = false;
        public bool WhiteQueenCastle { get; set; } = false;
        public bool BlackKingCastle { get; set; } = false;
        public bool BlackQueenCastle { get; set; } = false;

        // the rest of the data will be awailable on class level 
        public List<Piece> Position = new List<Piece>();


        public bool IsMoved = false; // To keep a track of En passnt, Castelling

        // Enumerable ranks in reverese, comes handy while looping the board
        public int[] ranks = new int[8] { 8, 7, 6, 5, 4, 3, 2, 1 };

        // All pieces as a string
        public List<string> allPieces = new List<string> { "P", "N", "B", "R", "Q", "K", "p", "n", "b", "r", "q", "k" };

        // Jagged array, holding directions of pieces, match based on indexes.
        private int[][] LegalMoves = new int[][]
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

        

        // Functions responsible for returning each piece moves

        public bool GetRook(MoveObject moveObject)
        {
            // Checks for Rook color and position to change the correspondence castle right
            _pathelements.Clear();
            // Move in files
            if (moveObject.OnBoardStartSquare.Substring(0, 1) == moveObject.OnBoardEndSquare.Substring(0, 1))
            {
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    // step is -8 --> moving up
                    for (int i = 1; i < 8; i++)
                    {
                        int path = moveObject.StartIndex - (i * 8);
                        if (path > moveObject.EndIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;

                }
                if (moveObject.StartIndex < moveObject.EndIndex)
                {
                    // step is +8 --> moving down
                    for (int i = 1; i < 8; i++)
                    {
                        int path = moveObject.StartIndex + (i * 8);
                        if (path < moveObject.EndIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }

                        if (path == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
            }


            // Move in ranks
            else if (moveObject.OnBoardStartSquare.Substring(1, 1) == moveObject.OnBoardEndSquare.Substring(1, 1))
            {
                // step is -1 --> moving left
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        int path = moveObject.StartIndex - (i * 1);
                        if (path > moveObject.EndIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                if (moveObject.StartIndex < moveObject.EndIndex)
                {
                    // step is +1 --> moving right
                    for (int i = 1; i < 8; i++)
                    {
                        int path = moveObject.StartIndex + (i * 1); // ref here 
                        if (path > moveObject.EndIndex)
                        {
                            _pathelements.Add(_board.board[path]);
                        }
                        if (path == moveObject.EndIndex)
                        {
                            break;
                        }
                    }
                    if (_pathelements.All(x => x == ".") || _pathelements.Count == -0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            // Castle special move --> 
            return false;
        }
        public bool GetBishop(MoveObject moveObject)
        {
            _pathelements.Clear();
            int result_of_9 = moveObject.difference % 9;
            int result_of_7 = moveObject.difference % 7;

            if (moveObject.difference % 9 == 0 || moveObject.difference % 7 == 0 && moveObject.OnBoardStartSquare.Substring(0, 1) != moveObject.OnBoardEndSquare.Substring(0, 1)
                && moveObject.OnBoardStartSquare.Substring(0, 2) != moveObject.OnBoardEndSquare.Substring(0, 2)// preventing the piece to move cross the files
                && moveObject.OnBoardStartSquare.Substring(1, 1) != moveObject.OnBoardEndSquare.Substring(1, 1)) // preventing the piece to move cross the ranks
            { // I need to parameterize them to one function with, different difs, and side to move
              // +9 up right
              // +7 up left
                if (moveObject.StartIndex > moveObject.EndIndex)
                {
                    if (moveObject.difference % 9 == 0)
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            int path = moveObject.StartIndex - (9 * i);
                            if (path >= moveObject.EndIndex && path <= _board.board.Count)
                            {
                                if (path != moveObject.EndIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                        {
                            return true;
                        }
                    }
                    if (moveObject.difference % 7 == 0)
                    {

                        for (int i = 1; i < 8; i++)
                        {
                            int path = moveObject.StartIndex - (7 * i);
                            if (path >= moveObject.EndIndex && path <= _board.board.Count)
                            {
                                if (path != moveObject.EndIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                        {

                            return true;
                        }
                    }
                }
                // -9 down left
                // -7 down right
                else
                {
                    if (moveObject.difference % 9 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int path = moveObject.StartIndex + (9 * i);
                            if (path <= moveObject.EndIndex && path <= _board.board.Count)
                            {
                                if (path != moveObject.EndIndex)
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                        if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                        {
                            return true;
                        }
                    }
                    if (moveObject.difference % 7 == 0)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            int path = moveObject.StartIndex + (7 * i);
                            if (path <= moveObject.EndIndex && path <= _board.board.Count)
                            {
                                if (path != moveObject.EndIndex) // this condition is already applied! in line above
                                {
                                    _pathelements.Add(_board.board[path]);
                                }
                            }
                        }
                    }
                    if (_pathelements.All(x => x == ".") || _pathelements.Count == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool GetKnight(MoveObject moveObject)
        {
            if (moveObject.LegalMoves.Contains(moveObject.difference))
            {
                return true;
            }
            return false;
        }
        // int sourceIndex, int targetIndex, int dif, string sSquare, string tSquare, int[] pieceLegalMoves
        public bool GetQueen(MoveObject moveObject)
        {
            // If in ranks or files GetRook()
            if (moveObject.OnBoardStartSquare.Substring(0, 1) == moveObject.OnBoardEndSquare.Substring(0, 1) || moveObject.OnBoardStartSquare.Substring(1, 1) == moveObject.OnBoardEndSquare.Substring(1, 1))
            {
                if (GetRook(moveObject))
                {
                    return true;
                }
                return false;
            }

            // If diagonal GetBishop()
            else
            {
                if (GetBishop(moveObject))
                {
                    return true;
                }
                return false;
            }
        }
        public bool GetKing(MoveObject moveObject)
        {   
            var piece = _board.board[moveObject.StartIndex];
            // Will use shadow board
            // Will add more filters her, 1)Will king be in check? 3)Is king in check? 3)Can castle?

            // For now just move in correct directions
            if (moveObject.LegalMoves.Contains(moveObject.difference))
            {
                // White King
                if (piece == "K")
                {
                    // Castling conditions --> 
                    Console.WriteLine($"King side castle:{WhiteKingCastle} Queen side castle:{WhiteQueenCastle}");
                    
                    // Conditions are growing, need to implement shadow board, "King will be in check next move, king is check"
                    Console.ReadKey();
                    if (moveObject.difference == -2
                        && WhiteQueenCastle == true
                        && _board.board[moveObject.StartIndex - 1].ToString() == "."
                        && _board.board[moveObject.StartIndex - 2].ToString() == "."
                        && _board.board[moveObject.StartIndex - 3].ToString() == "."
                        && _board.board[56] == "R")
                    {
                        _board.board[56] = ".";
                        _board.board[59] = "R";
                        // King side castle possible "short" 
                        // Moving the king, what a bout the Rook?
                        WhiteKingCastle = false;
                        WhiteQueenCastle = false;
                        return true;
                    }

                    else if (moveObject.difference == +2
                        && WhiteKingCastle == true
                        && _board.board[moveObject.StartIndex + 1].ToString() == "."
                        && _board.board[moveObject.StartIndex + 2].ToString() == "."
                        && _board.board[63] == "R")
                    {
                        _board.board[63] = ".";
                        _board.board[61] = "R";
                        WhiteQueenCastle = false;


                        WhiteKingCastle = false;
                        // Queenside castle possible "long"
                        return true;
                    }
                    // Every other move
                    return false;
                }

                else if (piece == "k")
                {
                    
                    // Castling conditions -->
                    // Queen side castle 
                    if (moveObject.difference == -2
                        && BlackQueenCastle == true
                        && _board.board[moveObject.StartIndex - 1].ToString() == "."
                        && _board.board[moveObject.StartIndex - 2].ToString() == "."
                        && _board.board[moveObject.StartIndex - 3].ToString() == "."
                        && _board.board[0] == "r")
                    {
                        _board.board[0] = ".";
                        _board.board[3] = "r";
                        BlackKingCastle = false;
                        BlackQueenCastle = false;   
                        return true;   
                    }

                    // King side castle
                    else if (moveObject.difference == +2
                        && BlackKingCastle == true
                        && _board.board[moveObject.StartIndex + 1].ToString() == "."
                        && _board.board[moveObject.StartIndex + 2].ToString() == "."
                        && _board.board[7] == "r")
                    {
                        _board.board[7] = ".";
                        _board.board[5] = "r";
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

        /*
         * Pawn unlike what we think has a rather complex logic
         * A) 2 Squares only as a first move. 
         * B) 1 Square even as a first move.
         * C) Captures diagonaly next diagonal square 
         * D) Captures EN passant pawn, Capturing empty square and pawn besied!
         */
        public bool GetWhitePawn(MoveObject moveObject)
        {
            // -9 == right en passant and -7 == left en passant 
            if (moveObject.LegalMoves.Contains(moveObject.difference))
            {
                if (moveObject.difference == -7 || moveObject.difference == -9)
                { 
                    var leftgap = moveObject.StartIndex += 15;
                    var rightgap = moveObject.StartIndex += 17;
                    if( leftgap == moveObject.StartIndex || rightgap == moveObject.StartIndex)
                    {
                        if (_board.board[moveObject.EndIndex].ToString() == "." && LastMoveWasPawn == true && PawnFirstMove == true)
                        {
                            if (_board.board[moveObject.StartIndex + 1] != null && _board.board[moveObject.StartIndex + 1].ToString() == "p")
                            {
                                _board.board[moveObject.StartIndex + 1] = ".";
                                return true;
                            }
                            else if (_board.board[moveObject.StartIndex - 1] != null && _board.board[moveObject.StartIndex - 1].ToString() == "p")
                            {
                                _board.board[moveObject.StartIndex - 1] = ".";
                                return true;
                            }
                            return false;
                        }
                    }   
                    // Capture condition
                    if (_board.board[moveObject.EndIndex].ToString() != ".")
                    {
                        return true;
                    }
                    return false;
                }
                // Forward condition 
                var row = _board.GetCoordinates(moveObject.StartIndex)[1].ToString();
                if(moveObject.difference == -16 && row == "2")
                {
                    return true;
                }
                else if(moveObject.difference == -8 && _board.board[moveObject.EndIndex].ToString() == ".")
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool GetBlackPawn(MoveObject moveObject)
        {
            if (moveObject.LegalMoves.Contains(moveObject.difference))
            {
                if (moveObject.difference == +7 || moveObject.difference == +9)
                {
                    // +9 == right en passant and +7 == left en passant 
                    var leftgap = TargetSourceIndex += 15;
                    var rightgap = TargetSourceIndex -= 17;
                    if (leftgap == TargetSourceIndex || rightgap == TargetSourceIndex)
                    {
                        if (_board.board[moveObject.EndIndex].ToString() == "." && LastMoveWasPawn == true && PawnFirstMove == true)
                        {
                            if (_board.board[moveObject.StartIndex + 1] != null && _board.board[moveObject.StartIndex + 1].ToString() == "P")
                            {
                                _board.board[moveObject.StartIndex + 1] = ".";
                                return true;
                            }
                            else if (_board.board[moveObject.StartIndex - 1] != null && _board.board[moveObject.StartIndex - 1].ToString() == "P")
                            {
                                _board.board[moveObject.StartIndex - 1] = ".";
                                return true;
                            }
                            return false;
                        }
                    }
                    // Capture condition
                    if (_board.board[moveObject.EndIndex].ToString() != ".")
                    {
                        return true;
                    }
                    return false;
                }
                // Forward condition 
                var row = _board.GetCoordinates(moveObject.StartIndex)[1].ToString();
                if (moveObject.difference == +16 && row == "7")
                {
                    return true;
                }
                else if (moveObject.difference == +8 && _board.board[moveObject.EndIndex].ToString() == ".")
                {
                    return true;
                }
                return false;
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
                        Console.WriteLine(LastMoveWasPawn);
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
            Console.WriteLine("\nA  B  C  D  E  F  G  H");
            Console.WriteLine();
            
            
            
        }

        /// <summary>
        /// Refactoring: 
        /// creating separate functions for piece moves   ,,,,
        /// </summary>

        /// <summary>
        /// Most of the logics / chess rules will be implemented here
        /// </summary>

        public bool MakeMove(string sSquare, string tSquare)
        {
            
            // Problem is here --> 
            //Console.WriteLine($"Move is: {sSquare}-{tSquare}");
            int sourceIndex = _board.GetBoardIndex(sSquare);
            string piece = _board.board[sourceIndex];
            string pieceColor = piece.Substring(0, 1);
            int targetIndex = _board.GetBoardIndex(tSquare);
            string targetColor = _board.board[targetIndex].Substring(0, 1);
            var pieceSymbol = _board.board[sourceIndex].ToString();
            
            //1) Checking if there is no same color piece at target square.
            if (pieceColor != targetColor)
            {
                int pieceIndex = GetPieceIndex(piece);

                // Getting list of legal moves based on type of the piece
                var pieceLegalMoves = LegalMoves[pieceIndex];
                int dif = targetIndex - sourceIndex;

                MoveObject moveObject = new MoveObject
                {
                    StartIndex = _board.GetBoardIndex(sSquare),
                    EndIndex = targetIndex,
                    StartCoordinate = _board.GetCoordinates(sourceIndex),
                    EndCoordinate = _board.GetCoordinates(targetIndex),
                    difference = dif,
                    LegalMoves = pieceLegalMoves,
                    OnBoardStartSquare = sSquare,
                    OnBoardEndSquare = tSquare,
                    pieceSYmbol = pieceSymbol

                };
                //2) piece rules
                switch (piece)
                {
                    case "N" or "n":
                        if (GetKnight(moveObject))
                        {
                            UpdateBoard(moveObject.StartIndex, moveObject.EndIndex);
                            return true;
                        }
                        return false;

                    case "B" or "b":
                        // TODO: check for bug here --> :| // moveObject.StartIndex, moveObject.EndIndex, moveObject.Difference, moveObject.StartCoordinate, moveObject.EndCoordinate
                        if (GetBishop(moveObject))
                        {
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    case "R" or "r":
                        if (GetRook(moveObject))
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
                    case "P":
                        if (GetWhitePawn(moveObject))
                        {
                            if(dif == -16)
                            {
                                PawnFirstMove = true;
                            }
                            else
                            {
                                PawnFirstMove = false;
                            }
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;


                    case "p":
                        if (GetBlackPawn(moveObject))
                        {
                            if (dif == 16)
                            {
                                PawnFirstMove = true;
                            }
                            else
                            {
                                PawnFirstMove = false;
                            }
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;


                    case "K" or "k":
                        if (GetKing(moveObject))
                        {
                            Console.WriteLine($"Black --> KSide{BlackKingCastle}  QSide{BlackQueenCastle}");
                            Console.WriteLine($"White --> KSide{WhiteKingCastle}  QSide{WhiteQueenCastle}");
                            Console.ReadKey();
                            UpdateBoard(sourceIndex, targetIndex);
                            return true;
                        }
                        return false;

                    case "Q" or "q":
                        if (GetQueen(moveObject)) // sourceIndex, targetIndex, dif, sSquare, tSquare, pieceLegalMoves
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
            TargetSourceIndex= sourceIndex;
            string startSquare = _board.GetCoordinates(sourceIndex);
            string endSquare = _board.GetCoordinates(targetIndex);
            var move = ($"{sourceIndex}{startSquare}-{endSquare}");
            return move;
        }
        public void UpdateBoard(int sourceIndex , int targetIndex)
        {
            if (_board.board[sourceIndex].ToString() == "p"|| _board.board[sourceIndex].ToString() == "P")
            {
                LastMoveWasPawn = true;
            }
            else
            {
                LastMoveWasPawn = false;
            }

            var moveNotation = GetMoveNotation(sourceIndex, targetIndex);
            var moveToAdd = moveNotation.Substring(2);
            // This will be addon flag
            var coordinate = moveNotation.Substring(0, 2);

            History.Add((moveNotation));
            _board.board[targetIndex] = _board.board[sourceIndex];
            _board.board[sourceIndex] = ".";
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
                        _board.board[sourceSquare] != ".")
                    { return move; }
                }
                catch (System.ArgumentOutOfRangeException) { }
            }
            return "Err";
        }

        public void WhiteTurn()
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

        public void BlackTurn()
        {
            //Console.WriteLine("Black turn, test mode will return control to user");
            
            //Console.ReadKey();
            WhiteTurn();
             
        }

        public string GetSide()
        {
            // First time running the game, setting the Turn from Fen
            // Next we set it from Ply
            // Ply sets from History.count
            if (FenFlag)
            {
                if (Turn == "w")
                {
                    Console.WriteLine($"It is a {Turn} turn");
                    Console.ReadKey();
                    WhiteTurn();
                }
                else
                {
                    //Console.WriteLine($"It is a {Turn} turn");
                    //Console.WriteLine("In testing mode, turn will back to edit mode");
                    //Console.ReadKey();
                    //BlackTurn();
                    WhiteTurn();
                    
                }
                FenFlag= false;
            }
            
            else if(History != null  || History.Count != 0)
            {
                Ply = History.Count;
                if(Ply %2 == 0)
                {
                    Turn = "w";
                }
                else
                {
                    Turn = "b";
                }
            }

            return Turn;
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Turn = GetSide();
                if (Turn == "w") { WhiteTurn(); }
                else { BlackTurn(); }
                Console.WriteLine(Turn);
                Console.ReadKey();
            }

        }
    }
}

/*
 * TODO:
 * Implement Fen parser --> Partially done: Piece positions, turn, castle rights. Done--> Position, Castle rights, turn
 * 
 * Implement Castle rights for King and Rooks. Done 
 * 
 * Implement En passant, first 2square move for pawns. Done 
 * 
 * Implement Check
 * 
 * Implement King check in next move
 * 
 */
