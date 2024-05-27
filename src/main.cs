using System;
using Chess.Core;
//using Position;

class Start{

    public static void Main()
    {
        BoardState x = FenParser.makeDefaultBoard();
        x.printBoard();

         Move q = new Move(Position.a1, Position.a8, 0b0100);


        //BoardState y = FenParser.makeBoard("8/8/1k6/2b5/2pP4/8/5K2/8 b - d3 0 1");
        //y.printBoard();
        
    }

}