using System;
using Chess.Core;
using static Chess.Core.Position;

class Start{

    public static void Main()
    {
        
        BoardState x = FenParser.makeDefaultBoard();
        x.printBoard();
        //x.pawn.printBitBoard();
        Bitboard blackRook = new Bitboard(x.bitboards[BitboardType.ROOK] & x.bitboards[BitboardType.BLACK]);

        blackRook.printBitBoard();
    }

}