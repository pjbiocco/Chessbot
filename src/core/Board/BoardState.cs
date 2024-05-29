namespace Chess.Core{

    public class BoardState{

        List<Move> moveList = new List<Move>();

        public Bitboard[] bitboards {get; set;} 


        int[] board = new int[64];
        public int currentTurn = BitboardType.WHITE;
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

        public void applyMove(Move m){

        }

        public void printBoard(){
            Console.WriteLine("-----------------");
            for(int i = 0; i < 8; i++){
                for(int j = 0; j < 8; j++){
                    Console.Write("|" + Piece.getSymbol(board[i*8 + j]));
                }
                Console.WriteLine("|\n-----------------");
            }
            Console.WriteLine("Turn: " + (currentTurn == BitboardType.WHITE ? "White" : "Black"));
            Console.WriteLine("Castle Rights Code: " + castleRights);
            Console.WriteLine("En Passant Target: " + enPassant);
            Console.WriteLine("Half Clock: " + halfClock);
            Console.WriteLine("Full Clock: " + fullClock);
        }

    }
}