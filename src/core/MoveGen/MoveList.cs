namespace Chess.Core{

    public struct MoveList{
        public int length {get; set;}
        public Move[] moves {get; set;}

        public MoveList(){
            length = 0; 
            moves = new Move[256];
        }

        public void add(Move m){
            moves[length] = m;
            length++;
        }
    }

}