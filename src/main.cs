
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        MoveList x = new MoveList();

        //BoardState board = FenParser.makeBoard("rnbqkbnr/1ppppp2/6p1/8/8/8/pPPPPPPp/RNBQKBN1 b Qkq - 0 1");
        

        BoardState board = FenParser.makeBoard("rnbqk2r/pp3p1p/2p2np1/b2p2N1/P2Pp3/1PP3P1/4PPBP/RNBQ1RK1 w kq - 0 9");
        //BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        MoveList list = new MoveList();

        list = MoveGen.genQueenMoves(d1, board, list);

        for(int i = 0; i < list.length; i++){
           list.moves[i].printMove();
        } 
        //ulong[] magics = MagicGen.genBishopMagics();

        // Console.WriteLine("{");
        // for(int i = 0; i < magics.Length; i++){
        //     Console.Write(magics[i] + ", ");
        // }
        // Console.WriteLine("}");

    }
}