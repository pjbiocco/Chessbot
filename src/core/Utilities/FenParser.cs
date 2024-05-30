using System.Security.Cryptography.X509Certificates;

namespace Chess.Core{

    public static class FenParser
    {
        public const string defaultStartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1";

        

        public static BoardState makeDefaultBoard() => makeBoard(defaultStartFEN);
        public static BoardState makeBoard(String fenString){

            int[] board = new int[64];
            int isWhite;
            int castleRights;
            string enPassant;
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

            Bitboard[] bitboards = {white , black, pawn, knight, bishop, rook, queen, king};

            string[] fenParts = fenString.Split(' ');

            int i = 0; 
            foreach(char c in fenParts[0]){
                if(char.IsNumber(c)) i+= (c - '0');
                else if(c == '/') continue;
                else{
                    int currentPiece = Piece.getPieceFromSymbol(c);
                    board[i] = currentPiece;

                    if(Piece.isWhite(currentPiece)) bitboards[(int) PieceType.WHITE].setBit(i);
                    else                            bitboards[(int) PieceType.BLACK].setBit(i);

                    switch(Piece.pieceMask & currentPiece) {
                        case Piece.pawn:
                            bitboards[(int) PieceType.PAWN].setBit(i);
                            break;
                        case Piece.knight: 
                            bitboards[(int) PieceType.KNIGHT].setBit(i);
                            break;
                        case Piece.bishop:
                            bitboards[(int) PieceType.BISHOP].setBit(i);
                            break;
                        case Piece.rook:
                            bitboards[(int) PieceType.ROOK].setBit(i);
                            break;
                        case Piece.queen: 
                            bitboards[(int) PieceType.QUEEN].setBit(i);
                            break;
                        case Piece.king:
                            bitboards[(int) PieceType.KING].setBit(i);
                            break;
                    }
                    
                    i++;
                }
            }

            isWhite = fenParts[1].ToLower() == "w" ? (int) PieceType.WHITE : (int) PieceType.BLACK;
            castleRights = castleRightsFromString(fenParts[2]);
            enPassant = fenParts[3];
            halfClock =  Int32.Parse(fenParts[4]);
            fullClock =  Int32.Parse(fenParts[5]);

            return new BoardState(board, isWhite, castleRights, enPassant, halfClock, fullClock,
                                  bitboards);
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