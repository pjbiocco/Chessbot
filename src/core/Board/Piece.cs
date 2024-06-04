using System.Dynamic;

namespace Chess.Core{

    public static class Piece
    {

        // Masks
        public const int colorMask = 0b1000;
        public const int pieceMask = 0b0111;

        // Types of squares/pieces. First 3 bits 
        
        public const int pawn = 0;
        public const int knight = 1; 
        public const int bishop = 2; 
        public const int rook = 3; 
        public const int queen = 4; 
        public const int king = 5;
        public const int empty = 6;

        //Piece Color 
        public const int white = 0; 
        public const int black = 8; 

        //Piece Types + color
        public const int whitePawn = pawn | white;
        public const int whiteKnight = knight | white;
        public const int whiteBishop = bishop | white;
        public const int whiteRook = rook | white;
        public const int whiteQueen = queen | white;
        public const int whiteKing = king | white; 

        public const int blackPawn = pawn | black;
        public const int blackKnight = knight | black;
        public const int blackBishop = bishop | black;
        public const int blackRook = rook | black;
        public const int blackQueen = queen | black;
        public const int blackKing = king | black;

        public static int MakePiece(int pieceType, int pieceColor) => pieceType | pieceColor;
        public static int MakePiece(int pieceType, bool pieceIsWhite) => MakePiece(pieceType, pieceIsWhite ? white : black);

        public static bool isColor(int piece, int color) => (piece & colorMask) == white && piece != 6; 
        public static bool isWhite(int piece) => isColor(piece, white);

        public static int getPieceColor(int piece) => piece & colorMask;
        public static int getPieceType(int piece) => piece & pieceMask;

        public static bool isDiagonal(int piece) => getPieceType(piece) is queen or bishop; //getPieceType(piece) == queen || getPieceType(piece) == bishop;
        public static bool isOrthogonal(int piece) => getPieceType(piece) is queen or rook;
        public static bool isSlidingPiece(int piece) => getPieceType(piece) is queen or rook or bishop;

        public static char getSymbol(int piece){
            int pieceType = getPieceType(piece);

            char symbol = pieceType switch
            {
                pawn => 'P',
                rook => 'R',
                knight => 'N',
                bishop => 'B', 
                queen => 'Q',
                king => 'K',
                _ => ' '
            };

            symbol = isWhite(piece) ? symbol : char.ToLower(symbol);

            return symbol;
        }

        public static int getPieceFromSymbol(char symbol){
            int color = char.IsUpper(symbol) ? white : black;
            symbol = char.ToUpper(symbol);

            return color + symbol switch
            {

                'P' => pawn,
                'R' => rook,
                'N' => knight,
                'B' => bishop,
                'Q' => queen,
                'K' => king, 
                _ => empty
            };
        }
    }
}