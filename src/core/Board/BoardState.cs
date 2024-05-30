namespace Chess.Core{

    public class BoardState{

        public Bitboard[] bitboards {get; set;} 


        int[] board = new int[64];
        public int currentTurn = (int) PieceType.WHITE;
        public int castleRights = 15; //By default, everyone can castle!
        public string enPassant = "-";
        public int halfClock = 0;
        public int fullClock = 0;

        public BoardState(int[] board, int currentTurn, int castleRights, string enPassant, int halfClock, int fullClock,
                          Bitboard[] bitboards){
            this.board = board;
            this.currentTurn = currentTurn;
            this.castleRights = castleRights;
            this.enPassant = enPassant;
            this.halfClock = halfClock;
            this.fullClock = fullClock;
            this.bitboards = bitboards;
        }

        public void printBoard(){
            Console.WriteLine("-----------------");
            for(int i = 0; i < 8; i++){
                for(int j = 0; j < 8; j++){
                    Console.Write("|" + Piece.getSymbol(board[i*8 + j]));
                }
                Console.WriteLine("|\n-----------------");
            }
            Console.WriteLine("Turn: " + (currentTurn == (int)PieceType.WHITE ? "White" : "Black"));
            Console.WriteLine("Castle Rights Code: " + castleRights);
            Console.WriteLine("En Passant Target: " + enPassant);
            Console.WriteLine("Half Clock: " + halfClock);
            Console.WriteLine("Full Clock: " + fullClock);
        }

        public Bitboard getWhitePawns(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.PAWN]; }
        public Bitboard getBlackPawns(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.PAWN]; }
        public Bitboard getWhiteKnights(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.KNIGHT]; }
        public Bitboard getBlackKnights(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.KNIGHT]; }
        public Bitboard getWhiteBishops(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.BISHOP]; }
        public Bitboard getBlackBishops(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.BISHOP]; }
        public Bitboard getWhiteRooks(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.ROOK]; }
        public Bitboard getBlackRooks(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.ROOK]; }
        public Bitboard getWhiteQueens(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.QUEEN]; }
        public Bitboard getBlackQueens(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.QUEEN]; }
        public Bitboard getWhiteKing(){ return bitboards[(int) PieceType.WHITE] & bitboards[(int) PieceType.KING]; }
        public Bitboard getBlackKing(){ return bitboards[(int) PieceType.BLACK] & bitboards[(int) PieceType.KING]; }

        public Bitboard getCurrTurnPawns(){ return currentTurn == (int) PieceType.WHITE ? getWhitePawns() : getBlackPawns(); }
        public Bitboard getCurrTurnKnights(){ return currentTurn == (int) PieceType.WHITE ? getWhiteKnights() : getBlackKnights(); }
        public Bitboard getCurrTurnBishops(){ return currentTurn == (int) PieceType.WHITE ? getWhiteBishops() : getBlackBishops(); }
        public Bitboard getCurrTurnRooks(){ return currentTurn == (int) PieceType.WHITE ? getWhiteRooks() : getBlackRooks(); }
        public Bitboard getCurrTurnQueens(){ return currentTurn == (int) PieceType.WHITE ? getWhiteQueens() : getBlackQueens(); }
        public Bitboard getCurrTurnKing(){ return currentTurn == (int) PieceType.WHITE ? getWhiteKing() : getBlackKing(); }

        public Bitboard getCurrTurn(){ return currentTurn == (int) PieceType.WHITE ? bitboards[(int)PieceType.WHITE] : bitboards[(int)PieceType.BLACK];}

        public Bitboard getOccupiedSquares() { return bitboards[(int) PieceType.BLACK] | bitboards[(int) PieceType.WHITE]; }
       

    }
}