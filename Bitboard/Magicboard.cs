using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;


namespace Bitboard
{
    public class Magicboard : Chessboard
    {      
        static UInt64[] bishopAttacks;
        static UInt64[] knightAttacks;
        //public UInt64[] rookAttackse;

        /*public UInt64[,] bishopOccupancy;
        public Byte[] rookOccupancyRanks;
        public UInt64[] rookOccupancyLines;*/
        //lookup tables
        static UInt64[,]  rookAttackTable = new UInt64[64,4096];

        static UInt64[,] bishopAttackTable = new UInt64[64,512];

        static UInt64[] magicsBishop = [297272778304864576, 4617351810504655876, 4525591006554208, 2260875080368132, 565217697202730, 9224537656755488770, 4651093631902417154, 2816953152569858,
13835137359772942880, 4613410062331873537, 324558311208845824, 9290916238196770, 36353681408016, 1151590473752, 18726075238912, 10664594578448912640,
4701971385026085024, 1130332582609416, 2666427091645759504, 81100295526170944, 43910923158553088, 1830738668324898, 142940941353509, 9223943921430954248,
19351417837987840, 2287061629571217, 74313792032354336, 2956621952544014338, 9223654611344179200, 144186106592665608, 1157570343284573188, 90153975185179648,
2349770972913206592, 2547029425260544, 73183665811884613, 2161771835983792640, 594475425963770112, 1297186269216182528, 9223939385962271232, 360851203683321984,
577749518049615872, 648816348889088514, 72343472060761088, 311876478117609992, 4836892461156556864, 351282016844661280, 83459531832558090, 1134697158148160,
2310628702362292256, 36318278066728, 1119510859776, 5240967529472, 5278229829295153152, 2310416995436235048, 2308167960944185344, 1191221102408646657,
2271595586946048, 703842131117072, 2256751919927372, 11258999072752128, 20301382999474693, 4665870235180597376, 4612002686400200976, 4692772806286114952];

        static UInt64[] magicsRook =    [36035949213274145,
                                        162129861731950592,
                                        10412331684866297856,
                                        3602888516244087296,
                                        2341880602359496833,
                                        72066407310884866,
                                        6485185662503911688,
                                        4647715915094315136,
                                        506795696113860608,
                                        81205668224172178,
                                        289919296948158977,
                                        5066695610220561,
                                        5188287525407426560,
                                        3463831357875552768,
                                        577586962056216848,
                                        9223653516126494786,
                                        4611831703722263144,
                                        282575033614340,
                                        9800402336487580288,
                                        4611827305939994624,
                                        706436355064833,
                                        985712207922176,
                                        292875263056806144,
                                        2253999109899396,
                                        144185631981994016,
                                        70379483693184,
                                        145276291683188872,
                                        7570551044477288576,
                                        74309432507432964,
                                        288511864013849600,
                                        576504749954764802,
                                        4504157973135620,
                                        36028934495667268,
                                        35253095759936,
                                        153139981672521728,
                                        22535727929639424,
                                        11559187234424832,
                                        11529355800745083392,
                                        1299429238583066880,
                                        4683814532072603908,
                                        9241456806258049026,
                                        2319358481557372928,
                                        576478481945264192,
                                        4621537711333900298,
                                        8796176973840,
                                        27023796854620288,
                                        578714751157174400,
                                        72075753163980801,
                                        79715163505152,
                                        288547104224182592,
                                        2315430751145590912,
                                        75444100589944960,
                                        2306424650999006464,
                                        9011599448866944,
                                        10520989583078982656,
                                        1188950868565754368,
                                        289497026736112897,
                                        5192931983050686465,
                                        9223794309452145858,
                                        2257297909223425,
                                        9295993201605215234,
                                        4612249002757623810,
                                        9511620013916717316,
                                        580964971481990146];

        static int[] bishopOccRelevancy = [6,5,5,5,5,5,5,6,
                                          5,5,5,5,5,5,5,5,
                                          5,5,7,7,7,7,5,5,
                                          5,5,7,9,9,7,5,5,
                                          5,5,7,9,9,7,5,5,
                                          5,5,7,7,7,7,5,5,
                                          5,5,5,5,5,5,5,5,
                                          6,5,5,5,5,5,5,6];


        static int[] rookOccRelevancy = [12,11,11,11,11,11,11,12,
                                    11,10,10,10,10,10,10,11,
                                    11,10,10,10,10,10,10,11,
                                    11,10,10,10,10,10,10,11,
                                    11,10,10,10,10,10,10,11,
                                    11,10,10,10,10,10,10,11,
                                    11,10,10,10,10,10,10,11,
                                    12,11,11,11,11,11,11,12];

        public Magicboard()
        {
            initRandKey();
        }

        public Magicboard(Chessboard board){
            pieceBB = board.pieceBB;
            side = board.side;
            enPassant = board.enPassant;
            halfMoves = board.halfMoves;
            initRandKey();
        }

        public Magicboard(string FEN) : base(FEN) {
            initRandKey();
        }

