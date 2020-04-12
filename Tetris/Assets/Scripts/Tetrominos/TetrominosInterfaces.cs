namespace Tetris.Models
{
    public interface ITetrominoModel
    {
        int CurrentLine { get; set; }
        int CurrentColumn { get; set; }
        int NumLines { get; }
        int NumColumns { get; }
        int PieceType { get; }
        int Rotation { get; set; }
        int MaxRotations { get; }
        int[,] Blocks { get; }
        int[,] BlocksPreview { get; }
    }
}
