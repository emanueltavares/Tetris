using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterOTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterONumLines = 2;
        private const int LetterONumColums = 2;

        private static readonly int[,] Vertical = new int[LetterONumLines, LetterONumColums]
            {
                {Constants.OPieceType, Constants.OPieceType},
                {Constants.OPieceType, Constants.OPieceType}
            };

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterONumLines;
        public int NumColumns => LetterONumColums;
        public int[,] Blocks { get; private set; }

        public LetterOTetrominoModel()
        {
            Blocks = Vertical;
        }

        public void RotateClockwise() { }

        public void RotateCounterClockwise() { }
    }
}