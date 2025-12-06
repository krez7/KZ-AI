using Bitboard;
using Krezb0t;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
MCTS root = new MCTS();


Console.WriteLine("Hello, World!");

for(int i = 0; i < 100; i++){
	Console.WriteLine(i);
}
//Console.WriteLine(test.ToAscii());
/*
for (int i = 0; i < 10000; i++)
{
    MagicBitBoard test = ChessBoard.parseFEN("rn1qkbnr/ppp1pppp/3p4/8/7P/2N2b2/PPPPPPP1/R1BQKB1R w KQkq - 0 4");
    //test.printChessBoard();
    //Console.WriteLine(test.ToAscii());
    root.process(test);
    Console.WriteLine($"{i}");
}

Move bMove = root.bestMove();
bMove.printMove();
*/
ChessBoard.printUInt64(0xFFFF);
/*
MagicBitBoard test = ChessBoard.parseFEN("2b2b1r/1pNpnkb1/b1p5/BpPpp1pb/K3P2p/5pNp/Q1P2PqP/R3n1BR w - - 0 1");
test.printChessBoard();
UInt64 posQ = test.pieceBB[4];
int n = ChessBoard.LS1BIndex(posQ);
List<Move> ez = new List<Move>();
test.getQueenMoves(n, test.boardOcc(true), true, ez);


foreach (Move move in ez)
{
    Console.WriteLine(move.to);
}
Console.WriteLine(ChessBoard.LS1BIndex(test.pieceBB[9]));
ChessBoard.printUInt64(test.getbishopAttackMap(n, test.boardOcc(true) | test.boardOcc(false)));
*/
Console.ReadLine();
