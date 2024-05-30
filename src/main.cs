using System;
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        MoveList x = new MoveList();

        BoardState board = FenParser.makeBoard("rnbqkb1r/1p1p1p1p/p1p1pnp1/8/8/1PNP1P1P/P1P1P1P1/R1BQKBNR w KQkq - 0 1");
        //BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        x = MoveGen.genAllPawnPushes(board, x);

        for(int i = 0; i < x.index; i++){
            x.moves[i].printMove();
        }
         

    }
}