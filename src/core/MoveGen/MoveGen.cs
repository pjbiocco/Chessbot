using static Chess.Core.Square;
using static Chess.Core.Direction;
using static Chess.Core.PositionMask;
using static Chess.Core.MoveFlag;
using static Chess.Core.Color;
using static Chess.Core.PieceType;

namespace Chess.Core{

    public static class MoveGen{

        static Bitboard[] kingAttacks = AttackGenerator.generateKingMasks();
        static Bitboard[] knightAttacks = AttackGenerator.generateKnightMasks();
        static Bitboard[][] pawnAttacks = AttackGenerator.generatePawnAttackMasks();

        static Move[][] castleMoves = {new Move[]{ new Move(e1, g1, KING_CASTLE), new Move(e1, c1, QUEEN_CASTLE)}, 
                                       new Move[]{ new Move(e8, g8, KING_CASTLE), new Move(e8, c8, QUEEN_CASTLE)} };

        static ulong[][] castleMasks = {new ulong[]{0x6000000000000000,0xe00000000000000}, new ulong[]{0x60,0xe}};



        public static MoveList genKingMoves(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard currentAttacks = kingAttacks[(int)start] & ~currentBoard.getCurrTurnBoard();
            Square attackSquare= currentAttacks.popLeastSignificantBit();            

            if(currentBoard.getCurrKingCastleRights() != 0 && (currentBoard.getOccupiedSquaresBoard() & castleMasks[currentBoard.currentTurn][0]) == 0) 
                moveList.add(castleMoves[currentBoard.currentTurn][0]);
            if(currentBoard.getCurrQueenCastleRights() != 0 && (currentBoard.getOccupiedSquaresBoard() & castleMasks[currentBoard.currentTurn][1]) == 0) 
                moveList.add(castleMoves[currentBoard.currentTurn][1]);
           

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(currentBoard.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                moveList.add(new Move(start, attackSquare, flag));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genKnightMoves(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard currentAttacks = knightAttacks[(int)start] & ~currentBoard.getCurrTurnBoard(); 
            Square attackSquare = currentAttacks.popLeastSignificantBit();

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(currentBoard.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                moveList.add(new Move(start, attackSquare, flag));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genPawnAttacks(Square start, BoardState currentBoard, MoveList moveList){
            Bitboard currentAttacks = pawnAttacks[currentBoard.currentTurn][(int)start] & ~currentBoard.getCurrTurnBoard();
            Square attackSquare = currentAttacks.popLeastSignificantBit();
            while(attackSquare != NONE){
                moveList.add(new Move(start, attackSquare));
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        public static MoveList genAllPawnPushes(BoardState currentBoard, MoveList moveList){

            Direction shiftDirection = currentBoard.currentTurn == (int)WHITE ? UP : DOWN;
            PositionMask rankMask = currentBoard.currentTurn == (int)WHITE ? RANK3_MASK: RANK6_MASK;

            Bitboard pawnPush = currentBoard.getCurrTurnPieceBoard(PAWN).shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquaresBoard();
            Bitboard pawnDoublePush = pawnPush;

            Square push = pawnPush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-(int)shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnPush.popLeastSignificantBit();
            }
            
            pawnDoublePush = ((ulong)rankMask & pawnDoublePush).shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquaresBoard();

            push = pawnDoublePush.popLeastSignificantBit();
            while(push != NONE){
                moveList.add(new Move(push-2*(int)shiftDirection, push)); //Subtraction because we're undoing a shift.
                push = pawnDoublePush.popLeastSignificantBit();
            }           
            return moveList;
        }
    }
}