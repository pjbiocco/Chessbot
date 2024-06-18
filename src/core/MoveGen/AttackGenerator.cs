using static Chess.Core.Direction;

namespace Chess.Core
{
    public static class AttackGenerator{

        public static Bitboard[] generateKingMasks(){
            Bitboard[] kingBoards = new Bitboard[64]; 
            
            for(int i = 0; i < kingBoards.Length; i++){

                kingBoards[i] = new Bitboard();
                kingBoards[i].setBit(i);

                kingBoards[i] = kingBoards[i].shiftBoard(UP)        |
                                kingBoards[i].shiftBoard(DOWN)      |
                                kingBoards[i].shiftBoard(LEFT)      |
                                kingBoards[i].shiftBoard(RIGHT)     |
                                kingBoards[i].shiftBoard(UPRIGHT)   |
                                kingBoards[i].shiftBoard(UPLEFT)    |
                                kingBoards[i].shiftBoard(DOWNRIGHT) |
                                kingBoards[i].shiftBoard(DOWNLEFT)  & ~kingBoards[i];  
                                
            }
            return kingBoards;
        }

        public static Bitboard[] generateKnightMasks(){
            Bitboard[] knightBoards = new Bitboard[64];
            
            for(int i = 0; i < knightBoards.Length; i++){
                knightBoards[i] = new Bitboard();
                knightBoards[i].setBit(i);

                knightBoards[i] = knightBoards[i].shiftBoard(UPUPLEFT)          |
                                  knightBoards[i].shiftBoard(UPUPRIGHT)         |
                                  knightBoards[i].shiftBoard(DOWNDOWNLEFT)      |
                                  knightBoards[i].shiftBoard(DOWNDOWNRIGHT)     |
                                  knightBoards[i].shiftBoard(LEFTLEFTDOWN)      |
                                  knightBoards[i].shiftBoard(LEFTLEFTUP)        |
                                  knightBoards[i].shiftBoard(RIGHTRIGHTDOWN)    |
                                  knightBoards[i].shiftBoard(RIGHTRIGHTUP)      & ~knightBoards[i]; 
            }
            return knightBoards;
        }

        public static Bitboard[][] generatePawnAttackMasks(){
            
            Bitboard[] whitePawns = new Bitboard[64];
            Bitboard[] blackPawns = new Bitboard[64];

            for(int i = 0; i < whitePawns.Length; i++){
                whitePawns[i] = new Bitboard();
                blackPawns[i] = new Bitboard();

                whitePawns[i].setBit(i);

                blackPawns[i] = whitePawns[i].shiftBoard(DOWNRIGHT)   |
                                whitePawns[i].shiftBoard(DOWNLEFT)    & ~whitePawns[i];

                whitePawns[i] = whitePawns[i].shiftBoard(UPRIGHT)   |
                                whitePawns[i].shiftBoard(UPLEFT)    & ~whitePawns[i];

                
            }

            Bitboard[][] result = {whitePawns, blackPawns};
        
            return result;
        }

        public static Bitboard[] generateBishopAttackMasks(){
            Bitboard[] bishopMoves = new Bitboard[64];

            for(int i = 0; i < bishopMoves.Length; i++){

                bishopMoves[i] = new Bitboard();
                bishopMoves[i].setBit(i);
                
                Bitboard bishopMoveUpLeft = new Bitboard();
                bishopMoveUpLeft.setBit(i);

                Bitboard bishopMoveUpRight = new Bitboard();
                bishopMoveUpRight.setBit(i);

                Bitboard bishopMoveDownLeft = new Bitboard();
                bishopMoveDownLeft.setBit(i);

                Bitboard bishopMoveDownRight = new Bitboard();
                bishopMoveDownRight.setBit(i);

                for(int j = 0; j < 7; j++)   bishopMoveUpLeft |= bishopMoveUpLeft.shiftBoard(UPLEFT);
                for(int j = 0; j < 7; j++)   bishopMoveUpRight |= bishopMoveUpRight.shiftBoard(UPRIGHT);
                for(int j = 0; j < 7; j++)   bishopMoveDownLeft |= bishopMoveDownLeft.shiftBoard(DOWNLEFT);
                for(int j = 0; j < 7; j++)   bishopMoveDownRight |= bishopMoveDownRight.shiftBoard(DOWNRIGHT);

                
                
                bishopMoves[i] = (bishopMoveUpLeft | bishopMoveUpRight | bishopMoveDownLeft | bishopMoveDownRight) & ~bishopMoves[i];

            }
            return bishopMoves;
        }

        public static Bitboard[] generateRookAttackMasks(){
            Bitboard[] rookMoves = new Bitboard[64];

            for(int i = 0; i < rookMoves.Length; i++){
                rookMoves[i] = new Bitboard((ulong)(Bitboard.getRank((Square) i) ^ Bitboard.getFile((Square) i)));
            }
            return rookMoves;
        }
    }
}

    