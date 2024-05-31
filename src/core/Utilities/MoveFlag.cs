namespace Chess.Core{

    public enum MoveFlag : int{
        QUIET =             0b0000,
		DOUBLEPAWN_PUSH =   0b0001,
		KING_CASTLE =       0b0010,
		QUEEN_CASTLE =      0b0011,
        CAPTURE =           0b0100,
        EP_CAPTURE =        0b0101, // EP is En passant
        KNIGHT_PROMO =      0b1000,
        BISHOP_PROMO =      0b1001,
        ROOK_PROMO =        0b1010,
        QUEEN_PROMO =       0b0110,
        KNIGHT_PROMO_CAP =  0b1100,
        BISHOP_PROMO_CAP =  0b1101,
        ROOK_PROMO_CAP =    0b1110,
        QUEEN_PROMO_CAP =   0b1111
    }
}