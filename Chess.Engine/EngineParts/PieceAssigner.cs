using static Chess.EngineCore.Boards; // Why using static using statement? 
using Chess.EngineCore.Objects;


namespace Chess.EngineCore.EngineParts
{
    public class PieceAssigner
    {
        private readonly Piece _piece;
        private readonly Engine _engine;
        Board _board = new Board(); // why?
        public PieceAssigner(Piece piece, Engine engine)
        {           
            _piece = piece;
            _engine = engine;
            
        }

        public void AsignThepieces()
        {
          
            for (int i = 0; i < _board.board.Count(); i++)
            {
                string positioinCoords = _board.GetCoordinates(i);
                var row = positioinCoords[1].ToString();
                if (_board.board[i] != ".")
                {
                    if (_board.board[i] == "N")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "n")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "R")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "r")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "B")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "b")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "Q")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "q")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "P")
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

                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "p")
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
                        if (row == "7")
                        {
                            _piece.IsMoved = false;
                        }
                        else
                        {
                            _piece.IsMoved = true;
                        }
                        _engine.Position.Add(_piece);
                    }

                    else if (_board.board[i] == "K")
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
                        _engine.Position.Add(_piece);
                    }
                    else if (_board.board[i] == "k")
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
                        _engine.Position.Add(_piece);
                    }
                }
            }
            Console.WriteLine($"Pieces in position: {_engine.Position.Count}");
            Console.WriteLine("Press a key");
            Console.ReadKey();
            Console.WriteLine("Asigned pieces: \n");
            foreach (var piece in _engine.Position)
            {
                Console.WriteLine($"Name:" +
                    $"{piece.Name} Sign:{piece.Sign} Value:{piece.Value}, " +
                    $"PIndex:{piece.PositionIndex} PCoords:{piece.PositionCoords}");
            }
            Console.ReadKey();
        }
    }


}

