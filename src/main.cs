
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        //MoveList x = new MoveList();

        BoardState board = FenParser.makeBoard("r1bqkbnr/pppp1Qpp/2n5/4p3/4P3/8/PPPP1PPP/RNB1KBNR b KQkq - 0 3");
        

        //BoardState board = FenParser.makeBoard("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");
        //BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        Console.WriteLine(
            MoveGen.isCastleBlocked(board, new Move(e8, c8, MoveFlag.QUEEN_CASTLE))
        );

        // Bitboard[] x = BlockerBoard.bishopTrimMask;
       
        // for(int i = 0; i < 64; i++){
        //     Console.WriteLine(i);
        //     x[i].printBitBoard();

        // }
       
        MoveList list = new MoveList();

        list = MoveGen.genBoardMoves(board, list);

        for(int i = 0; i < list.length; i++){
           list.moves[i].printMove();
        } 
        Console.WriteLine(list.length);





        //ulong[] magics = MagicGen.genBishopMagics();

        // Console.WriteLine("{");
        // for(int i = 0; i < magics.Length; i++){
        //     Console.Write(magics[i] + ", ");
        // }
        // Console.WriteLine("}");

    }
}