namespace Tests;
using Bitboard;
using Square = Bitboard.Chessboard.Square;

public class BitboardTest
{
	[Fact]
	public void FENConstructorTest(){
		Chessboard testBoard = new ("r1bqkbnr/pp2pp1p/2n3p1/1BppP3/8/5N2/PPPP1PPP/RNBQK2R w KQkq d6 0 5"); // edited from https://lichess.org/study/7WCFYt0R/ePy2DmAC#5
		
		UInt64 wPawnPos = (UInt64)(Square.A2 | Square.B2 | Square.C2 | Square.D2 | Square.E5 | Square.F2 | Square.G2 | Square.H2);
		UInt64 bPawnPos = (UInt64)(Square.A7 | Square.B7 | Square.C5 | Square.D5 | Square.E7 | Square.F7 | Square.G6 | Square.H7);
		
		UInt64 wKnightPos = (UInt64)(Square.B1 | Square.F3);
		UInt64 bKnightPos = (UInt64)(Square.C6 | Square.G8);
		
		UInt64 wBishopPos = (UInt64)(Square.C1 | Square.B5);
		UInt64 bBishopPos = (UInt64)(Square.C8 | Square.F8);
		UInt64 wRookPos = (UInt64)(Square.A1 | Square.H1);
		UInt64 bRookPos = (UInt64)(Square.A8 | Square.H8);

		Assert.Multiple(
        		() => Assert.Equal(wPawnPos, testBoard.pieceBB[(int)Chessboard.Material.P]), 

        		() => Assert.Equal(bPawnPos, testBoard.pieceBB[(int)Chessboard.Material.p]), 
        		() => Assert.Equal(wKnightPos, testBoard.pieceBB[(int)Chessboard.Material.N]), 
        		() => Assert.Equal(bKnightPos, testBoard.pieceBB[(int)Chessboard.Material.n]), 
        		() => Assert.Equal(wBishopPos, testBoard.pieceBB[(int)Chessboard.Material.B]), 
        		() => Assert.Equal(bBishopPos, testBoard.pieceBB[(int)Chessboard.Material.b]), 
        		() => Assert.Equal(wRookPos, testBoard.pieceBB[(int)Chessboard.Material.R] ), 
        		() => Assert.Equal(bRookPos, testBoard.pieceBB[(int)Chessboard.Material.r]), 
        		() => Assert.Equal((UInt64)Square.D1, testBoard.pieceBB[(int)Chessboard.Material.Q]), 
        		() => Assert.Equal((UInt64)Square.D8, testBoard.pieceBB[(int)Chessboard.Material.q]), 
        		() => Assert.Equal((UInt64)Square.E1, testBoard.pieceBB[(int)Chessboard.Material.K]), 
        		() => Assert.Equal((UInt64)Square.E8, testBoard.pieceBB[(int)Chessboard.Material.k]),
			() => Assert.Equal((UInt64)Square.E8, testBoard.pieceBB[(int)Chessboard.Material.k]),
			() => Assert.Equal(15, testBoard.castling),
			() => Assert.Equal(36, testBoard.enPassant),
			() => Assert.Equal(0, testBoard.halfMoves)
    		);
	}
	
}
