namespace Tetris.Models
{
    public interface ITetrominoModel
    {
        int CurrentLine { get; set; }
        int CurrentColumn { get; set; }
        int NumLines { get; }
        int NumColumns { get; }
        int Type { get; }
        int[,] Blocks { get; }
        int[,] BlocksPreview { get; }
        void RotateClockwise();
        void RotateCounterClockwise();
    }
}
