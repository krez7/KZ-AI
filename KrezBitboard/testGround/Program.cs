using KrezBitboard;

Console.WriteLine("Before:" + "{0}", ((UInt64)ChessBoard.Square.B1).ToString());
Console.WriteLine("Before:" + "{0}", 0x0F.ToString());
MagicBitBoard test = MagicBitBoard.parseFEN("r2qkbnr/2p1p1pp/1pn2p2/p5B1/P1bP4/2Np1N1P/1PPK1PP1/3R3R b kq - 1 12");

ChessBoard.printBoard(test.boardOcc(true) | test.boardOcc(false));
test.printChessBoard();
Console.WriteLine("r2qkbnr/2p1p1pp/1pn2p2/p5B1/P1bP4/2Np1N1P/1PPK1PP1/3R3R b kq - 1 12");
//List<Move> aaaaa = new List<Move>();
//test.getKingMoves(12, test.boardOcc(true), true, aaaaa);

/*
foreach(Move move in aaaaa)
{
    Console.Write(move.to);
    Console.WriteLine(" " + move.attack);
}
*/
