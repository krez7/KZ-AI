using System;
using Bitboard;

namespace Krezb0t
{
    internal class MCTS(Move? _move=null, bool _color=true)
    {
        List<Move>? lazyMoves;
        Move? move = _move;
        List<MCTS> children = new List<MCTS>();
        int wins = 0;
        int losses = 0;
        int visited = 0;
        bool color = _color;
       
        /*
        PieceColor Color
        {
            get
            {
                if (color) return PieceColor.White;
                else return PieceColor.Black;
            }  
        }
        */
        /*
        static int abs(int a1, int a2) { 
        
            if(a1>a2) return a1-a2;
            else return a2-a1;
        }
        */
        void printStats()
        {
            Console.WriteLine($"{visited} {wins} {losses}");

        }

        double UCT(MCTS child, double c_param = 0.1)
        {
            if (child.visited == 0)
            {
                return Double.MaxValue;
            }

            else 
            {            
                return (((double)(child.wins) / (double)child.visited) + c_param * (Math.Sqrt((2 * Math.Log(visited)) / child.visited)));
            }
        }
        MCTS selection(MagicBitBoard board)
        {
            if (lazyMoves.Count > 0)
            {
                board.makeMove(lazyMoves[0], color);
                MCTS nextMove = new MCTS(lazyMoves[0], !color);
                lazyMoves.RemoveAt(0);
                children.Add(nextMove);
                return nextMove;
            }

            else
            {
                MCTS nextMove = null;
                double maxVal = Double.MinValue;
                foreach (MCTS cMove in children)
                {
                    double currVal = UCT(cMove);
                    if (currVal == Double.MaxValue)
                    {
                        nextMove = cMove;
                        break;
                    }
                    if (currVal > maxVal)
                    {
                        maxVal = currVal;
                        nextMove = cMove;
                    }

                }
                board.makeMove(nextMove.move, color);
                return nextMove;
            }
        }

        MCTS expand(MagicBitBoard board)
        {
            lazyMoves = board.getMoves(board.side);
            Random rand = new Random();
            //hessBoard.printBoard(board.boardOcc(true)|board.boardOcc(false));
            MCTS nextMove = new MCTS(lazyMoves[rand.Next(0, lazyMoves.Count-1)], !color);
            board.makeMove(nextMove.move, color);           
            //Console.WriteLine(board.ToAscii());
            children.Add(nextMove);
            return nextMove;
        }

        bool? expansion(MagicBitBoard board)
        {
            visited++;
            if (!board.gameHasEnded)
            {
                bool? res = expand(board).expansion(board);
                if (res != null)
                {
                    if (res != color) { losses++; } else { wins++; }
                }
                return res;
            }
            
            if (board.winner != null)
            {
                losses++;                
            }
            return board.winner;


        }
        internal bool? process(MagicBitBoard board)
        {
            visited++;
            if(children.Any()) // .Any() ??
            {
                bool? res = selection(board).process(board);
                if (res != null)
                {
                    if (res != color) { losses++; } else { wins++; }
                }
                return res;
            }
            else
            {
                visited--;
                return expansion(board);
            }
        }
        internal Move bestMove()
        {
            MCTS bMCTS = children[0];
            int bVi = int.MinValue;
            foreach (MCTS cMCTS in children)
            {
                cMCTS.printStats();
                if (cMCTS.visited > bVi)
                {
                    bMCTS = cMCTS;
                    bVi = cMCTS.visited;
                }

            }
            return bMCTS.move;

        }
    }
}
