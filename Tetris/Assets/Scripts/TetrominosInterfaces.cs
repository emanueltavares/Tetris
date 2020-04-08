namespace Tetris.Models
{
    public interface ITetrominoModel
    {
        int Width { get; }
        int Height { get; }
        int[,] Blocks { get; }
        void RotateClockwise();
        void RotateCounterClockwise();
    }
}
