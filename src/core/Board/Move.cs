namespace Chess.Core
{
    public readonly struct Move
    {

        readonly ushort value; //16 bit moves

        //Flags
        public const int NoFlag = 0b0000;
		public const int EnPassantCaptureFlag = 0b0001;
		public const int CastleFlag = 0b0010;
		public const int PawnTwoUpFlag = 0b0011;

        //Promo flags
        public const int PromoteToQueenFlag = 0b0100;
		public const int PromoteToKnightFlag = 0b0101;
		public const int PromoteToRookFlag = 0b0110;
		public const int PromoteToBishopFlag = 0b0111;

        //Masks
        const ushort startSquareMask = 0b0000000000111111;
        const ushort endSquareMask = 0b0000111111000000;
        const ushort flagMask = 0b1111000000000000;

        public Move(ushort moveValue){
            value = moveValue;
        }

        public Move(int startSquare, int endSquare){
            value = (ushort)(startSquare | endSquare << 6);
        }

        public Move(int startSquare, int endSquare, int flag){
            value = (ushort)(startSquare | endSquare << 6 | flag << 12);
        }

        public bool isNull => value == 0;
        public static Move nullMove = new Move(0);
        
        public static bool sameMove(Move a, Move b) => a.value == b.value;

        public int startSquare => value & startSquareMask;
        public int targetSquare => (value & endSquareMask) >> 6;
        public int moveFlags => value >> 12;

        public bool isPromotion => moveFlags >= PromoteToQueenFlag;

        public void printMove(){
            Console.WriteLine("Start:" + startSquare + " End:" + targetSquare); 
        } 
        
        public int PromotionPieceType{
           get{
                switch(moveFlags){
                    case PromoteToRookFlag:
                        return Piece.rook;
                    case PromoteToKnightFlag:
                        return Piece.knight;
                    case PromoteToBishopFlag:
                        return Piece.bishop;
                    case PromoteToQueenFlag:
                        return Piece.queen;
                    default:
                        return Piece.empty;
                }
           } 
        }

    }
}