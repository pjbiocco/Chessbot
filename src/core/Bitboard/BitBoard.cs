using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

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
    }
}