        static Magicboard()
        {
            knightAttacks = new UInt64[64];

            knightAttacks[0] = (UInt64)Square.F2 | (UInt64)Square.G3;
            knightAttacks[1] = (knightAttacks[0] << 1) | (UInt64)Square.H3;
            knightAttacks[2] = (knightAttacks[1] << 1) | (UInt64)Square.H2;
            for(int i = 3; i < 8; i++)
            {
                knightAttacks[i] = (knightAttacks[i - 1] << 1) & (~ranks[3] & ~files[7]);
            }

            knightAttacks[8] = (knightAttacks[0] << 8) | (UInt64)Square.F1;
            knightAttacks[16] = (knightAttacks[8] << 8) | (UInt64)Square.G1;
            knightAttacks[9] = (knightAttacks[1] << 8) | (UInt64)Square.E1;
            knightAttacks[17] = (knightAttacks[9] << 8) | (UInt64)Square.F1 | (UInt64)Square.H1;


            knightAttacks[15] = (knightAttacks[7] << 8) | (UInt64)Square.C1;
            knightAttacks[23] = (knightAttacks[15] << 8) | (UInt64)Square.B1;
            knightAttacks[14] = (knightAttacks[6] << 8) | (UInt64)Square.D1;
            knightAttacks[22] = (knightAttacks[14] << 8) | (UInt64)Square.C1 | (UInt64)Square.A1;

            knightAttacks[10] = (knightAttacks[2] << 8) | (UInt64)Square.D1 | (UInt64)Square.H1;
            knightAttacks[18] = (knightAttacks[10] << 8) | (UInt64)Square.E1 | (UInt64)Square.G1;

            for(int j = 3; j < 6; j++)
            {
                knightAttacks[8 + j] = (knightAttacks[8 + (j - 1)] << 1);
                knightAttacks[16 + j] = (knightAttacks[16 + (j - 1)] << 1);
            }
            for (int i = 4; i < 9; i++)
            {
                knightAttacks[(i-1) * 8] = (knightAttacks[(i - 2) * 8] & ~ranks[7]) << 8;
                knightAttacks[(i-1) * 8 + 1] = (knightAttacks[(i - 2) * 8 + 1] & ~ranks[7]) << 8;

                knightAttacks[(i - 1) * 8 + 2] = (knightAttacks[(i - 2) * 8 + 2] & ~ranks[7]) << 8;

                for(int j = 3; j < 7; j++) { knightAttacks[(i - 1) * 8 + j] = (knightAttacks[(i - 1) * 8 + (j - 1)] << 1); }

                knightAttacks[i * 8 - 1] = (knightAttacks[(i - 1) * 8 - 1] & ~ranks[7]) << 8;
                knightAttacks[i * 8 - 2] = (knightAttacks[(i - 1) * 8 - 2] & ~ranks[7]) << 8;
            }

            //rookAttackse = new UInt64[64];

            bishopAttacks = new UInt64[64];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    UInt64 piecePos = (UInt64)1 << (j + (8 * i));

                    bishopAttacks[j + (i * 8)] = piecePos;
                    for (int k = 1; (k + i < 8) && (j - k >= 0); k++)
                    {
                        bishopAttacks[j + (i * 8)] |= bishopAttacks[j + (i * 8)] << 7;
                    }
                    UInt64 temp = piecePos;

                    for (int k = 1; (k + i < 8) && (k + j < 8); k++)
                    {
                        temp |= temp << 9;
                    }
                    bishopAttacks[j + (i * 8)] |= temp;

                    temp = piecePos;

                    for (int k = 1; (i - k >= 0) && (k + j < 8); k++)
                    {
                        temp |= temp >> 7;
                    }
                    bishopAttacks[j + (i * 8)] |= temp;

                    temp = piecePos;

                    for (int k = 1; (i - k >= 0) && (j - k >= 0); k++)
                    {
                        temp |= temp >> 9;
                    }
                    bishopAttacks[j + (i * 8)] |= temp;

                    bishopAttacks[j + (i * 8)] &= ~piecePos;

                    /// rook
                    /// 
                    /*
                    UInt64 attack;
                    UInt64 ten = ((UInt64)1 << j); //square index singleton
                    ten = (te << (8 * i));
                    attack = (ranks[i] & ~files[0] & ~files[7]) | (files[7 - j] & ~ranks[0] & ~ranks[7]);
                    //rookAttacks[j + (i * 8)] = (ranks[i]) | (files[7-j]);
                    attack &= ~te;
                    rookAttackse[j + (i * 8)] = attack;
                    */
                }



            }

            /*
            rookOccupancyRanks = new Byte[64];
            for (int i = 2; i < 128; i = i + 2)
            {
                rookOccupancyRanks[i / 2 - 1] = (byte)i;
            }

            rookOccupancyLines = new UInt64[64];
            for (int i = 0; i < 64; i++)
            {
                rookOccupancyLines[i] = (rookOccupancyRanks[i] * diag) & files[0];
            }
            */

            //init slider pieces attack tables

