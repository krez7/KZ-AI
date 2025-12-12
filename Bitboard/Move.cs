using System;

namespace Bitboard
{
    public class Move
    {
        public char PieceType {get;}
        public int Square {get;}
        public int To {get;}
        public int? Promotion {get;}
        public bool Attack {get;}
        public bool CastlingMove {get;}
        public bool IsEnPassant {get;}


        public Move(char pt, int _square, int _to, bool _attack = false, int? _promotion=null, bool _castlingMove=false, bool _isEnPassant=false)
        {
            PieceType = pt; 
            Square = _square;
            To = _to;
            Promotion = _promotion;
            Attack = _attack;
            CastlingMove = _castlingMove;
            IsEnPassant = _isEnPassant;
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
