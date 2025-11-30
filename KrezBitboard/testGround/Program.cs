using KrezBitboard;

//Console.WriteLine("Before:" + "{0}", ((UInt64)ChessBoard.Square.B1).ToString());
//Console.WriteLine("Before:" + "{0}", 0x0F.ToString());

//https://lichess.org/study/7WCFYt0R/ePy2DmAC#62
//Initial board
MagicBitBoard test = new MagicBitBoard("2k3r1/p7/1pp4p/2p1b2q/4PpN1/1P1P1Q1P/P1P5/5RK1 w - - 0 32");
test.printChessBoard();

/*
for(int i = 55; i < 64; i++){
	MagicBitBoard.printUInt64(MagicBitBoard.pawnAttacks[0, i]);
	Console.WriteLine("\n");
}
*/

//MagicBitBoard.printUInt64(MagicBitBoard.rookAttacksOcc(test.boardOcc(MagicBitBoard.white)|test.boardOcc(MagicBitBoard.black), 2));
//Best move 



//List<Move> aaaaa = new List<Move>();
//test.getKingMoves(12, test.boardOcc(true), true, aaaaa);
/*
foreach(Move move in aaaaa)
{
    Console.Write(move.to);
    Console.WriteLine(" " + move.attack);
}
*/
