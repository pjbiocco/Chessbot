namespace Chess.Core
{
    public static class AttackGenerator{

        //Important ranks for wrap around prevent
        private static int aCol = 0;
        private static int hCol = 7;
        private static int bCol = 1;
        private static int gCol = 6;

        //King directions
        private static int kingUp = 8;
        private static int kingDown = -8;
        private static int kingLeft = -1;
        private static int kingRight = 1;
        private static int kingUpLeft = kingUp + kingLeft;
        private static int kingUpRight = kingUp + kingRight;
        private static int kingDownLeft = kingDown + kingLeft;
        private static int kingDownRight = kingDown + kingRight;

        //Knight directions
        private static int knightUpUpLeft = kingUp + kingUpLeft;
        private static int knightUpUpRight = kingUp + kingUpRight;
        private static int knightUpLeftLeft = kingLeft + kingUpLeft; 
        private static int knightUpRightRight = kingRight + kingUpRight; 
        private static int knightDownDownLeft = kingDown + kingDownLeft; 
        private static int knightDownDownRight = kingDown + kingDownRight;
        private static int knightDownLeftLeft = kingLeft + kingDownLeft;
        private static int knightDownRightRight = kingRight + kingDownRight;

        public static Bitboard[] generateKingMasks(){

            Bitboard[] kingBoards = new Bitboard[64];
            
            for(int i = 0; i < kingBoards.Length; i++){
                kingBoards[i] = new Bitboard();

                if(validKingMove(i, kingUp)) kingBoards[i].setBit(i+kingUp);
                if(validKingMove(i, kingDown)) kingBoards[i].setBit(i+kingDown);
                if(validKingMove(i, kingLeft)) kingBoards[i].setBit(i+kingLeft);
                if(validKingMove(i, kingRight)) kingBoards[i].setBit(i+kingRight);
                if(validKingMove(i, kingUpRight)) kingBoards[i].setBit(i+kingUpRight);
                if(validKingMove(i, kingUpLeft)) kingBoards[i].setBit(i+kingUpLeft);
                if(validKingMove(i, kingDownRight)) kingBoards[i].setBit(i+kingDownRight);
                if(validKingMove(i, kingDownLeft)) kingBoards[i].setBit(i+kingDownLeft);
            }
            return kingBoards;
        }

        public static bool validKingMove(int location, int modifier){

            if(location + modifier < 0 || location + modifier >= 64)                                                     return false;
            if(location % 8 == aCol && (modifier == kingLeft || modifier == kingDownLeft || modifier == kingUpLeft))    return false;
            if(location % 8 == hCol && (modifier == kingRight || modifier == kingDownRight || modifier == kingUpRight)) return false;
            return true;
        }

        public static Bitboard[] generateKnightMasks(){
            Bitboard[] knightBoards = new Bitboard[64];
            
            for(int i = 0; i < knightBoards.Length; i++){
                knightBoards[i] = new Bitboard();

                if(validKnightMove(i, knightUpUpLeft)) knightBoards[i].setBit(i+knightUpUpLeft);
                if(validKnightMove(i, knightUpUpRight)) knightBoards[i].setBit(i+knightUpUpRight);
                if(validKnightMove(i, knightDownDownLeft)) knightBoards[i].setBit(i+knightDownDownLeft);
                if(validKnightMove(i, knightDownDownRight)) knightBoards[i].setBit(i+knightDownDownRight);
                if(validKnightMove(i, knightUpRightRight)) knightBoards[i].setBit(i+knightUpRightRight);
                if(validKnightMove(i, knightUpLeftLeft)) knightBoards[i].setBit(i+knightUpLeftLeft);
                if(validKnightMove(i, knightDownRightRight)) knightBoards[i].setBit(i+knightDownRightRight);
                if(validKnightMove(i, knightDownLeftLeft)) knightBoards[i].setBit(i+knightDownLeftLeft);
            }
            return knightBoards;
        }

        public static bool validKnightMove(int location, int modifier){

            if(location + modifier < 0 || location + modifier >= 64)                                             return false;
            if(location % 8 == aCol && (modifier == knightDownDownLeft || modifier == knightUpUpLeft ||
               modifier == knightUpLeftLeft || modifier == knightDownLeftLeft ))                                return false;
            if(location % 8 == hCol && (modifier == knightDownDownRight || modifier == knightUpUpRight ||
               modifier == knightUpRightRight || modifier == knightDownRightRight ))                            return false;
            if(location % 8 == bCol && (modifier == knightUpLeftLeft || modifier == knightDownLeftLeft ))       return false;
            if(location % 8 == gCol && (modifier == knightDownRightRight || modifier == knightUpRightRight))    return false;

            return true;
        }
    }
}

    