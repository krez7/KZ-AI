using System;
using System.Diagnostics;
using System.Reflection;

namespace Bitboard
{
    public class Chessboard
    {
        public const UInt64 diag = 72624976668147840; //0x8142241818244281
        public const UInt64 antiDiag = 9241421688590303745;
        
        public const bool white = true;
        public const bool black = false;

        public enum Square : UInt64
        {
            A1 = B1 << 1, B1 = C1 << 1, C1 = D1 << 1, D1 = E1 << 1, E1 = F1 << 1, F1 = G1 << 1, G1 = H1 << 1, H1 = 1,
            A2 = B2 << 1, B2 = C2 << 1, C2 = D2 << 1, D2 = E2 << 1, E2 = F2 << 1, F2 = G2 << 1, G2 = H2 << 1, H2 = A1 << 1,
            A3 = B3 << 1, B3 = C3 << 1, C3 = D3 << 1, D3 = E3 << 1, E3 = F3 << 1, F3 = G3 << 1, G3 = H3 << 1, H3 = A2 << 1,
            A4 = B4 << 1, B4 = C4 << 1, C4 = D4 << 1, D4 = E4 << 1, E4 = F4 << 1, F4 = G4 << 1, G4 = H4 << 1, H4 = A3 << 1,
            A5 = B5 << 1, B5 = C5 << 1, C5 = D5 << 1, D5 = E5 << 1, E5 = F5 << 1, F5 = G5 << 1, G5 = H5 << 1, H5 = A4 << 1,
            A6 = B6 << 1, B6 = C6 << 1, C6 = D6 << 1, D6 = E6 << 1, E6 = F6 << 1, F6 = G6 << 1, G6 = H6 << 1, H6 = A5 << 1,
            A7 = B7 << 1, B7 = C7 << 1, C7 = D7 << 1, D7 = E7 << 1, E7 = F7 << 1, F7 = G7 << 1, G7 = H7 << 1, H7 = A6 << 1,
            A8 = B8 << 1, B8 = C8 << 1, C8 = D8 << 1, D8 = E8 << 1, E8 = F8 << 1, F8 = G8 << 1, G8 = H8 << 1, H8 = A7 << 1,
        }

        /*
        public UInt64 P = (UInt64)Square.A2 | (UInt64)Square.B2 | (UInt64)Square.C2 | (UInt64)Square.D2 | (UInt64)Square.E2 | (UInt64)Square.F2 | (UInt64)Square.G2 | (UInt64)Square.H2;
        public UInt64 N = (UInt64)Square.B1 | (UInt64)Square.G1;
        public UInt64 B = (UInt64)Square.C1 | (UInt64)Square.F1;
        public UInt64 K = (UInt64)Square.D1;
        public UInt64 Q = (UInt64)Square.E1;
        public UInt64 R = (UInt64)Square.A1 | (UInt64)Square.H1;

        public UInt64 p = (UInt64)Square.A7 | (UInt64)Square.B7 | (UInt64)Square.C7 | (UInt64)Square.D7 | (UInt64)Square.E7 | (UInt64)Square.F7 | (UInt64)Square.G7 | (UInt64)Square.H7;
        public UInt64 n = (UInt64)Square.B8 | (UInt64)Square.G8;
        public UInt64 b = (UInt64)Square.C8 | (UInt64)Square.F8;
        public UInt64 k = (UInt64)Square.D8;
        public UInt64 q = (UInt64)Square.E8;
        public UInt64 r = (UInt64)Square.A8 | (UInt64)Square.H8;
        */

        public UInt64[] pieceBB = new UInt64[12];
        public enum Material
        {
            P, N, B, K, Q, R, p, n, b, k, q, r
        }

        public static readonly Dictionary<char, Material> fromCharPieces = new Dictionary<char, Material>
        {
            ['P'] = Material.P,
            ['N'] = Material.N,
            ['B'] = Material.B,
            ['K'] = Material.K,
            ['Q'] = Material.Q,
            ['R'] = Material.R,
            ['p'] = Material.p,
            ['n'] = Material.n,
            ['b'] = Material.b,
            ['k'] = Material.k,
            ['q'] = Material.q,
            ['r'] = Material.r
        };

        public static readonly Dictionary<Material, Char> reverseFromCharPieces = new Dictionary<Material, Char>
        {
            [Material.P] = 'P',
            [Material.N] = 'N',
            [Material.B] = 'B',
            [Material.K] = 'K',
            [Material.Q] = 'Q',
            [Material.R] = 'R',
            [Material.p] = 'p',
            [Material.n] = 'n',
            [Material.b] = 'b',
            [Material.k] = 'k',
            [Material.q] = 'q',
            [Material.r] = 'r'
        };

