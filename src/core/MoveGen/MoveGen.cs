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

        static Bitboard[] rookAttacks = AttackGenerator.generateRookAttackMasks();
        static Bitboard[][] rookDict = MagicGen.makeRookDict();

        static Bitboard[] bishopAttacks = AttackGenerator.generateBishopAttackMasks();
        static Bitboard[][] bishopDict = MagicGen.makeBishopDict();


        static Move[][] castleMoves = {new Move[]{ new Move(e1, g1, KING_CASTLE), new Move(e1, c1, QUEEN_CASTLE)}, 
                                       new Move[]{ new Move(e8, g8, KING_CASTLE), new Move(e8, c8, QUEEN_CASTLE)} };

        static ulong[][] castleMasks = {new ulong[]{0x6000000000000000,0xe00000000000000}, new ulong[]{0x60,0xe}};

        public static MoveList genBoardMoves(BoardState boardState, MoveList moveList){
            moveList = genKingMoves(boardState, moveList);
            moveList = genQueenMoves(boardState, moveList);
            moveList = genRookMoves(boardState, moveList);
            moveList = genBishopMoves(boardState, moveList);
            moveList = genKnightMoves(boardState, moveList);
            moveList = genPawnMoves(boardState, moveList);

            return moveList;
        }

        public static MoveList genKingMoves(BoardState currentBoard, MoveList moveList){

            Bitboard kingBoard = currentBoard.getCurrTurnPieceBoard(KING);
            Square start = kingBoard.popLeastSignificantBit();

            Bitboard currentAttacks = kingAttacks[(int)start] & ~currentBoard.getCurrTurnBoard();
            Square attackSquare= currentAttacks.popLeastSignificantBit();      

            if(currentBoard.getCurrKingCastleRights() != 0 &&
                 (currentBoard.getOccupiedSquaresBoard() & castleMasks[(int)currentBoard.currentTurn][0]) == 0){
                Move castle = castleMoves[(int)currentBoard.currentTurn][0];
                if(!isCastleBlocked(currentBoard, castle))moveList.add(castle);
            }
            if(currentBoard.getCurrQueenCastleRights() != 0 && (currentBoard.getOccupiedSquaresBoard() & castleMasks[(int)currentBoard.currentTurn][1]) == 0){ 
                Move castle = castleMoves[(int)currentBoard.currentTurn][1];
                if(!isCastleBlocked(currentBoard, castle))moveList.add(castle);;
            }

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(currentBoard.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                Move move = new Move(start, attackSquare, flag);
                if(testMove(currentBoard, move)) moveList.add(move);

                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        private static MoveList genKnightMoves(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard currentAttacks = knightAttacks[(int)start] & ~currentBoard.getCurrTurnBoard(); 
            Square attackSquare = currentAttacks.popLeastSignificantBit();

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(currentBoard.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                Move move = new Move(start, attackSquare, flag);
                if(testMove(currentBoard, move)) moveList.add(move);
                
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        private static MoveList genPawnAttacks(Square start, BoardState currentBoard, MoveList moveList){

            Bitboard epBoard = new Bitboard();
            if(currentBoard.enPassant != NONE) epBoard.setBit((int)currentBoard.enPassant);

            Bitboard currentAttacks = pawnAttacks[(int)currentBoard.currentTurn][(int)start] & ~currentBoard.getCurrTurnBoard() & (currentBoard.getOppTurnBoard() | epBoard);
            
            // if(currentBoard.enPassant != NONE){
            //     Direction epCheckLeft = currentBoard.currentTurn == (int) WHITE ? UPLEFT : DOWNLEFT; 
            //     Direction epCheckRight = currentBoard.currentTurn == (int) WHITE ? UPRIGHT : DOWNRIGHT;
            //     if((int)currentBoard.enPassant == (int)epCheckLeft + (int)start){moveList.add(new Move(start, currentBoard.enPassant, EP_CAPTURE));}
            //     if((int)currentBoard.enPassant == (int)epCheckRight + (int)start){moveList.add(new Move(start, currentBoard.enPassant, EP_CAPTURE));}
            // }

            Square attackSquare = currentAttacks.popLeastSignificantBit();
            while(attackSquare != NONE){
                Bitboard promoCheck = new Bitboard();
                promoCheck.setBit((int)attackSquare);

                if((promoCheck & currentBoard.getPromotionRank()) != 0){
                    if(testMove(currentBoard, new Move(start, attackSquare, QUEEN_PROMO_CAP))){
                        moveList.add(new Move(start, attackSquare, QUEEN_PROMO_CAP));
                        moveList.add(new Move(start, attackSquare, ROOK_PROMO_CAP));
                        moveList.add(new Move(start, attackSquare, BISHOP_PROMO_CAP));
                        moveList.add(new Move(start, attackSquare, KNIGHT_PROMO_CAP));
                    }
                } else {
                    MoveFlag flag =  attackSquare != currentBoard.enPassant ? CAPTURE : EP_CAPTURE;
                    Move move = new Move(start, attackSquare, flag);
                    if(testMove(currentBoard, move)) moveList.add(move);
                }                
                attackSquare = currentAttacks.popLeastSignificantBit();
            }
            return moveList;
        }

        private static MoveList genAllPawnPushes(BoardState currentBoard, MoveList moveList){

            Direction shiftDirection = currentBoard.currentTurn == (int)WHITE ? UP : DOWN;
            PositionMask rankMask = currentBoard.currentTurn == (int)WHITE ? RANK3_MASK: RANK6_MASK;

            Bitboard pawnPush = currentBoard.getCurrTurnPieceBoard(PAWN).shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquaresBoard();
            Bitboard pawnDoublePush = pawnPush;

            Square push = pawnPush.popLeastSignificantBit();
            while(push != NONE){

                Bitboard promoCheck = new Bitboard();
                promoCheck.setBit((int)push);

                if((promoCheck & currentBoard.getPromotionRank()) != 0){
                    if(testMove(currentBoard, new Move(push-(int)shiftDirection, push, QUEEN_PROMO))){
                        moveList.add(new Move(push-(int)shiftDirection, push, QUEEN_PROMO));
                        moveList.add(new Move(push-(int)shiftDirection, push, ROOK_PROMO));
                        moveList.add(new Move(push-(int)shiftDirection, push, BISHOP_PROMO));
                        moveList.add(new Move(push-(int)shiftDirection, push, KNIGHT_PROMO));
                    }
                } else {
                    Move move = new Move(push-(int) shiftDirection, push, QUIET);
                    if(testMove(currentBoard, move)) moveList.add(move);
                }
                push = pawnPush.popLeastSignificantBit();
            }
            
            pawnDoublePush = ((ulong)rankMask & pawnDoublePush).shiftBoard(shiftDirection) & ~currentBoard.getOccupiedSquaresBoard();

            push = pawnDoublePush.popLeastSignificantBit();
            while(push != NONE){
                Move move = new Move(push-2*(int)shiftDirection, push, DOUBLEPAWN_PUSH);
                if(testMove(currentBoard, move)) moveList.add(move); //Subtraction because we're undoing a shift.
                push = pawnDoublePush.popLeastSignificantBit();
            }           
            return moveList;
        }

        private static Bitboard buildRookBoard(Square square, BoardState boardState){
            Bitboard blocker = boardState.getOccupiedSquaresBoard() & 
                               rookAttacks[(int)square]             &
                               BlockerBoard.rookTrimMask[(int)square];

            ulong val =  blocker.bitboard * MagicGen.rMagics[(int)square]>> (64-MagicGen.rBits[(int)square]);

            Bitboard moves = new Bitboard(     
                rookDict[(int)square][val].bitboard
            );
            moves &= ~boardState.getCurrTurnBoard();
            return moves;
        }

        private static MoveList genRookMoves(Square square, BoardState boardState, MoveList moveList){
            Bitboard moves = buildRookBoard(square, boardState);

            Square attackSquare = moves.popLeastSignificantBit();

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(boardState.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                Move move = new Move(square, attackSquare, flag);
                if(testMove(boardState, move)) moveList.add(move);

                attackSquare = moves.popLeastSignificantBit();
            }
            return moveList;
        }

        private static Bitboard buildBishopBoard(Square square, BoardState boardState){
            Bitboard blocker = boardState.getOccupiedSquaresBoard() & 
                               bishopAttacks[(int)square]           &
                               BlockerBoard.bishopTrimMask[(int) square];

            ulong val =  blocker.bitboard * MagicGen.bMagics[(int)square]>> (64-MagicGen.bBits[(int)square]);

            Bitboard moves = new Bitboard(     
                bishopDict[(int)square][val].bitboard
            );
            moves &= ~boardState.getCurrTurnBoard();
            return moves;
        }

        private static MoveList genBishopMoves(Square square, BoardState boardState, MoveList moveList){
            Bitboard moves = buildBishopBoard(square, boardState);

            Square attackSquare = moves.popLeastSignificantBit();

            while(attackSquare != NONE){
                MoveFlag flag = QUIET;
                if(boardState.getOppTurnBoard().getBit((int)attackSquare) != 0) flag = CAPTURE;

                Move move = new Move(square, attackSquare, flag);
                if(testMove(boardState, move)) moveList.add(move);

                attackSquare = moves.popLeastSignificantBit();
            }
            return moveList;
        }

        private static MoveList genQueenMoves(Square square, BoardState boardState, MoveList moveList){

            moveList = genBishopMoves(square, boardState, moveList);
            moveList = genRookMoves(square, boardState, moveList);

            return moveList;
        }

        public static MoveList genPawnMoves(BoardState boardState, MoveList moveList){

            moveList = genAllPawnPushes(boardState, moveList);
            Bitboard pawnOccupancy = boardState.getColorPieceBoard(boardState.currentTurn, PAWN);

            while(pawnOccupancy != 0){
                Square square = pawnOccupancy.popLeastSignificantBit();
                moveList = genPawnAttacks(square, boardState, moveList);
            } 
            return moveList;
        }

        public static MoveList genKnightMoves(BoardState boardState, MoveList moveList){
            Bitboard knightOccupancy = boardState.getColorPieceBoard(boardState.currentTurn, KNIGHT);

            while(knightOccupancy != 0){
                Square square = knightOccupancy.popLeastSignificantBit();
                moveList = genKnightMoves(square, boardState, moveList);
            } 
            return moveList;
        }

        public static MoveList genBishopMoves(BoardState boardState, MoveList moveList){
            Bitboard bishopOccupancy = boardState.getColorPieceBoard(boardState.currentTurn, BISHOP);

            while(bishopOccupancy != 0){
                Square square = bishopOccupancy.popLeastSignificantBit();
                moveList = genBishopMoves(square, boardState, moveList);
            } 
            return moveList;
        }

        public static MoveList genRookMoves(BoardState boardState, MoveList moveList){
            Bitboard rookOccupancy = boardState.getColorPieceBoard(boardState.currentTurn, ROOK);

            while(rookOccupancy != 0){
                Square square = rookOccupancy.popLeastSignificantBit();
                moveList = genRookMoves(square, boardState, moveList);
            } 
            return moveList;
        }

        public static MoveList genQueenMoves(BoardState boardState, MoveList moveList){
            Bitboard queenOccupancy = boardState.getColorPieceBoard(boardState.currentTurn, QUEEN);

            while(queenOccupancy != 0){
                Square square = queenOccupancy.popLeastSignificantBit();
                moveList = genQueenMoves(square, boardState, moveList);
            } 
            return moveList;
        } 

        public static bool inCheck(BoardState currentBoard){

            Color oppColor = currentBoard.currentTurn == WHITE ? BLACK : WHITE;
            Square kingSquare = currentBoard.getColorPieceBoard(currentBoard.currentTurn, KING).popLeastSignificantBit();
            
            ulong pawnChecks = currentBoard.getColorPieceBoard(oppColor, PAWN) & pawnAttacks[(int)currentBoard.currentTurn][(int)kingSquare];
            if(pawnChecks != 0) return true;
            ulong knightChecks = currentBoard.getColorPieceBoard(oppColor, KNIGHT) & knightAttacks[(int)kingSquare];
            if(knightChecks != 0) return true;
            ulong kingChecks = currentBoard.getColorPieceBoard(oppColor, KING) & kingAttacks[(int)kingSquare];
            if(kingChecks != 0) return true;

            ulong rookChecks = (currentBoard.getColorPieceBoard(oppColor, ROOK) |
                                currentBoard.getColorPieceBoard(oppColor, QUEEN)) &
                                buildRookBoard(kingSquare, currentBoard);
            if(rookChecks != 0) return true;

            ulong bishopChecks = (currentBoard.getColorPieceBoard(oppColor, BISHOP) |
                                  currentBoard.getColorPieceBoard(oppColor, QUEEN)) &
                                  buildBishopBoard(kingSquare, currentBoard);
            if(bishopChecks != 0) return true;

            return false;
        }

        public static bool isCastleBlocked(BoardState currentBoard, Move move){

            Color color = currentBoard.currentTurn;            
            MoveFlag flag = (MoveFlag)move.moveFlag;

            if(inCheck(currentBoard)) return true;

            BoardState castled = new BoardState(currentBoard);
            castled.applyMove(move);
            castled.currentTurn = currentBoard.currentTurn;
            if(inCheck(castled)) return true;

            BoardState mid = new BoardState(currentBoard);

            if(color == WHITE && flag == KING_CASTLE){  
                mid.applyMove(new Move(e1,f1,QUIET));
                mid.currentTurn = currentBoard.currentTurn;
                if(inCheck(mid)) return true;

                return false;
            }

            if(color == WHITE && flag == QUEEN_CASTLE){
                mid.applyMove(new Move(e1,d1,QUIET));
                mid.currentTurn = currentBoard.currentTurn;
                if(inCheck(mid)) return true;

                return false;
            }

            if(color == BLACK && flag == KING_CASTLE){  
                mid.applyMove(new Move(e8,f8,QUIET));
                mid.currentTurn = currentBoard.currentTurn;
                if(inCheck(mid)) return true;

                return false;
            }

            if(color == BLACK && flag == QUEEN_CASTLE){
                mid.applyMove(new Move(e8,d8,QUIET));
                mid.currentTurn = currentBoard.currentTurn;
                if(inCheck(mid)) return true;

                return false;
            }
            return true;
        }

        public static bool testMove(BoardState currentBoard, Move move){
            BoardState testBoard = new BoardState(currentBoard);
            testBoard.applyMove(move);
            testBoard.currentTurn = currentBoard.currentTurn;
            
            return !inCheck(testBoard);
        }
    }
}