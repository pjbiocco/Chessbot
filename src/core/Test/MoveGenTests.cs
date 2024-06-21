using System.Diagnostics.CodeAnalysis;
using Chess.Core;

public class MoveGenTest{
    public String testFEN = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
    public String defaultFen = FenParser.defaultStartFEN;

    public ulong plyTest(int depth, BoardState board){

        ulong total = 0;

        MoveList moveList = new MoveList();
        moveList = MoveGen.genBoardMoves(board, moveList);
        if(depth == 0) return 1;
        if(depth == 1) return (ulong) moveList.length;

        for(int i = 0; i < moveList.length; i++){

            BoardState clone = new BoardState(board);
            clone.applyMove(moveList.moves[i]);

            total += plyTest(depth - 1, clone);
        }
         
        return total; 
    }

    public void plyTestExpanded(int depth, BoardState board){

        ulong total = 0;
        
        MoveList moveList = new MoveList();
        moveList = MoveGen.genBoardMoves(board, moveList);

        String[] plys = new String[moveList.length];

        for(int i = 0; i < moveList.length; i++){
            BoardState clone = new BoardState(board);
            clone.applyMove(moveList.moves[i]);

            plys[i] = moveList.moves[i].toString() + " " + plyTest(depth - 1, clone);     
            total+= plyTest(depth - 1, clone);
            
        }
        
        Array.Sort(plys);

        foreach(String s in plys){
            Console.WriteLine(s);
        }
        Console.WriteLine("Total:  " + total);
    }


}