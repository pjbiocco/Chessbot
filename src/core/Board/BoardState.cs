using static Chess.Core.Color;
using static Chess.Core.PieceType;
using static Chess.Core.Square;
using static Chess.Core.PositionMask;

namespace Chess.Core{

    public class BoardState{

        public Bitboard[] occupancy {get; set;}
        public Bitboard[] pieces {get; set;} 


        int[] board = new int[64];
        public int currentTurn = (int) WHITE;
        public int castleRights = 15; //By default, everyone can castle!
        public Square enPassant = NONE;
        public int halfClock = 0;
        public int fullClock = 0;

        public BoardState(int[] board, int currentTurn, int castleRights, Square enPassant, int halfClock, int fullClock,
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

        public Bitboard getCurrTurnBoard(){ return occupancy[currentTurn];}
        public Bitboard getOppTurnBoard(){return currentTurn == (int) WHITE ? occupancy[(int)BLACK] : occupancy[(int)WHITE];}
        public Bitboard getCurrTurnPieceBoard(PieceType piece){ return getCurrTurnBoard() & pieces[(int)piece];}
        public Bitboard getColorPieceBoard(Color color, PieceType piece) {return occupancy[(int) color] & pieces[(int)piece]; }
        public Bitboard getOccupiedSquaresBoard() { return occupancy[(int)WHITE] | occupancy[(int)BLACK]; }
        public int getCurrKingCastleRights(){return currentTurn == (int) WHITE ? castleRights & (int) WHITE_CASTLE_KING_MASK : castleRights & (int) BLACK_CASTLE_QUEEN_MASK;}
        public int getCurrQueenCastleRights(){return currentTurn == (int) WHITE ? castleRights & (int) WHITE_CASTLE_QUEEN_MASK : castleRights & (int) BLACK_CASTLE_QUEEN_MASK;}
       

    }
}