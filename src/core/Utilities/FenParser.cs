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

            string[] fenParts = fenString.Split(' ');

            int i = 0; 
            foreach(char c in fenParts[0]){
                //Console.WriteLine(c);
                if(char.IsNumber(c)) i+= (c - '0');
                else if(c == '/') continue;
                else{
                    board[i] = Piece.getPieceTypeFromSymbol(c);
                    i++;
                }
            }

            isWhite = fenParts[1] == "w";
            castleRights = castleRightsFromString(fenParts[2]);
            enPassant = fenParts[3];
            halfClock =  Int32.Parse(fenParts[4]);
            fullClock =  Int32.Parse(fenParts[5]);

            return new BoardState(board, isWhite, castleRights, enPassant, halfClock, fullClock);
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