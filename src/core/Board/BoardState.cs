namespace Chess.Core{

    public class BoardState{

        int[] board = new int[64];
        bool isWhiteTurn = true;
        int castleRights = 15; //By default, everyone can castle!
        string enPeasant = "-";
        int halfClock = 0;
        int fullClock = 0;

        public BoardState(int[] board, bool isWhiteTurn, int castleRights, String enPeasant, int halfClock, int fullClock){
            this.board = board;
            this.isWhiteTurn = isWhiteTurn;
            this.castleRights = castleRights;
            this.enPeasant = enPeasant;
            this.halfClock = halfClock;
            this.fullClock = fullClock;
        }

        public void printBoard(){
            Console.WriteLine("-----------------");
            for(int i = 0; i < 8; i++){
                for(int j = 0; j < 8; j++){
                    Console.Write("|" + Piece.getSymbol(board[i*8 + j]));
                }
                Console.WriteLine("|\n-----------------");
            }
            Console.WriteLine();
        }

    }
}