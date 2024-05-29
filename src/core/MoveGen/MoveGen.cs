namespace Chess.Core{

    public static class MoveGen{

        static Bitboard[] kingBoards = AttackGenerator.generateKingMasks();
        static Bitboard[] knightBoards = AttackGenerator.generateKnightMasks();

        public static List<Move> genKingMoves(int start, BoardState currentBoard){

            List<Move> moves = new List<Move>();
            Bitboard currentAttacks = new Bitboard(kingBoards[start]);
            currentAttacks &= ~(currentBoard.bitboards[currentBoard.currentTurn]);

            ulong currBit = currentAttacks.popLeastSignificantBit();
            while(currBit != 64){
                moves.Add(new Move(start, (int)currBit));
                currBit = currentAttacks.popLeastSignificantBit();
            }
            return moves;
        }

        public static List<Move> genKnightMoves(int start, BoardState currentBoard){

            List<Move> moves = new List<Move>();
            Bitboard currentAttacks = new Bitboard(knightBoards[start]);
            currentAttacks &= ~(currentBoard.bitboards[currentBoard.currentTurn]);

            ulong currBit = currentAttacks.popLeastSignificantBit();
            while(currBit != 64){
                moves.Add(new Move(start, (int)currBit));
                currBit = currentAttacks.popLeastSignificantBit();
            }
            return moves;
        }

    }
    
}