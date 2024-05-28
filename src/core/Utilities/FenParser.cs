using System.Security.Cryptography.X509Certificates;

namespace Chess.Core{

    public static class FenParser
    {
        public const string defaultStartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        

        public static BoardState makeDefaultBoard() => makeBoard(defaultStartFEN);

        public static BoardState makeBoard(String fenString){

            int[] board = new int[64];
            bool isWhite;
            int castleRights;
            string enPassant;
            int halfClock;
            int fullClock;

            Bitboard[] bitboards = new Bitboard[8];

            Bitboard black = new Bitboard();
            Bitboard white = new Bitboard();
            Bitboard pawn = new Bitboard();
            Bitboard knight = new Bitboard();
            Bitboard bishop = new Bitboard();
            Bitboard rook = new Bitboard();
            Bitboard queen = new Bitboard();
            Bitboard king = new Bitboard();

            string[] fenParts = fenString.Split(' ');

            int i = 0; 
            foreach(char c in fenParts[0]){
                if(char.IsNumber(c)) i+= (c - '0');
                else if(c == '/') continue;
                else{
                    int currentPiece = Piece.getPieceFromSymbol(c);
                    board[i] = currentPiece;

                    if(Piece.isWhite(currentPiece)) bitboards[BitboardType.WHITE].setBit(i);
                    else                            bitboards[BitboardType.BLACK].setBit(i);

                    switch(Piece.pieceMask & currentPiece) {
                        case Piece.pawn:
                            bitboards[BitboardType.PAWN].setBit(i);
                            break;
                        case Piece.knight: 
                            bitboards[BitboardType.KNIGHT].setBit(i);
                            break;
                        case Piece.bishop:
                            bitboards[BitboardType.BISHOP].setBit(i);
                            break;
                        case Piece.rook:
                            bitboards[BitboardType.ROOK].setBit(i);
                            break;
                        case Piece.queen: 
                            bitboards[BitboardType.QUEEN].setBit(i);
                            break;
                        case Piece.king:
                            bitboards[BitboardType.KING].setBit(i);
                            break;
                    }
                    
                    i++;
                }
            }

            isWhite = fenParts[1] == "w";
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