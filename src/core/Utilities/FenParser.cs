using static Chess.Core.Color;
using static Chess.Core.PieceType;
using static Chess.Core.Square;

namespace Chess.Core{

    public static class FenParser
    {
        public const string defaultStartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public static BoardState makeDefaultBoard() => makeBoard(defaultStartFEN);
        public static BoardState makeBoard(string fenString){

            int[] board = new int[64];
            Array.Fill(board, 6);
            Color isWhite;
            int castleRights;
            Square enPassant;
            int halfClock;
            int fullClock;
            
            Bitboard white = new Bitboard();
            Bitboard black = new Bitboard();
            Bitboard pawn = new Bitboard();
            Bitboard knight = new Bitboard();
            Bitboard bishop = new Bitboard();
            Bitboard rook = new Bitboard();
            Bitboard queen = new Bitboard();
            Bitboard king = new Bitboard();

            Bitboard[] occupancy = {white, black};
            Bitboard[] pieces = {white, black, pawn, knight, bishop, rook, queen, king};

            string[] fenParts = fenString.Split(' ');

            int i = 0; 
            foreach(char c in fenParts[0]){
                if(char.IsNumber(c)) i+= c - '0';
                else if(c == '/') continue;
                else{
                    int currentPiece = Piece.getPieceFromSymbol(c);
                    board[i] = currentPiece;

                    if(Piece.isWhite(currentPiece)) occupancy[(int) WHITE].setBit(i);
                    else                            occupancy[(int) BLACK].setBit(i);

                    switch(Piece.pieceMask & currentPiece) {
                        case Piece.pawn:
                            pieces[(int) PAWN].setBit(i);
                            break;
                        case Piece.knight: 
                            pieces[(int) KNIGHT].setBit(i);
                            break;
                        case Piece.bishop:
                            pieces[(int) BISHOP].setBit(i);
                            break;
                        case Piece.rook:
                            pieces[(int) ROOK].setBit(i);
                            break;
                        case Piece.queen: 
                            pieces[(int) QUEEN].setBit(i);
                            break;
                        case Piece.king:
                            pieces[(int) KING].setBit(i);
                            break;
                    }
                    
                    i++;
                }
            }

            isWhite = fenParts[1].ToLower() == "w" ?  WHITE : BLACK;
            castleRights = castleRightsFromString(fenParts[2]);

            enPassant = fenParts[3] == "-" ? NONE : (Square) Enum.Parse(typeof(Square), fenParts[3]) ;
            halfClock =  Int32.Parse(fenParts[4]);
            fullClock =  Int32.Parse(fenParts[5]);

            return new BoardState(board, isWhite, castleRights, enPassant, halfClock, fullClock,
                                  occupancy, pieces);
        }


        public static int castleRightsFromString(String rights){
            int rightNum = 0;
            rightNum = rights.Contains('K') ? rightNum + 1 : rightNum;
            rightNum = rights.Contains('Q') ? rightNum + 2 : rightNum;
            rightNum = rights.Contains('k') ? rightNum + 4 : rightNum;
            rightNum = rights.Contains('q') ? rightNum + 8 : rightNum;

            return rightNum;
        }
    }
}