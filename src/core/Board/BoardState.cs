using static Chess.Core.Color;
using static Chess.Core.PieceType;
using static Chess.Core.Square;
using static Chess.Core.PositionMask;
using static Chess.Core.MoveFlag;
using static Chess.Core.Direction;

namespace Chess.Core{

    [Serializable]
    public class BoardState{

        public Bitboard[] occupancy {get; set;}
        public Bitboard[] pieces {get; set;} 


        public int[] board = new int[64];
        public Color currentTurn = WHITE;
        public int castleRights = 15; //By default, everyone can castle!
        public Square enPassant = NONE;
        public int halfClock = 0;
        public int fullClock = 0;

        public BoardState(int[] board, Color currentTurn, int castleRights, Square enPassant, int halfClock, int fullClock,
                          Bitboard[] occupancy, Bitboard[] pieces){
            this.board = board;
            this.currentTurn = currentTurn;
            this.castleRights = castleRights;
            this.enPassant = enPassant;
            this.halfClock = halfClock;
            this.fullClock = fullClock;
            this.occupancy = occupancy;
            this.pieces = pieces;
        }

        public BoardState(BoardState clone){
            board = (int[]) clone.board.Clone();
            currentTurn = clone.currentTurn;
            castleRights = clone.castleRights;
            enPassant = clone.enPassant;
            halfClock = clone.halfClock;
            fullClock = clone.fullClock;
            occupancy = (Bitboard[]) clone.occupancy.Clone();
            pieces = (Bitboard[]) clone.pieces.Clone();
        }


        public void applyMove(Move move){

            Square from = move.startSquare;
            Square to = move.targetSquare;
            MoveFlag flag = (MoveFlag) move.moveFlag;

            Color opp = currentTurn == WHITE ? BLACK : WHITE;

            int pieceFrom = board[(int)from];

            if(currentTurn == BLACK) fullClock++; 
            if((pieceFrom & Piece.pieceMask) != 0 && flag != CAPTURE) halfClock++;

            if(castleRights > 0) updateCaslteRights(move);

            if(flag == EP_CAPTURE){
                Direction ep_direction = currentTurn == WHITE ? DOWN : UP;
                pieces[(int)PAWN].removeBit((int)to + (int)ep_direction);
                occupancy[(int) opp].removeBit((int)to + (int)ep_direction);
                board[(int)to + (int)ep_direction] = 6; 
            }
            else if((flag & CAPTURE) != 0){
                int capturedPiece = board[(int)to];
                pieces[capturedPiece & Piece.pieceMask].removeBit((int)to);
                occupancy[(int) opp].removeBit((int)to);
            }

            board[(int)from] = 6;
            pieces[pieceFrom & Piece.pieceMask].removeBit((int) from);
            occupancy[(int)currentTurn].removeBit((int)from);
            occupancy[(int)currentTurn].setBit((int) to);

            if((flag & KNIGHT_PROMO) == 0){
                board[(int)to] = pieceFrom;
                pieces[pieceFrom & Piece.pieceMask].setBit((int) to);
            } else {
                promotePawn(move);
            }

            if(flag == QUEEN_CASTLE){
                queenSideCastle(move);
            } else if(flag == KING_CASTLE){
                kingSideCastle(move);
            }

            if(flag == DOUBLEPAWN_PUSH) {enPassant = currentTurn == (int)WHITE ? to + (int)DOWN : to + (int)UP; return;}
            else                        {enPassant = NONE;}

            currentTurn = (currentTurn == WHITE) ? BLACK : WHITE;
            
        }

        public void promotePawn(Move move){

            Square to = move.targetSquare;
            MoveFlag flag = (MoveFlag) move.moveFlag;

            int pieceColor = currentTurn == WHITE ? 0 : 8; //This differs from Color.WHITE and Color.BLACK for masking purposes. See Piece.cs
            switch ((int)flag & 3){
                case 0: 
                    board[(int)to] = Piece.knight + pieceColor;
                    pieces[(int)KNIGHT].setBit((int) to);
                break;
                case 1:
                    board[(int)to] = Piece.bishop + pieceColor;
                    pieces[(int)BISHOP].setBit((int) to);
                break;
                case 2:
                    board[(int)to] = Piece.rook + pieceColor;
                    pieces[(int)ROOK].setBit((int) to);
                break;
                case 3:
                    board[(int)to] = Piece.queen + pieceColor;
                    pieces[(int)QUEEN].setBit((int) to);
                break;
            }
        }

