using System;


namespace Bitboard
{
    public class Move(char pt, int _square, int _to, bool _attack = false, int? _promotion=null, bool _castlingMove=false, bool _isEnPassant=false)
    {
        public char PieceType {get;} = pt; 
        public int Square {get;} = _square;
        public int To {get;} = _to;
        public int? Promotion {get;} = _promotion;
        public bool Attack {get;} = _attack;
        public bool CastlingMove {get;} = _castlingMove;
        public bool IsEnPassant {get;} = _isEnPassant;

        public static (int, int) stringToMove(string strMove)
        {
            return (1, 1);
        }

        public void printMove()
        {
            Console.WriteLine("Piece type : " + PieceType);

            Console.WriteLine("From : " + Square);

            Console.WriteLine("To : " + To);

            Console.WriteLine("Attack : " + Attack);

            Console.WriteLine("Castling : " + CastlingMove);
        }

    }


}
