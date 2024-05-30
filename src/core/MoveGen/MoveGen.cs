using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;
using static Chess.Core.Square;

namespace Chess.Core{

    public static class MoveGen{

        const ulong rank8Mask = 0xff;
        const ulong rank7Mask = 0xff00;
        const ulong rank6Mask = 0xff0000;
        const ulong rank5Mask = 0xff000000;
        const ulong rank4Mask = 0xff00000000;
        const ulong rank3Mask = 0xff0000000000;
        const ulong rank2Mask = 0xff000000000000;
        const ulong rank1Mask = 0xff00000000000000;

        static Bitboard[] kingAttacks = AttackGenerator.generateKingMasks();
        static Bitboard[] knightAttacks = AttackGenerator.generateKnightMasks();
        static Bitboard[][] pawnAttacks = AttackGenerator.generatePawnAttackMasks();

        public static MoveList genKingMoves(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard currentAttacks = kingAttacks[(int)start] & ~currentBoard.getCurrTurn();      
            Square attackSquare= currentAttacks.popLeastSignificantBit();

            while(attackSquare != NONE){
                moveList.add(new Move(start, attackSquare));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genKnightMoves(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard currentAttacks = knightAttacks[(int)start] & ~currentBoard.getCurrTurn(); 
            Square attackSquare = currentAttacks.popLeastSignificantBit();

            while(attackSquare != NONE){
                moveList.add(new Move(start, attackSquare));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genPawnAttacks(Square start, BoardState currentBoard, MoveList moveList){
            Bitboard currentAttacks = pawnAttacks[currentBoard.currentTurn][(int)start] & ~currentBoard.getCurrTurn();
            Square attackSquare = currentAttacks.popLeastSignificantBit();
            while(attackSquare != NONE){
                moveList.add(new Move(start, attackSquare));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genAllPawnPushes(BoardState currentBoard, MoveList moveList){         
            int shiftDirection = currentBoard.currentTurn == (int)PieceType.WHITE ? -8 : 8;
            Bitboard pawnPush = currentBoard.currentTurn == (int)PieceType.WHITE ? currentBoard.getCurrTurnPawns() >> 8 : currentBoard.getCurrTurnPawns() << 8; 
            pawnPush &= ~currentBoard.getOccupiedSquares();

            Square push = pawnPush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnPush.popLeastSignificantBit();
            }

            pawnPush = currentBoard.currentTurn == (int)PieceType.WHITE ? currentBoard.getCurrTurnPawns() >> 8 : currentBoard.getCurrTurnPawns() << 8; 
            pawnPush &= ~currentBoard.getOccupiedSquares();
            Bitboard pawnDoublePush = currentBoard.currentTurn == (int)PieceType.WHITE ? (rank3Mask & pawnPush) >> 8 : 
                                                                                         (rank6Mask & pawnPush) << 8;
            pawnDoublePush &= ~currentBoard.getOccupiedSquares();
            
            push = pawnDoublePush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-2*shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnDoublePush.popLeastSignificantBit();
            }           
            return moveList;
        }
    }
}