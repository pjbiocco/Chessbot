namespace Chess.Core
{
    public readonly struct Move
    {

        readonly ushort value; //16 bit moves

        //Masks
        const ushort startSquareMask = 0b0000000000111111;
        const ushort endSquareMask = 0b0000111111000000;
        const ushort flagMask = 0b1111000000000000;

        public Move(ushort moveValue){
            value = moveValue;
        }

        public Move(Square startSquare, Square endSquare){
            value = (ushort)((int) startSquare | (int) endSquare << 6);
        }

        public Move(Square startSquare, Square endSquare, MoveFlag flag){
            value = (ushort)((int) startSquare | (int) endSquare << 6 | (int) flag << 12);
        }

        public bool isNull => value == 0;
        public static Move nullMove = new Move(0);
        
        public static bool sameMove(Move a, Move b) => a.value == b.value;

        public Square startSquare =>(Square)(value & startSquareMask);
        public Square targetSquare => (Square)((value & endSquareMask) >> 6);
        public int moveFlag => value >> 12;

        public void printMove(){
            Console.WriteLine("Start:" + startSquare + " End:" + targetSquare + " Flag: " + moveFlag); 
        }

        public String toString(){
            return startSquare + "" + targetSquare;
        }

    }
}