using System;
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {
         BoardState board = FenParser.makeBoard("2r5/3pk3/8/2P5/8/2K5/8/8 w - - 5 4");
        //BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        List<Move> moves = MoveGen.genPawnAttacks((int) g4, board);

        foreach(Move m in moves){
            m.printMove();
        }
        

       //Bitboard[][] x = AttackGenerator.generatePawnAttackMasks();

       //for(int i = 0; i < x[0].Length; i++){
       //     Console.WriteLine("Board No: " + i);
       //     x[0][i].printBitBoard();
       //     Console.WriteLine();
       //}
    }
}