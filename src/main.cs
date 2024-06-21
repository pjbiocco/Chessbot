
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        //MoveList x = new MoveList();

        //BoardState board = FenParser.makeBoard("r1bq1rk1/pppp1ppp/2n2n2/4p3/1bB1P3/2NP1N2/PPP2PPP/R1BQK2R w KQ - 3 6");
        
        BoardState kiwi = FenParser.makeBoard("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");
        BoardState board = FenParser.makeBoard("1r2k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/P1N2Q1p/1PPBBPPP/1R2K2R b Kk - 0 2");
        //BoardState board = FenParser.makeBoard("rnbqkbnr/p1pppppp/8/1p6/P7/8/1PPPPPPP/RNBQKBNR w KQkq b6 0 2");
        //BoardState board = FenParser.makeDefaultBoard();

        MoveGenTest moveTester = new MoveGenTest();
        moveTester.plyTestExpanded(5, kiwi);
        //Console.WriteLine(x);
        // board.printBoard();

        // Console.WriteLine(
        //     MoveGen.isCastleBlocked(board, new Move(e8, c8, MoveFlag.QUEEN_CASTLE))
        // );

        // Bitboard[] x = BlockerBoard.bishopTrimMask;
       
        // for(int i = 0; i < 64; i++){
        //     Console.WriteLine(i);
        //     x[i].printBitBoard();

        // }
       
        // MoveList list = new MoveList();

        // list = MoveGen.genBoardMoves(board, list);

        // for(int i = 0; i < list.length; i++){
        //    list.moves[i].printMove();
        // } 
        // Console.WriteLine(list.length);





        //ulong[] magics = MagicGen.genBishopMagics();

        // Console.WriteLine("{");
        // for(int i = 0; i < magics.Length; i++){
        //     Console.Write(magics[i] + ", ");
        // }
        // Console.WriteLine("}");

    }
}