        public void updateCaslteRights(Move move){

            Square from = move.startSquare;
            Square to = move.targetSquare;
            int pieceFrom = board[(int)from];

            if((pieceFrom & Piece.pieceMask) == (int)KING)
                castleRights &= currentTurn == WHITE ? ~(int)WHITE_CASTLE_MASK : ~(int)BLACK_CASTLE_MASK;

            if(from == a1 || to == a1)      castleRights &= ~(int)WHITE_CASTLE_QUEEN_MASK;
            else if(from == h1 || to == h1) castleRights &= ~(int)WHITE_CASTLE_KING_MASK;
            else if(from == a8 || to == a8) castleRights &= ~(int)BLACK_CASTLE_QUEEN_MASK;
            else if(from == h8 || to == h8) castleRights &= ~(int)BLACK_CASTLE_KING_MASK;
            
        }

        public void kingSideCastle(Move move){
            Square to = move.targetSquare;

            int pieceColor = currentTurn == WHITE ? 0 : 8;
            int rookFrom = (int)to + (int)RIGHT;
            int rookTo = (int)to + (int)LEFT;

            board[rookFrom] = 6;
            board[rookTo] = 3 + pieceColor; 

            pieces[(int)ROOK].removeBit(rookFrom);
            pieces[(int)ROOK].setBit(rookTo);

            occupancy[(int)currentTurn].removeBit(rookFrom);
            occupancy[(int)currentTurn].setBit(rookTo);
            
        }

        public void queenSideCastle(Move move){

            Square from = move.startSquare;
            Square to = move.targetSquare;

            int pieceColor = currentTurn == WHITE ? 0 : 8;
            int rookFrom = (int)to + (int)LEFT + (int)LEFT;
            int rookTo = (int)from + (int)LEFT;

            board[rookFrom] = 6;
            board[rookTo] = 3 + pieceColor; 

            pieces[(int)ROOK].removeBit(rookFrom);
            pieces[(int)ROOK].setBit(rookTo);

            occupancy[(int)currentTurn].removeBit(rookFrom);
            occupancy[(int)currentTurn].setBit(rookTo);
        }

        public void printBoard(){
            Console.WriteLine("-----------------");
            for(int i = 0; i < 8; i++){
                for(int j = 0; j < 8; j++){
                    Console.Write("|" + Piece.getSymbol(board[i*8 + j]));
                }
                Console.WriteLine("|\n-----------------");
            }
            Console.WriteLine("Turn: " + (currentTurn == (int) WHITE ? "White" : "Black"));
            Console.WriteLine("Castle Rights Code: " + castleRights);
            Console.WriteLine("En Passant Target: " + enPassant);
            Console.WriteLine("Half Clock: " + halfClock);
            Console.WriteLine("Full Clock: " + fullClock);
        }

        public Bitboard getCurrTurnBoard(){ return occupancy[(int)currentTurn];}
        public Bitboard getOppTurnBoard(){return currentTurn == WHITE ? occupancy[(int)BLACK] : occupancy[(int)WHITE];}
        public Bitboard getCurrTurnPieceBoard(PieceType piece){ return getCurrTurnBoard() & pieces[(int)piece];}
        public Bitboard getColorPieceBoard(Color color, PieceType piece) {return occupancy[(int) color] & pieces[(int)piece]; }
        public Bitboard getOccupiedSquaresBoard() { return occupancy[(int)WHITE] | occupancy[(int)BLACK]; }
        public int getCurrKingCastleRights(){return currentTurn == WHITE ? castleRights & (int) WHITE_CASTLE_KING_MASK : castleRights & (int) BLACK_CASTLE_QUEEN_MASK;}
        public int getCurrQueenCastleRights(){return currentTurn == WHITE ? castleRights & (int) WHITE_CASTLE_QUEEN_MASK : castleRights & (int) BLACK_CASTLE_QUEEN_MASK;}
        public PositionMask getPromotionRank(){return currentTurn == WHITE ? RANK8_MASK : RANK1_MASK;}

    }
}