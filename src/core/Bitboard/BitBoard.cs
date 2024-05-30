
using static Chess.Core.Direction;
using static Chess.Core.PositionMask;

namespace Chess.Core{
    public struct Bitboard{

        ulong bitboard {get; set;} 

        public Bitboard(){
            bitboard = 0;
        }

        public Bitboard(ulong bitboard){
            this.bitboard = bitboard;
        }

        public void setBit(int square){
            bitboard |= 1UL << square;
        }

        public ulong getBit(int square){
            return bitboard >> square & 1;
        }

        public void removeBit(int square){
            bitboard &= ~(1UL << square);
        }

        public Bitboard shiftBoard(Direction dir){
            if(dir == DOWN)         return bitboard << (int) dir;
            if(dir == UP)           return bitboard >> (int) dir * -1;
            if(dir == RIGHT)        return (bitboard & ~(ulong)FILEH_MASK) << (int) dir;
            if(dir == LEFT)         return (bitboard & ~(ulong)FILEA_MASK) >> (int) dir * -1;

            if(dir == UPRIGHT)      return (bitboard & ~(ulong)FILEH_MASK) >> (int) dir * -1;
            if(dir == UPLEFT)       return (bitboard & ~(ulong)FILEA_MASK) >> (int) dir * -1;
            if(dir == DOWNRIGHT)    return (bitboard & ~(ulong)FILEH_MASK) << (int) dir;
            if(dir == DOWNLEFT)     return (bitboard & ~(ulong)FILEA_MASK) << (int) dir;

            if(dir == UPUPLEFT)         return (bitboard & ~(ulong)FILEA_MASK) >> (int) dir * -1;
            if(dir == UPUPRIGHT)        return (bitboard & ~(ulong)FILEH_MASK) >> (int) dir * -1;
            if(dir == DOWNDOWNLEFT)     return (bitboard & ~(ulong)FILEA_MASK) << (int) dir;
            if(dir == DOWNDOWNRIGHT)    return (bitboard & ~(ulong)FILEH_MASK) << (int) dir;

            if(dir == LEFTLEFTDOWN)     return (bitboard & ~(ulong)FILEA_MASK & ~(ulong)FILEB_MASK) << (int) dir;
            if(dir == LEFTLEFTUP)       return (bitboard & ~(ulong)FILEA_MASK & ~(ulong)FILEB_MASK) >> (int) dir * -1;
            if(dir == RIGHTRIGHTDOWN)   return (bitboard & ~(ulong)FILEH_MASK & ~(ulong)FILEG_MASK) << (int) dir;
            if(dir == RIGHTRIGHTUP)     return (bitboard & ~(ulong)FILEH_MASK & ~(ulong)FILEG_MASK) >> (int) dir * -1;
            
            Console.WriteLine("Shift not supported.");
            return bitboard; 
        }

        public Square getLeastSignificantBit(){
            return (Square) ulong.TrailingZeroCount(bitboard);
        }

        public Square popLeastSignificantBit(){
            Square lsb = getLeastSignificantBit();
            bitboard = bitboard & (bitboard - 1);
            return lsb;
        }

        public ulong countBits(){
            return ulong.PopCount(bitboard);
        }

        public void printBitBoard(){
            for(int i = 0; i < 8; i++){
                for(int j = 0; j < 8; j++){
                    Console.Write(getBit(i * 8 + j));
                }
                Console.WriteLine();
            }
        }

        public static implicit operator ulong(Bitboard bb) => bb.bitboard;
        public static implicit operator Bitboard(ulong u) => new Bitboard(u);
        public static Bitboard operator &(Bitboard bb1, Bitboard bb2) => bb1.bitboard & bb2.bitboard;
        public static Bitboard operator |(Bitboard bb1, Bitboard bb2) => bb1.bitboard | bb2.bitboard;

        public static Bitboard operator &(Bitboard bb, ulong u) => bb.bitboard & u;
        public static Bitboard operator &(ulong u, Bitboard bb) => u & bb.bitboard;
        public static Bitboard operator |(Bitboard bb, ulong u) => bb.bitboard | u;
        public static Bitboard operator |(ulong u, Bitboard bb) => u | bb.bitboard;

        public static Bitboard operator &(Bitboard bb, PositionMask m) => bb.bitboard & (ulong)m;
        public static Bitboard operator &(PositionMask m, Bitboard bb) => (ulong)m & bb.bitboard;
        public static Bitboard operator |(Bitboard bb, PositionMask m) => bb.bitboard & (ulong)m;
        public static Bitboard operator |(PositionMask m, Bitboard bb) => (ulong)m & bb.bitboard;
    }
}