using KrezBitboard;
using System;
using System.Collections.Generic;
using static KrezBitboard.ChessBoard;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Before:" + "{0}", ((UInt64)ChessBoard.Square.B1).ToString());
Console.WriteLine("Before:" + "{0}", 0x0F.ToString());
Console.WriteLine(typeof(UInt64));
//ChessBoard test = ChessBoard.parseFEN("3rr1k1/1ppq1pp1/1b1p1nb1/pP1P4/P1N1P3/6Pp/1BQ2PBK/3RR3 w - - 88 25");
//MagicBitBoard test = MagicBitBoard.parseFEN("2kr1b1r/pp2nppp/2n5/qB1p4/5Pb1/P1N1BN2/1PP1Q1PP/2KR3R w - - 1 13");
MagicBitBoard test = MagicBitBoard.parseFEN("r2qkbnr/2p1p1pp/1pn2p2/p5B1/P1bP4/2Np1N1P/1PPK1PP1/3R3R b kq - 1 12");
//ChessBoard test = new ChessBoard();
//UInt64 a = 0;

//UInt64 a = (test.b | test.k | test.q | test.r | test.n);
//Console.WriteLine(a.ToString());
//Console.WriteLine(Convert.ToString((long)index.B1, 2));
//ChessBoard.printBoard(a);
//ChessBoard.printBoard(test.K);
//ChessBoard.printBoard(test.K<<(8));
//ChessBoard.printBoard(test.attPatternKing(false));
//Console.Write("                        ");
//ChessBoard.printBoard((UInt64)0x000000000000FF00);
//ChessBoard.printBoard((UInt64)0x000000000000FF00 << (9));
//UInt64 test2 = ChessBoard.K;
//Pieces test3 = new Pieces();
/*for (int i = 0; i < 64; i++) {
    Console.WriteLine(Convert.ToString(test3.rookAttackRanks[i], 2).PadLeft(8, '0'));
}




int i = 0;

foreach(ulong nb n Enum.GetValues(typeof(index))){
    //Console.WriteLine(nb);
    Console.WriteLine(i);
    i++;
}



UInt64 ddd = 1;
UInt64 dddd = 0;

for (int i = 0; i < 8; i++) {
    dddd |= ddd << (9*i); 
}
ChessBoard.printBoard(dddd);
Console.WriteLine(dddd);
*/

/*
for (int i = 1; i < 9; i++) { ChessBoard.printBoard(test3.knightAttacks[(i-1) * 8 +5]);
    Console.WriteLine("");
}

//ChessBoard.printBoard(test.LS1B(ChessBoard.diag));


Console.WriteLine(ChessBoard.bitCount((UInt64)ChessBoard.Square.A1));

UInt64 aaa = ((UInt64)1 << ChessBoard.bitCount(test3.bishopAttacks[32]));

for (int i = 0; i < (int)aaa; i++)
{
    ChessBoard.printBoard(test3.listOccupancy(32, i));
}
/*
Console.WriteLine(ChessBoard.bitCount(test3.bishopAttacks[32]));
Console.WriteLine(aaa);

Console.WriteLine((int)aaa);

//ChessBoard.printBoard(test3.bishopAttacks[35]);
//Console.Write(ChessBoard.bitCount(test3.bishopAttacks[35]));

for (int i = 0; i < 8; i++)
{

   //ChessBoard.printBoard(test3.kingAttack(test3.square[i]));
   //Console.WriteLine("");
     
    for (int j = 0; j < 8; j++)
        {
                //Console.Write(ChessBoard.bitCount(test3.rookAttacks((7-i)*8 + 7-j)) + ",");
                ChessBoard.printBoard(ChessBoard.generateMagic());
         }
    Console.WriteLine();
    
    //Console.WriteLine(RandXOR.GetRandNumber());
}


//ChessBoard.printBoard(test3.rookAttacksOcc(test3.square[42], 44));*/
//81065962094218304
/*
for(int i = 0; i < 8; i++) {
    Console.WriteLine(test3.findMagic(100000000, i, test3.getOccupancies(i, true), test3.bishopAttacks[i], true));
}

for (int i = 0; i < 8; i++)
{

    //ChessBoard.printBoard(test3.kingAttack(test3.square[i]));
    //Console.WriteLine("");

    for (int j = 0; j < 8; j++)
    {
        Console.Write(test3.findMagic(100000000, (i*8 +j), test3.getOccupancies((i * 8 + j), false), test3.rookAttacks(i * 8 + j), false) + ", ");
    }
    Console.WriteLine();

    //Console.WriteLine(RandXOR.GetRandNumber());
}
*/

