using System;


namespace Bitboard
{
    public class Move(char pt, int _square, int _to, bool _attack = false, int? _promotion=null, bool _castlingMove=false, bool _isEnPassant=false)
    {
        public char pieceType = pt;
        public int square = _square;
        public int to = _to;
        public int? promotion = _promotion;
        public bool attack = _attack;
        public bool castlingMove = _castlingMove;
        //public bool isEnPassant = _isEnPassant;
        public static (int, int) stringToMove(string strMove)
        {
            return (1, 1);
        }

        public void printMove()
        {
            Console.WriteLine("Piece type : " + pieceType);

            Console.WriteLine("From : " + square);

            Console.WriteLine("To : " + to);

            Console.WriteLine("Attack : " + attack);

            Console.WriteLine("Castling : " + castlingMove);
        }

    }


}
