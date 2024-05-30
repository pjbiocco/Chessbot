namespace Chess.Core{

    public struct MoveList{
        public int index {get; set;}
        public Move[] moves {get; set;}

        public MoveList(){
            index = 0; 
            moves = new Move[256];
        }

        public void add(Move m){
            moves[index] = m;
            index++;
        }
    }

}