//ChessBoard.printBoard(test3.getBishopMoves(0, test3.square[36]));

/*
ChessBoard.printBoard(test3.rookAttacksOcc(Pieces.square[43], 35));
for(int i = 0; i < 8; i++) { ChessBoard.printBoard(test.files[i]); }
*/


//Console.WriteLine(test.halfMoves);

//ChessBoard.printBoard(test.enPassant);

//ChessBoard.printBoard(test.pieceBB[(int)ChessBoard.Material.P]);
//ChessBoard.printBoard(test.pieceBB[(int)ChessBoard.Material.p]);

ChessBoard.printBoard(test.boardOcc(true) | test.boardOcc(false));
test.printChessBoard();
Console.WriteLine("r2qkbnr/2p1p1pp/1pn2p2/p5B1/P1bP4/2Np1N1P/1PPK1PP1/3R3R b kq - 1 12");
List<Move> aaaaa = new List<Move>();
test.getKingMoves(12, test.boardOcc(true), true, aaaaa);

foreach(Move move in aaaaa)
{
    Console.Write(move.to);
    Console.WriteLine(" " + move.attack);
}


/*
for (int i = 0; i < 8; i++)
{

    //ChessBoard.printBoard(test3.kingAttack(test3.square[i]));
    //Console.WriteLine("");

    for (int j = 0; j < 8; j++)
    {
        int pos = i * 8 + j;
        //Console.WriteLine(test.findMagic(100000000, (pos), test.getOccupancies(pos, true), test.bishopAttacks[pos], true) + ", ");
        //UInt64[] raaa = test.getOccupancies(pos, false);
        //foreach(UInt64 raaa2 in raaa) { ChessBoard.printBoard(raaa2); }
        Console.WriteLine(test.findMagic(100000000, (pos), test.getOccupancies(pos, false), test.rookAttacks(pos), false) + ", ");
        //ChessBoard.printBoard(test.bishopAttacksOcc(MagicBitBoard.square[i * (8 + 1) + (7 - j)], i * 8 + (7 - j)));
        //ChessBoard.printBoard(test.rookAttacks(pos));
        //Console.WriteLine(pos);
    }
    Console.WriteLine();

    //Console.WriteLine(RandXOR.GetRandNumber());
}
*/
//UInt64[] raaa = test.getOccupancies(0, false);


/*
for(int i =345; i < 356; i++)
{
    Console.WriteLine("-----------");
    ChessBoard.printBoard(raaa[i]);

    ChessBoard.printBoard(test.rookAttacksOcc(raaa[i], 35));
}




Dictionary<UInt64, int> testdict = new Dictionary<UInt64, int>();
foreach (UInt64 raaa2 in raaa) { 
    testdict.Add(raaa2, 1);
    
}
Console.WriteLine(raaa.Length);
Console.WriteLine(testdict.Count);
*/

//for (int i = 0; i < 8; i++) { Console.WriteLine(test.findMagic(100000000, i, test.getOccupancies(i, false), test.rookAttackse[i], false)); }

//Console.WriteLine(test.findMagic(100000000, (0), test.getOccupancies(0, true), test.bishopAttacks[0], true));