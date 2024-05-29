namespace Chess.Core{

    public static class MoveGen{

        static Bitboard[] kingAttacks = AttackGenerator.generateKingMasks();
        static Bitboard[] knightAttacks = AttackGenerator.generateKnightMasks();
        static Bitboard[][] pawnAttacks = AttackGenerator.generatePawnAttackMasks();

        public static List<Move> genKingMoves(int start, BoardState currentBoard){

            List<Move> moves = new List<Move>();
            Bitboard currentAttacks = new Bitboard(kingAttacks[start]);
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
            Bitboard currentAttacks = new Bitboard(knightAttacks[start]);
            currentAttacks &= ~(currentBoard.bitboards[currentBoard.currentTurn]);

            ulong currBit = currentAttacks.popLeastSignificantBit();
            while(currBit != 64){
                moves.Add(new Move(start, (int)currBit));
                currBit = currentAttacks.popLeastSignificantBit();
            }
            return moves;
        }

        public static List<Move> genPawnAttacks(int start, BoardState currentBoard){
            List<Move> moves = new List<Move>();
            Bitboard currentAttacks = new Bitboard(pawnAttacks[currentBoard.currentTurn][start]);
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