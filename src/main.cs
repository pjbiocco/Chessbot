using System;
using System.Runtime.Intrinsics.X86;
using Chess.Core;
using static Chess.Core.Square;

class Start{

    public static void Main()
    {

        MoveList x = new MoveList();

        //BoardState board = FenParser.makeBoard("rnbqkbnr/1ppppp2/6p1/8/8/8/pPPPPPPp/RNBQKBN1 b Qkq - 0 1");
        

        BoardState board = FenParser.makeBoard("r3kb1r/1p2pppp/1pn2n2/3p4/2pP1Bb1/P1P1PN2/1P1N1PPP/R3KB1R b KQkq - 0 9");
        //BoardState board = FenParser.makeDefaultBoard();
        board.printBoard();

        x = MoveGen.genKingMoves(e8, board, x);

        Console.WriteLine();
        Console.WriteLine("POST MOVE:  ");

        board.applyMove(x.moves[0]);
        board.printBoard();

        Console.WriteLine("ROOK: ");
        board.pieces[(int)PieceType.ROOK].printBitBoard();
        Console.WriteLine("WHITE: ");
        board.occupancy[(int)Color.WHITE].printBitBoard();
        Console.WriteLine("King: ");
        board.pieces[(int)PieceType.KING].printBitBoard();

        //board.pieces[(int)PieceType.KNIGHT].printBitBoard();

    }
}