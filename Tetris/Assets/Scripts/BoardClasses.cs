namespace Tetris.Models
{
    public class BoardModel : IBoardModel
    {
        public int NumLines { get; private set; }
        public int NumColumns { get; private set; }
        public bool[,] Blocks { get; private set; }

        public class Builder
        {            
            public IBoardModel Build(bool[,] blocks)
            {
                if (blocks == null)
                {
                    blocks = new bool[0, 0];
                }

                IBoardModel boardModel = new BoardModel()
                {
                    NumLines = blocks.GetLength(0),
                    NumColumns = blocks.GetLength(1),
                    Blocks = blocks
                };
                return boardModel;
            }
        }
    }
}