        public static readonly Dictionary<string, UInt64> squareDict;

        public static readonly UInt64[] files = [0x8080808080808080, 0x4040404040404040, 0x2020202020202020, 0x1010101010101010, 0x808080808080808, 0x404040404040404, 0x0202020202020202, 0x0101010101010101];
        public static readonly UInt64[] ranks = [0x00000000000000ff, 0x000000000000ff00, 0x0000000000ff0000, 0x00000000ff000000, 0x000000ff00000000, 0x0000ff0000000000, 0x00ff000000000000, 0xff00000000000000];

        public bool side = white;
        public Int16 castling;

        public int halfMoves;
        public int enPassant; 


        public static UInt64[] square;

        public bool isInCheck = false;

        public bool gameHasEnded = false;
        public bool? winner = null;

        public List<Move> moveCache;

        ///////////// TFR (Zobrist keys)////////////
        ///        
        public UInt64[] threeFold;
        public int tfIndex;
        UInt64[,] pieceKeys;
        UInt64[] enPassantKeys;
        UInt64[] castleKeys;
        UInt64 sideKey;
        /// /////////

        static Chessboard()
        {
            squareDict = new Dictionary<string, UInt64>(64);
            foreach (Square sq in Enum.GetValues(typeof(Square)))
            {
                squareDict.Add(Enum.GetName(sq), (UInt64)sq);
            }

            square = new UInt64[64];
            UInt64 id = 1;
            for (int i = 0; i < 64; i++)
            {
                square[i] = id;
                id = (id << 1);
            }

        }
        public Chessboard()
        {
            threeFold = new UInt64[100];
            pieceKeys = new UInt64[12,64];
            enPassantKeys = new UInt64[64];
            castleKeys = new UInt64[16];
            moveCache = new List<Move>();

            pieceBB[0] = 65280;
            pieceBB[1] = 66;
            pieceBB[2] = 36;
            pieceBB[3] = 8;
            pieceBB[4] = 16;
            pieceBB[5] = 129;
            pieceBB[6] = 71776119061217280;
            pieceBB[7] = 4755801206503243776;
            pieceBB[8] = 2594073385365405696;
            pieceBB[9] = 576460752303423488;
            pieceBB[10] = 1152921504606846976;
            pieceBB[11] = 9295429630892703744;

            halfMoves = 0;
            enPassant = -1;

            tfIndex = 0;
            castling = 15;

        }

        /*
        public Chessboard(UInt64[] _pieceBB, bool _side, int _enPassant, int _halfMoves){
            pieceBB = _pieceBB;
            side = _side;
            enPassant = _enPassant;
            halfMoves = _halfMoves;
        }
        */

        public Chessboard(string FEN)
        {
            threeFold = new UInt64[100];
            pieceKeys = new UInt64[12,64];
            enPassantKeys = new UInt64[64];
            castleKeys = new UInt64[16];
            moveCache = new List<Move>();
            
            tfIndex = 0;
            castling = 0;

            string hm = "";
            string ep = "";
 
            int i = 0;
            int pos = 63;

            for (i = 0; FEN[i] != ' '; i++)
            {
                if (((FEN[i] >= 'a') && (FEN[i] <= 'z')) || ((FEN[i] >= 'A') && (FEN[i] <= 'Z')))
                {
                    pieceBB[(int)Chessboard.fromCharPieces[FEN[i]]] |= Chessboard.square[pos];
                    pos--;
                }

                else if ((FEN[i] > '0') && (FEN[i] <= '9'))
                {
                    pos -= FEN[i] - '0';                       
                }
            }

            if(FEN[i+1] == 'b') side = black;
            i+=3;
            
            for(; i < FEN.Length - 2; i++){
                switch (FEN[i])
                {
                    case 'k':
                        castling |= 1;
                        break;
                    case 'q':
                        castling |= 2;
                        break;
                    case 'K':
                        castling |= 4;
                        break;
                    case 'Q':
                        castling |= 8;
                        break;
                    case char c when (c >= 'a') && (c <= 'h'):
                        ep += c;
                        ep += FEN[++i];
                        i++;
                        break;
                    case '0':
                        hm = "0";
                        break;
                    case char c when (c >= '0') && (c <= '9'):
                        hm += c;
                        if(FEN[++i] != ' ') {
                            hm += FEN[i];
                            if(FEN[++i] != ' ') hm = "100";
                        }
                        break;
                }
            }
            enPassant = ep != "" ? squareCoordUint64(Chessboard.squareDict[ep.ToUpper()]) : -1;
            halfMoves = Int32.Parse(hm);
        }