            for (int i = 0; i < 64; i++)
            {
                int relevantRook = rookOccRelevancy[i];
                int relevantBishop = bishopOccRelevancy[i];

                int occIndexRook = (1 << relevantRook);
                int occIndexBishop = (1 << relevantBishop);

                for (int j = 0; j < occIndexRook; j++)
                {
                    UInt64 occupancy = listOccupancy(i, j, false);
                    int magicId = (int)((occupancy * magicsRook[i]) >> (64 - relevantRook));
                    
                    rookAttackTable[i,magicId] = rookAttacksOcc(occupancy, i);

                }

                for (int j = 0; j < occIndexBishop; j++)
                {
                    UInt64 occupancy = listOccupancy(i, j, true);
                    int magicId = (int)((occupancy * magicsBishop[i]) >> (64 - relevantBishop));

                    bishopAttackTable[i, magicId] = bishopAttacksOcc(occupancy, i);

                }
            }
        }

        UInt64 kingAttacks(int sq) // formerly attPatternKing
        {
            UInt64 pos = square[sq];
            UInt64 pattern = 0;
            /*
            if (c) { pos = K; }
            else { pos = k; }
            */
            if ((pos & ranks[0]) == 0)
            { //type ?
                pattern = pos >> 8;
                if ((pos & files[0]) == 0)
                {
                    pattern |= pos << (1);
                    pattern |= (pos >> (7));

                }
                if ((pos & files[7]) == 0){
                    pattern |= pos >> (1);
                    pattern |= (pos >> (9));
                }

            }

            if ((pos & ranks[7]) == 0)
            { //type ?
                pattern |= pos << 8;
                if ((pos & files[0]) == 0)
                {
                    pattern |= pos << (1);
                    pattern |= (pos << (9));

                }
                if ((pos & files[7]) == 0)
                {
                    pattern |= pos >> (1);
                    pattern |= (pos << (7));
                }
            }

            return pattern;
        }

        /*
        public UInt64 attPatternPawns(bool c){ //supposition : aucuns pions sur case de promotion (first move à faire aussi)
            UInt64 pos;
            if(c){pos = P;}
            else{pos = p;}
            UInt64 obs = boardCol(!c);
            UInt64 pattern = (pos<<(8)) & (~obs);


        }
        */
 
        static UInt64 bishopAttacksOcc(UInt64 mask, int pos)
        {
            
            UInt64 squarePos = square[pos];
            mask &= ~squarePos;
            int i = pos / 8;
            int j = (pos % 8);
            //square index singleton

            UInt64 attack = 0;
            UInt64 temp = 0;

            for (int k = 0; (k < 7 - i) && (k < j); k++){
                temp |= (squarePos << (7*(k+1)));
                if((mask & temp) != 0){
                    break;
                }
            }

            attack |= temp;

            temp = squarePos;

            for (int k = 0; (k < 7 - i) && (k < (7 - j)); k++) 
            {
                temp |= (squarePos << (9 * (k + 1)));
                if ((mask & temp) != 0)
                {
                    break;
                }
            }
            
            attack |= temp;

            temp = squarePos;

            for (int k = 0; (k < i) && (k < (7 - j)); k++) {
                temp |= (squarePos >> (7 * (k + 1)));
                if ((mask & temp) != 0)
                {
                    break;
                }
            }
           
            attack |= temp;

            temp = squarePos;

            for (int k = 0; (k < i) && (k < j); k++){
                temp |= (squarePos >> (9 * (k + 1)));
                if ((mask & temp) != 0)
                {
                    break;
                }
            }

            attack |= temp;
            attack &= ~squarePos;
            return attack;
        }
        static UInt64 rookAttacks(int pos)
        {
            int i = pos / 8;
            int j = (pos % 8);
            UInt64 attack;
            UInt64 te = ((UInt64)1 << j); //square index singleton
            te = (te << (8 * i));
            attack = (ranks[i] & ~files[0] & ~files[7]) | (files[7 - j] & ~ranks[0] & ~ranks[7]);
            //rookAttacks[j + (i * 8)] = (ranks[i]) | (files[7-j]);
            attack &= ~te;
            return attack;


        }
        
        static UInt64 rookAttacksOcc(UInt64 mask, int pos)
        {
            mask &= ~square[pos];
            int i = pos / 8; //vérifer validité
            int j = 7-(pos % 8); //vérifer validité
            //Console.WriteLine(j);
            UInt64 attack = (ranks[i]) | (files[j]);
            //square index singleton
            //rookAttacks[j + (i * 8)] = (ranks[i]) | (files[7-j]);
            while (mask != 0)
            {
                int lastBit = LS1BIndex(mask);
                if ((square[lastBit] & files[j]) != 0) { //vérifier
                    if (lastBit > pos)
                    {
                        for (int k = 1; (lastBit + (k * 8)) < 64; k++)
                        {
                            attack &= ~square[lastBit + (k * 8)];
                        }
                    }
                    else
                    {
                        for (int k = 1; (lastBit - (k * 8)) >= 0; k++)
                        {
                            attack &= ~square[lastBit - (k * 8)];
                        }
                    }
                }
                
                else if ((square[lastBit] & ranks[i]) != 0)
                {
                    if (lastBit > pos)
                    {
                        for (int k = 1; (lastBit + k) < ((i+1)*8); k++)
                        {
                            attack &= ~square[lastBit + k];
                        }
                    }
                    else
                    {
                        for (int k = 1; (lastBit - k) >= (i*8); k++)
                        {
                            attack &= ~square[lastBit - k];
                        }
                    }
                }
                mask &= ~square[lastBit];            
            }
            attack &= ~square[pos];
            return attack;
        }

        static UInt64 listOccupancy(int position, int permutationIndex, bool slidingPiece)
        {
            UInt64 boardPos = ((UInt64)1 << position);
            Debug.Assert(bitCount(boardPos)==1);
            UInt64 attacks = slidingPiece ? bishopAttacks[position] : rookAttacks(position);
            int bitsInAttack = bitCount(attacks);
            UInt64 occupancy = 0;
            for (int i = 0; i < bitsInAttack; i++) {
                UInt64 lastPos = LS1B(attacks);
                attacks = attacks & ~lastPos;

                if((permutationIndex & (1 << i)) != 0)
                {
                    occupancy |= lastPos;
                }
            }
            return occupancy;
        }

        UInt64[] getOccupancies(int pos, bool slidingPiece)
        {
            UInt64 attackMask = slidingPiece ? bishopAttacks[pos] : rookAttacks(pos);
            int relevance = slidingPiece ? bishopOccRelevancy[pos] : rookOccRelevancy[pos];
            int permutationsNB = (1 << (relevance));
            UInt64[] permutations = new UInt64[permutationsNB];
            for (int i = 0; i < permutationsNB; i++)
            {
                permutations[i] = listOccupancy(pos, i, slidingPiece);
            }
            return permutations;
        }


        UInt64 findMagic(int iterations, int square, UInt64[] occupancies, UInt64 attackMask, bool slidingPiece) // 1 for bishop, 2 for rook
        {
            UInt64[] occAttacks = new UInt64[occupancies.Length];
            
            int relevance = slidingPiece ? bishopOccRelevancy[square] : rookOccRelevancy[square];
            //Console.WriteLine(relevance);
            for (int i = 0; i < occupancies.Length; i++)
            {
                //Chessboard.printBoard(occupancies[i]);
                occAttacks[i] = slidingPiece ? bishopAttacksOcc(occupancies[i], square) : rookAttacksOcc(occupancies[i], square);
                //Chessboard.printBoard(occAttacks[i]);

            }
            
            for (int i = 0; i < iterations; i++) {
                UInt64[] usedAttacks = new UInt64[occupancies.Length];
                UInt64 magicNumber = generateMagic();
                //if(magicNumber == 140002000100040) { Console.WriteLine("wtf"); }
                if (bitCount((attackMask * magicNumber) & ranks[7]) < 6) continue;
                bool failed = false;
                for(int j = 0; !failed && (j < occupancies.Length); j++)
                {
                    int magicIndex = (int)((occupancies[j] * magicNumber) >> (64 - relevance));

                    if (usedAttacks[magicIndex] == 0)
                    {
                        usedAttacks[magicIndex] = occAttacks[j];
                    }

                    else if (usedAttacks[magicIndex] != occAttacks[j]) {
                        failed = true;
                    }
                }
                if (!failed) {
                    return magicNumber;
                }

           }
           return 0;   
        }

        public UInt64 getbishopAttackMap(int sq, UInt64 occupancy)
        {
            occupancy &= bishopAttacks[sq];
            occupancy *= magicsBishop[sq];
            occupancy >>= 64 - bishopOccRelevancy[sq];
            return bishopAttackTable[sq, occupancy];
        }

        public UInt64 getRookAttackMap(int sq, UInt64 occupancy)
        {
            occupancy &= rookAttacks(sq);
            occupancy *= magicsRook[sq];
            occupancy >>= 64 - rookOccRelevancy[sq];
            return rookAttackTable[sq, occupancy];
        }

        UInt64 getPawnThreat(int sq, bool color){
            UInt64 threatPawn = 0, squareUint64 = square[sq];
            if(color){
                if(sq%8 != 0) threatPawn |= square[sq + 7];
                if((sq+1)%8 != 0) threatPawn |= square[sq + 9];
                if(enPassant != -1){
                    if(enPassant - sq == 1) threatPawn |= square[sq + 1];
                    if(enPassant - sq == -1) threatPawn |= square[sq - 1];
                }
            }

            else{
                if(sq%8 != 0) threatPawn |= square[sq - 9];
                if((sq+1)%8 != 0) threatPawn |= square[sq - 7];
                if(enPassant != -1){
                    if(enPassant - sq == 1) threatPawn |= square[sq + 1];
                    if(enPassant - sq == -1) threatPawn |= square[sq - 1];
                }
            }

            return threatPawn;
        } 

        UInt64 GetThreatToKing(bool color, UInt64 occupancy)
        {
            UInt64 posOppP = !color ? pieceBB[0] : pieceBB[6];
            UInt64 posOppN = !color ? pieceBB[1] : pieceBB[7];
            UInt64 posOppB = !color ? pieceBB[2] : pieceBB[8];
            UInt64 posOppK = !color ? pieceBB[3] : pieceBB[9];
            UInt64 posOppQ = !color ? pieceBB[4] : pieceBB[10];
            UInt64 posOppR = !color ? pieceBB[5] : pieceBB[11];
            
            int sqKing = LS1BIndex(color ? pieceBB[3] : pieceBB[9]);
            UInt64 threatMap = 0;
            //posOppP &= attackMap;
            while (posOppP != 0)
            {
                int n = LS1BIndex(posOppP);
                posOppP &= ~square[n];
                threatMap |= getPawnThreat(n, !color);
            }

            while (posOppN != 0)
            {
                int n = LS1BIndex(posOppN);
                posOppN &= ~square[n];
                threatMap |= knightAttacks[n];
            }

            UInt64 nonKingOccupancy = occupancy & ~square[sq];
            while (posOppB != 0)
            {
                int n = LS1BIndex(posOppB);
                posOppB &= ~square[n];
                threatMap |= getbishopAttackMap(n, nonKingOccupancy);

            }

            while (posOppK != 0)
            {
                int n = LS1BIndex(posOppK);
                posOppK &= ~square[n];
                threatMap |= kingAttacks(n);
            }

            while (posOppQ != 0)
            {
                int n = LS1BIndex(posOppQ);
                posOppQ &= ~square[n];
                threatMap |= getbishopAttackMap(n, nonKingOccupancy) | getRookAttackMap(n, nonKingOccupancy);
            }

            while (posOppR != 0)
            {
                int n = LS1BIndex(posOppR);
                posOppR &= ~square[n];
                threatMap |= getRookAttackMap(n, nonKingOccupancy);
            }

            return threatMap;
        }

        bool CheckPseudoMoveKing(bool color, UInt64 customOccupancy, UInt64 customOppOccupancy)
        {
            UInt64 threatMap = GetThreatToKing(LS1BIndex(pieceBB[color ? 3 : 9]), color, customOccupancy | customOppOccupancy);
            if ((threatMap & (pieceBB[color ? 3 : 9])) != 0) { return false; }
            return true;
        }
        void getBishopMoves(int sq, UInt64 colorOccupancy, bool color, List<Move> moves, bool queen=false)
        {
            char piece;
            if (color) {             //UPPERCASE FASTER ??
                if (queen) { piece = 'Q'; }
                else { piece = 'B'; }
            }
            else
            {
                if (queen) { piece = 'q'; }
                else { piece = 'b'; }
            }
            UInt64 magicKey = colorOccupancy|boardOcc(!color);
            magicKey &= bishopAttacks[sq];
            magicKey *= magicsBishop[sq];
            magicKey >>= 64 - bishopOccRelevancy[sq];
            UInt64 possibleMoves = bishopAttackTable[sq, magicKey];

            
            UInt64 attackMoves = possibleMoves & boardOcc(!color);
            possibleMoves &= ~attackMoves;
            while (attackMoves != 0)
            {
                int id = LS1BIndex(attackMoves);
                if (true)
                {
                    attackMoves = attackMoves & ~square[id];
                    moves.Add(new Move(piece, sq, id, true));
                }
            }
            possibleMoves &= ~colorOccupancy; 
            while (possibleMoves != 0)
            {
                int id = LS1BIndex(possibleMoves);
                if (true)
                {
                    possibleMoves = possibleMoves & ~square[id];
                    moves.Add(new Move(piece, sq, id, false));
                }
            }
        }
        void getRookMoves(int sq, UInt64 colorOccupancy, bool color, List<Move> moves, bool queen=false)
        {
            char piece;
            if (color)
            {             //UPPERCASE FASTER ??
                if (queen) { piece = 'Q'; }
                else { piece = 'B'; }
            }
            else
            {
                if (queen) { piece = 'q'; }
                else { piece = 'b'; }
            }

            UInt64 magicKey = colorOccupancy | boardOcc(!color);
            magicKey &= rookAttacks(sq);
            magicKey *= magicsRook[sq];
            magicKey >>= 64 - rookOccRelevancy[sq];

            UInt64 possibleMoves = rookAttackTable[sq, magicKey];
            
            UInt64 attackMoves = possibleMoves & boardOcc(!color);
            possibleMoves &= ~attackMoves;
            while (attackMoves != 0)
            {
                int id = LS1BIndex(attackMoves);
                if (true) //!(checkPseudoMove(color, sq, id))
                {
                    attackMoves = attackMoves & ~square[id];
                    moves.Add(new Move(piece, sq, id, true));
                }

            }
            possibleMoves &= ~colorOccupancy;
            while (possibleMoves != 0)
            {
                int id = LS1BIndex(possibleMoves);
                if (true)
                {
                    possibleMoves = possibleMoves & ~square[id];
                    moves.Add(new Move(piece, sq, id, false));

                }

            }
        }

        void getQueenMoves(int square, UInt64 occupancy, bool color, List<Move> moves)
        {
            getBishopMoves(square, occupancy, color, moves, true); 
            getRookMoves(square, occupancy, color, moves, true);
        }

        
        void getPawnMoves(int sq, UInt64 occupancy, bool color, List<Move> moves) // can use LS1B maybe ??
        {
            int i = sq / 8;
            int j = 7-(sq % 8);

            UInt64 colorOccupancy = boardOcc(!color);
            //en peasant

            if (color)
            {
                if ((enPassant != -1) && ((sq == (enPassant - 7)) || (sq == (enPassant - 9)))) //vérifier si la case est vide
                {
                    moves.Add(new Move('P', sq, enPassant, true));
                }
                bool promotable = i == 6;

                if (i < 7 && (square[sq + 8] & occupancy) == 0)
                {
                    if (promotable) 
                    {                 
                            moves.Add(new Move('P', sq, sq + 8, false, 1));
                            moves.Add(new Move('P', sq, sq + 8, false, 4));
                    }
                    else { moves.Add(new Move('p', sq, sq + 8, false)); }
                }


                if (i == 1)
                {
                    if((square[sq + 8] & occupancy) == 0)  moves.Add(new Move('P', sq, sq + 8, false));
                    if((square[sq + 16] & occupancy) == 0)moves.Add(new Move('P', sq, sq + 16, false));
                }

                if ((j < 7) && (square[sq + 9] & colorOccupancy) != 0)
                {
                    if (promotable)
                    {
                        moves.Add(new Move('P', sq, sq + 9, true, 1));
                        moves.Add(new Move('P', sq, sq + 9, true, 4));
                    }
                    else { moves.Add(new Move('P', sq, sq + 9, true)); }
                }


                if ((j >= 0) && (square[sq + 7] & colorOccupancy) != 0)
                {
                    if (promotable)
                    {
                        moves.Add(new Move('P', sq, sq + 7, true, 1));
                        moves.Add(new Move('P', sq, sq + 7, true, 4));
                    }
                    else { moves.Add(new Move('P', sq, sq + 7, true)); }
                }
            }

            else
            {
                if ((enPassant != -1) && ((sq == (enPassant - 7)) || (sq == (enPassant - 9)))) //vérifier si la case est vide
                {
                    moves.Add(new Move('P', sq, enPassant, true));
                }
                bool promotable = i == 6;

                if (i < 7 && (square[sq + 8] & occupancy) == 0)
                {
                    if (promotable) 
                    {                 
                            moves.Add(new Move('P', sq, sq + 8, false, 1));
                            moves.Add(new Move('P', sq, sq + 8, false, 4));
                    }
                    else { moves.Add(new Move('p', sq, sq + 8, false)); }
                }


                if (i == 1)
                {
                    if((square[sq + 8] & occupancy) == 0)  moves.Add(new Move('P', sq, sq + 8, false));
                    if((square[sq + 16] & occupancy) == 0)moves.Add(new Move('P', sq, sq + 16, false));
                }

                if ((j < 7) && (square[sq + 9] & colorOccupancy) != 0)
                {
                    if (promotable)
                    {
                        moves.Add(new Move('P', sq, sq + 9, true, 1));
                        moves.Add(new Move('P', sq, sq + 9, true, 4));
                    }
                    else { moves.Add(new Move('P', sq, sq + 9, true)); }
                }


                if ((j >= 0) && (square[sq + 7] & colorOccupancy) != 0)
                {
                    if (promotable)
                    {
                        moves.Add(new Move('P', sq, sq + 7, true, 1));
                        moves.Add(new Move('P', sq, sq + 7, true, 4));
                    }
                    else { moves.Add(new Move('P', sq, sq + 7, true)); }
                }
            }
        }

        void getKnightMoves(int sq, UInt64 occupancy, bool color, List<Move> moves)
        {
           
            UInt64 opColorOccupancy = boardOcc(!color);
            //Chessboard.printBoard(opColorOccupancy);
            UInt64 possibleMoves = (knightAttacks[sq] & ~(occupancy & (~(opColorOccupancy))));
            UInt64 attackMoves = possibleMoves & opColorOccupancy;
            possibleMoves &= ~attackMoves;
            while (attackMoves != 0)
            {
                int id = LS1BIndex(attackMoves);
                attackMoves = attackMoves & ~square[id];
                moves.Add(color ? new Move('N', sq, id, true) : new Move('n', sq, id, true));
            
            }
            while (possibleMoves != 0)
            {
                int id = LS1BIndex(possibleMoves);
                possibleMoves = possibleMoves & ~square[id];
                moves.Add(color ? new Move('N', sq, id, false) : new Move('n', sq, id, false));
            }
        }

        /*
        public UInt64 getOppThreats(bool color)
        {
            UInt64 occupancy = boardOcc(color) | boardOcc(!color);
            UInt64 threatMap = 0;:

            List<Move> moves = new List<Move>();
            UInt64 posOppP = color ? pieceBB[0] : pieceBB[6];
            UInt64 posOppN = color ? pieceBB[1] : pieceBB[7];
            UInt64 posOppB = color ? pieceBB[2] : pieceBB[8];
            UInt64 posOppK = color ? pieceBB[3] : pieceBB[9];
            UInt64 posOppQ = color ? pieceBB[4] : pieceBB[10];
            UInt64 posOppR = color ? pieceBB[5] : pieceBB[11];

            posOppP &= 
            foreach (Move move in getPawnMoves())
        }
        */
        void getKingMoves(int sq, UInt64 colorOccupancy, bool color, List<Move> kingMoves)
        {           
            char piece;
            if (color){ piece = 'K'; }
            else{ piece = 'k'; }


            UInt64 attackMap = kingAttacks(sq);
            UInt64 oppOccupancy = boardOcc(!color);

            UInt64 threatMap = GetThreatToKing(sq, color, colorOccupancy | oppOccupancy);

            if ((threatMap & square[sq]) != 0) {
                //printChessboard();
                isInCheck = true;                
                //Console.WriteLine(color);
            }
            
            //printBoard(threatMap);
            UInt64 canMove = kingAttacks(sq) & (~threatMap);
            
            canMove &= ~colorOccupancy;
            //UInt64 baseRank = color ? ranks[0] : ranks[7];
            switch(color)
            {
                case true:
                    if (((castling & 1) != 0) && ((threatMap & (UInt64)6) == 0) && ((colorOccupancy & (UInt64)6) == 0))
                    {
                        kingMoves.Add(new Move(piece, sq, sq - 2, false, null, true));
                    }
                    UInt64 nb = (UInt64)Square.B1 + (UInt64)Square.C1 + (UInt64)Square.D1;
                    if (((castling & 2) != 0) && ((threatMap & nb) == 0) && ((colorOccupancy & nb) == 0))
                    {
                        kingMoves.Add(new Move(piece, sq, sq + 2, false, null, true));
                    }
                    break;
                case false:
                    nb = (UInt64)Square.F8 + (UInt64)Square.G8;
                    if (((castling & 4) != 0) && ((threatMap & nb) == 0) && ((colorOccupancy & nb) == 0))
                    {
                        kingMoves.Add(new Move(piece, sq, sq - 2, false, null, true));
                    }
                    nb = (UInt64)Square.B8 + (UInt64)Square.C8 + (UInt64)Square.D8;
                    if (((castling & 8) != 0) && ((threatMap & nb) == 0) && ((colorOccupancy & nb) == 0))
                    {
                        kingMoves.Add(new Move(piece, sq, sq + 2, false, null, true));
                    }
                    break;

            }

            UInt64 attacks = canMove & oppOccupancy;
            canMove &= ~attacks;
            while (attacks != 0)
            {
                int n = LS1BIndex(attacks);
                attacks &= ~square[n];
                kingMoves.Add(new Move(piece, sq, n, true));

            }
            
            while (canMove != 0)
            {
                int n = LS1BIndex(canMove);
                canMove &= ~square[n];
                kingMoves.Add(new Move(piece, sq, n, false));

            }
        }

        int searchTarget(int n, bool color)
        {
            int offset = color ? 6 : 0;
            for (int i = offset; i < 12; i++)
            {
                if ((pieceBB[i] & square[n]) != 0) { return i; }
            }
            return -1;
        }

        bool checkForUnsufficientMaterial()
        {
            if ((pieceBB[0] | pieceBB[6] | pieceBB[4] | pieceBB[10] | pieceBB[5] | pieceBB[11]) == 0){
                if ((bitCount(pieceBB[1] | pieceBB[2]) <= 1) && (bitCount(pieceBB[7] | pieceBB[8]) <= 1))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        bool checkForThreeFoldRepetition()
        {
            UInt64 key = generateHash();
            bool repetition = false;
            if(tfIndex >= 151) { tfIndex = 0; }
            for (int i = 0; i < tfIndex; i++)
            {
                if(key == threeFold[i]) {
                    if (repetition)
                    {
                        tfIndex = 0;
                        return true;
                    }
                    else
                    {
                        repetition = true;
                    }
                }
            }
            return false;
        }

        bool checkStalemate()
        {
            List<Move> list = getMoves(!side);
            if(list.Count == 0) {
                return true;               
            }
            else
            {
                moveCache = list;
                return false;
            }
        }

        public List<Move> getMoves(bool color)
        {


            List<Move> list = new List<Move>();
            if (moveCache.Count > 0)
            {
                foreach (Move move in moveCache) {
                    list.Add(move);
                }
                moveCache.Clear();        
                return list;
            }

            UInt64 colorOccupancy = boardOcc(color);
            UInt64 occupancy = colorOccupancy | boardOcc(!color);
            //printBoard(pieceBB[3]);
            getKingMoves(LS1BIndex(color ? pieceBB[3] : pieceBB[9]), boardOcc(color), color, list);


            UInt64 posP = color ? pieceBB[0] : pieceBB[6];
            UInt64 posN = color ? pieceBB[1] : pieceBB[7];
            UInt64 posB = color ? pieceBB[2] : pieceBB[8];
            //UInt64 posK = color ? pieceBB[3] : pieceBB[9];
            UInt64 posQ = color ? pieceBB[4] : pieceBB[10];
            UInt64 posR = color ? pieceBB[5] : pieceBB[11];

 
            while (posP != 0)
            {
                int n = LS1BIndex(posP);
                posP &= ~square[n];
                getPawnMoves(n, occupancy, color, list);
            }
            //printBoard(threatMap);
            while (posN != 0)
            {
                int n = LS1BIndex(posN);
                posN &= ~square[n];
                getKnightMoves(n, occupancy, color, list);
            }
            //printBoard(threatMap);
            while (posB != 0)
            {
                int n = LS1BIndex(posB);
                posB &= ~square[n];
                getBishopMoves(n, colorOccupancy, color, list);
            }
            //printBoard(threatMap);
            /*
            while (posK != 0)
            {
                int n = LS1BIndex(posK);
                posK &= ~square[n];
                coves(n, colorOccupancy, color, list,);
            }
            */
            //printBoard(threatMap);
            while (posQ != 0)
            {
                int n = LS1BIndex(posQ);
                posQ &= ~square[n];
                getQueenMoves(n, colorOccupancy, color, list);
            }
            
            while (posR != 0)
            {
                int n = LS1BIndex(posR);
                posR &= ~square[n];
                getRookMoves(n, colorOccupancy, color, list);
                
            }

            List<Move> list2 = new List<Move>();
            foreach(Move move in list)
            {
                if (move.Attack)
                {
                    if (CheckPseudoMoveKing(color, (square[move.To] | colorOccupancy) & (~square[move.Square]), boardOcc(!color)))
                    {
                        list2.Add(move);
                    }
                }
                else
                {
                    if (CheckPseudoMoveKing(color, (square[move.To] | colorOccupancy) & (~square[move.Square]), boardOcc(!color) & (~square[move.To])))
                    {
                        list2.Add(move);
                    }
                }
            }
            if (list2.Count == 0)
            {
                gameHasEnded = true;
                if (isInCheck)
                {
                     //???
                    winner = !color;
                } 
            }
            isInCheck = false;
            return list2;
        }
        public void makeMove(Move move, bool color)
        {
            
            bool enpassed = false;

            if (move.Attack)
            {
                int t = searchTarget(move.To, color); // TODO : put in check bool as true if t==3 or 9

                if ((t == 3) || (t == 9))
                {                   
                    printBoard(pieceBB[((int)fromCharPieces[move.PieceType])]);
                    Console.WriteLine("nb : " + t);
                    Console.WriteLine(color);
                    printChessboard();
                    //gameHasEnded = true;
                    //winner = color;
                }

                if (t != -1) { pieceBB[t] &= ~square[move.To]; } //??
                halfMoves = 0; //50 moves rule
                tfIndex = 0;

            }

            if ((move.PieceType == 'P') || (move.PieceType == 'p'))          //TODO: handle en passant
            {
                pieceBB[(int)fromCharPieces[move.PieceType]] &= ~square[move.Square];
                pieceBB[(int)fromCharPieces[move.PieceType]] |= square[move.To];
                if (move.Promotion != null)
                {
                    pieceBB[(int)move.Promotion] |= square[move.To];
                    pieceBB[(int)fromCharPieces[move.PieceType]] &= ~square[move.To];

                }
                if(((move.To-move.Square)/8 == 2) && ((boardOcc(true)|boardOcc(false)) & (square[move.To-8])) == 0)
                {
                    enPassant = color ? move.To-8 : move.To+8;
                    enpassed = true;
                }
                halfMoves = 0; //50 moves rule
                tfIndex = 0;
            }

            else if ((move.PieceType == 'K') || (move.PieceType == 'k'))
            {
                pieceBB[(int)fromCharPieces[move.PieceType]] &= ~square[move.Square];
                pieceBB[(int)fromCharPieces[move.PieceType]] |= square[move.To];
                if (move.CastlingMove)
                {
                    switch (move.To - move.Square)
                    {
                        case 2:
                            UInt64 rookPos = color ? (UInt64)Square.A1 : (UInt64)Square.A8;
                            pieceBB[color ? 5 : 11] &= ~rookPos;
                            pieceBB[color ? 5 : 11] |= square[move.To - 1];
                            break;
                        case -2:
                            rookPos = color ? (UInt64)Square.H1 : (UInt64)Square.H8;
                            pieceBB[color ? 5 : 11] &= ~rookPos;
                            pieceBB[color ? 5 : 11] |= square[move.To + 1];
                            break;
                    }
                }

                if (color) { castling &= ~((Int16)3); }
                else { castling &= ~((Int16)12); }
                //remove castling ability
                halfMoves++;
            }

            else if ((move.PieceType == 'R') || (move.PieceType == 'r'))          //TODO: handle en passant
            {
                pieceBB[(int)fromCharPieces[move.PieceType]] &= ~square[move.Square];
                pieceBB[(int)fromCharPieces[move.PieceType]] |= square[move.To];
                halfMoves++;
                if (color) {
                    switch (move.Square)
                    {
                        case 7:
                            castling &= ~((Int16)2);
                            break;
                        case 0:
                            castling &= ~((Int16)1);
                            break;
                    } 
                }
                else {
                    switch (move.Square)
                    {
                        case 63:
                            castling &= ~((Int16)8);
                            break;
                        case 56:
                            castling &= ~((Int16)4);
                            break;
                    }
                }
                //remove castling ability
            }

            else
            {
                pieceBB[(int)fromCharPieces[move.PieceType]] &= ~square[move.Square];
                pieceBB[(int)fromCharPieces[move.PieceType]] |= square[move.To];
                halfMoves++;
            }


            if (!enpassed)
            {
                enPassant = -1;
            }

            if((halfMoves>=100) || (checkForUnsufficientMaterial()) || (checkForThreeFoldRepetition()) || (checkStalemate()))
            {
                gameHasEnded = true;
            }
            side = !side;
        }
    }
}
