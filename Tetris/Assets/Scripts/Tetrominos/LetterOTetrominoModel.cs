using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterOTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterONumLines = 3;
        private const int LetterONumColums = 2;

        private static readonly int[,] Vertical = new int[LetterONumLines, LetterONumColums]
            {
                {0,                    0 },
                {Constants.OPieceType, Constants.OPieceType},
                {Constants.OPieceType, Constants.OPieceType}
            };

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterONumLines;
        public int NumColumns => LetterONumColums;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Constants.MaxNumLinesPreview, Constants.MaxNumColumnsPreview]
        {
            {0, Constants.OPieceType, Constants.OPieceType, 0 },
            {0, Constants.OPieceType, Constants.OPieceType, 0 },
        };

        public LetterOTetrominoModel()
        {
            Blocks = Vertical;
        }

        public void RotateClockwise() { }

        public void RotateCounterClockwise() { }
    }
}