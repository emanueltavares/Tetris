namespace Tetris.Models
{
    public class BoardModel : IBoardModel
    {
        public int NumLines { get; private set; }
        public int NumColumns { get; private set; }
        public int[,] Blocks { get; private set; }

        public class Builder
        {            
            public IBoardModel Build(int[,] blocks)
            {
                if (blocks == null)
                {
                    blocks = new int[0, 0];
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