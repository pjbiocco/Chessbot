using System;
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {
        //BoardState board = FenParser.makeBoard("2r5/3pk3/8/2P5/8/2K5/8/8 b - - 5 4");
        BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        List<Move> moves = MoveGen.genKnightMoves(b1, board);

        foreach(Move m in moves){
            m.printMove();
        }
        
    }
}