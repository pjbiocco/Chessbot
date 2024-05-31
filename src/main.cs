using System;
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        MoveList x = new MoveList();

        BoardState board = FenParser.makeBoard("r2qk2r/pbppbppp/1pn2n2/4p3/4P3/1PN2NP1/PBPPQPBP/R3K2R b KQkq - 0 1");

        //BoardState board = FenParser.makeBoard("rnbqkb1r/1p1p1p1p/p1p1pnp1/8/8/1PNP1P1P/P1P1P1P1/R1BQKBNR b KQkq - 0 1");
        //BoardState board = FenParser.makeDefaultBoard();
    
        Bitboard[][] bitboards = AttackGenerator.generatePawnAttackMasks();
        board.getCurrTurnPieceBoard(PieceType.PAWN).printBitBoard();
        //Bitboard b = board.getWhitePawns();

        //b.shiftBoard(Direction.LEFTLEFTDOWN).printBitBoard();

        x = MoveGen.genKingMoves(e8, board, x);

        for(int i = 0; i < x.index; i++){
            x.moves[i].printMove();
        }
    }
}