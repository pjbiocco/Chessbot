using System;
using Chess.Core;

class HelloWord{

    public static void Main()
    {
        BoardState x = FenParser.makeDefaultBoard();
        x.printBoard();

        BoardState y = FenParser.makeBoard("8/5k2/3p4/1p1Pp2p/pP2Pp1P/P4P1K/8/8 b - - 99 50");
        y.printBoard();
        
    }

}