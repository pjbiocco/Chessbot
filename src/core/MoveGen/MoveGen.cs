using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;
using static Chess.Core.Square;
using static Chess.Core.Direction;
using static Chess.Core.PositionMask;

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

            Direction shiftDirection = currentBoard.currentTurn == (int)PieceType.WHITE ? UP : DOWN;
            PositionMask rankMask = currentBoard.currentTurn == (int)PieceType.WHITE ? RANK3_MASK: RANK6_MASK;

            Bitboard pawnPush = currentBoard.getCurrTurnPawns().shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquares();
            Bitboard pawnDoublePush = pawnPush;

            Square push = pawnPush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-(int)shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnPush.popLeastSignificantBit();
            }
            
            pawnDoublePush = ((ulong)rankMask & pawnDoublePush).shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquares();
                        
            push = pawnDoublePush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-2*(int)shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnDoublePush.popLeastSignificantBit();
            }           
            return moveList;
        }
    }
}