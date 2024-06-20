using static Chess.Core.PositionMask;
using static Chess.Core.Direction;
using static Chess.Core.Square;

namespace Chess.Core{

    public static class BlockerBoard{

        public static Bitboard[] rookTrimMask = genTrimmedMoves(true);
        public static Bitboard[] bishopTrimMask = genTrimmedMoves(false);

        public static Bitboard[] blockerMasks(Bitboard moveMask, Square square, bool isRook){

            Bitboard trimmed = new Bitboard(moveMask.bitboard);
            trimmed &= isRook ? rookTrimMask[(int)square] : bishopTrimMask[(int)square];

            List<int> activeIndicies = new List<int>(); 
            for(int i = 0; i < 64; i++){
                if(trimmed.getBit(i) == 1) activeIndicies.Add(i);
            }

            int numBits = 1 << (int)trimmed.countBits();
            Bitboard[] blockerBoards = new Bitboard[numBits];

            for(int i = 0; i < numBits; i++ ){
                for(int j = 0; j < (int)trimmed.countBits(); j++){
                    int bit = (i >> j) & 1;
                    blockerBoards[i] |= (ulong) bit << activeIndicies[j];
                }
            }

            return blockerBoards;
        }

        public static Bitboard genRookMoveMask(Bitboard moveMask, Square square){

            Bitboard rookMoves = new Bitboard();

            Square currSquare = square;
            
            int rank = Bitboard.getRankNum(square);
            int file = Bitboard.getFileNum(square);

            for(int i = 0; i < rank; i++){
                currSquare += (int) DOWN;
                rookMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            currSquare = square;

            for(int i = 0; i < 7-rank; i++){
                currSquare += (int) UP;
                rookMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            currSquare = square;

             for(int i = 0; i < file; i++){
                currSquare += (int) LEFT;
                rookMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }
            currSquare = square;

            for(int i = 0; i < 7 - file; i++){
                currSquare += (int) RIGHT;
                rookMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            return rookMoves;
        }

        public static Bitboard genBishopMoveMask(Bitboard moveMask, Square square){

            Bitboard bishopMoves = new Bitboard();

            Square currSquare = square;
            
            int rank = Bitboard.getRankNum(square);
            int file = Bitboard.getFileNum(square);

            int urCount = 7-rank < 7-file ? 7-rank : 7-file;
            int ulCount = 7-rank < file ? 7-rank : file;
            int drCount = rank < 7-file ? rank : 7-file;
            int dlCount = rank < file ? rank : file;

            for(int i = 0; i < urCount; i++){
                currSquare += (int) UPRIGHT;
                bishopMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            currSquare = square;

            for(int i = 0; i < ulCount; i++){
                currSquare += (int) UPLEFT;
                bishopMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            currSquare = square;

             for(int i = 0; i < drCount; i++){
                currSquare += (int) DOWNRIGHT;
                bishopMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }
            currSquare = square;

            for(int i = 0; i < dlCount; i++){
                currSquare += (int) DOWNLEFT;
                bishopMoves.setBit((int)currSquare);
                if(moveMask.getBit((int)currSquare) == 1) break;
            }

            return bishopMoves;
        }

        public static Bitboard[] genTrimmedMoves(bool isRook){

            Bitboard[] attacks = isRook ? AttackGenerator.generateRookAttackMasks() : AttackGenerator.generateBishopAttackMasks();


            for(Square i = a8; (int)i < 64; i++){
            

                Bitboard start = new Bitboard();
                start.setBit((int)i);

                Bitboard trimmed = new Bitboard(attacks[(int)i].bitboard);
            
                trimmed = (start & RANK1_MASK) == 0 ? trimmed & ~RANK1_MASK : trimmed;
                trimmed = (start & RANK8_MASK) == 0 ? trimmed & ~RANK8_MASK : trimmed;
                trimmed = (start & FILEA_MASK) == 0 ? trimmed & ~FILEA_MASK : trimmed;
                trimmed = (start & FILEH_MASK) == 0 ? trimmed & ~FILEH_MASK : trimmed;

                attacks[(int)i] = trimmed;
            }

            return attacks;
        }
    }

}