        public UInt64 bitboard()
        {
            UInt64 res = 0;
            for(int i = 0; i < 12; i++)
            {
                res |= pieceBB[i];
            }
            return res;
        }
        public static void printUInt64(UInt64 n)
        {
            string f = Convert.ToString((long)n, 2);
            string f2 = f.PadLeft(64, '0');
            for (int i = 0; i < f2.Length / 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(f2[i * 8 + j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            int test = (f2.Length / 8) * 8;
            for (int i = 0; i < (f2.Length - test); i++)
            {
                Console.Write(f2[test + i]);
            }
        }

        public static void printBoard(UInt64 n)
        {
            string[] letters = ["A", "B", "C", "D", "E", "F", "G", "H"];
            string f = Convert.ToString((long)n, 2);
            string f2 = f.PadLeft(64, '0');
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(letters[i] + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < f2.Length / 8; i++)
            {
                Console.Write(8 - i);
                Console.Write(" ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(f2[i * 8 + j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            int test = (f2.Length / 8) * 8;
            for (int i = 0; i < (f2.Length - test); i++)
            {
                Console.Write(f2[test + i]);
            }
        }

        public void printChessboard()
        {
            string[] letters = ["A", "B", "C", "D", "E", "F", "G", "H"];
            Console.Write("  ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(letters[i] + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < 8; i++)
            {
                Console.Write(8 - i);
                Console.Write(" ");
                for (int j = 0; j < 8; j++)
                {
                    char piece = '*';
                    foreach (Material k in Enum.GetValues(typeof(Material))) 
                    {
                        if ((pieceBB[(int)k] & (square[63 - (i*8 + j)])) != 0)
                        {
                            piece = reverseFromCharPieces[k];
                            break;  
                        }
                    }
                    Console.Write(piece);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            string toMove = side ? "White" : "Black";
            Console.WriteLine("\nTo Move :" + ' ' + toMove);
        }

        /*
        public UInt64 boardCol(bool c)
        {
            if (c) { return (K | Q | N | B | R | P); }
            else { return (k | q | n | b | r | r); }
        }
        */

        public UInt64 boardOcc(bool c)
        {
            UInt64 tot = 0;
            if (c)
            {
                for(int i = 0; i < 6; i++)
                {
                    tot |= pieceBB[i];
                }
            }
            else
            {
                for (int i = 6; i < 12; i++)
                {
                    tot |= pieceBB[i];
                }
            }
            return tot;
        }
        public static UInt64 negativeUint64(UInt64 n)
        {
            return unchecked(unchecked((ulong)-((long)n)));
        }
        public static UInt64 LS1B(UInt64 n)
        {
            return (n & negativeUint64(n));
        }
        public static int bitCount(UInt64 n)
        {
            int i = 0;
            while (n != 0)
            {
                n &= n - 1; // ou n - LS1B(n)
                i++;
            }
            return i;

        }
        public static int squareCoordUint64(UInt64 n)
        {
            int i = 0;
            for(i = 0; n != 0; i++)
            {
                n = (n >> 1);
            }
            return (i - 1);
        }

        public static int LS1BIndex(UInt64 n)
        {
            return bitCount(LS1B(n) - 1);
        }

        public static UInt64 generateMagic()
        {
            return (RandXOR.getRandUInt64() & RandXOR.getRandUInt64() & RandXOR.getRandUInt64());
        }

        public void initRandKey()
        {
            for(int i = 0; i < 12; i++)
            {
                for (int sq = 0; sq < 64; sq++)
                {
                    pieceKeys[i, sq] = RandXOR.getRandUInt64();
                }
            }
            for (int sq = 0; sq < 64; sq++)
            {
                enPassantKeys[sq] = RandXOR.getRandUInt64();
            }
            for (int i = 0; i < 16; i++)
            {
                castleKeys[i] = RandXOR.getRandUInt64();
            }

            sideKey = RandXOR.getRandUInt64();
        }

        public UInt64 generateHash()
        {
            UInt64 key = 0;
            UInt64 bbCopy;
            for (int i = 0; i < 12; i++)
            {
                bbCopy = pieceBB[i];

                while (bbCopy != 0)
                {
                    int sq = LS1BIndex(bbCopy);
                    key ^= pieceKeys[i, sq];
                    bbCopy &= ~square[sq];
                }
            }

            if(enPassant != -1)
            {
                key ^= enPassantKeys[enPassant];
            }
            if(castling <= 0)
            {
                castling = 0;
            }
            key ^= castleKeys[castling];
            if (!side) { key ^= sideKey; }
            return key;
        }

    }
}
