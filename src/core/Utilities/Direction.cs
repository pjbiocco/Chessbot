namespace Chess.Core{
    public enum Direction
    {
        //KING MOVES
        UP = -8,
        DOWN = 8,
        LEFT = -1,
        RIGHT = 1,
        UPLEFT = UP + LEFT,
        UPRIGHT = UP + RIGHT,
        DOWNLEFT= DOWN + LEFT,
        DOWNRIGHT = DOWN + RIGHT,
        
        //KNIGHT MOVES
        UPUPLEFT = UP + UP + LEFT,
        UPUPRIGHT = UP + UP + RIGHT,
        DOWNDOWNLEFT = DOWN + DOWN + LEFT, 
        DOWNDOWNRIGHT = DOWN + DOWN + RIGHT,
        LEFTLEFTUP = LEFT + LEFT + UP,
        LEFTLEFTDOWN = LEFT + LEFT + DOWN,
        RIGHTRIGHTUP = RIGHT + RIGHT + UP,
        RIGHTRIGHTDOWN = RIGHT + RIGHT + DOWN
    }
}