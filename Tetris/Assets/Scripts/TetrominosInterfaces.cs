namespace Tetris.Models
{
    public interface ITetrominoModel
    {
        int NumLines { get; }
        int NumColumns { get; }
        int[,] Blocks { get; }
        void RotateClockwise();
        void RotateCounterClockwise();
    }
}
