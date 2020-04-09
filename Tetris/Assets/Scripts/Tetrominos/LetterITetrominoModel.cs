using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterITetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterINumLines = 4;
        private const int LetterINumColumns = 4;
        private const int MaxRotations = 2;

        // Readonlies
        private static readonly int[,] Vertical = new int[LetterINumLines, LetterINumColumns]
            {
                {0, 0, Constants.IPieceType, 0 },
                {0, 0, Constants.IPieceType, 0 },
                {0, 0, Constants.IPieceType, 0 },
                {0, 0, Constants.IPieceType, 0 }
            };
        private static readonly int[,] Horizontal = new int[LetterINumLines, LetterINumColumns]
            {
                {0, 0, 0, 0 },
                {Constants.IPieceType, Constants.IPieceType, Constants.IPieceType, Constants.IPieceType },
                {0, 0, 0, 0 },
                {0, 0, 0, 0 }
            };

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterINumLines;
        public int NumColumns => LetterINumColumns;
        public int[,] Blocks { get; private set; }        

        // Private
        private int _currentRotation = 0;

        public LetterITetrominoModel()
        {
            Blocks = Horizontal;
        }

        public void RotateClockwise()
        {
            _currentRotation += 1;
            _currentRotation = MathExt.Mod(_currentRotation, MaxRotations);
            SetRotation();
        }

        public void RotateCounterClockwise()
        {
            _currentRotation -= 1;
            _currentRotation = MathExt.Mod(_currentRotation, MaxRotations);
            SetRotation();
        }

        private void SetRotation()
        {
            if (_currentRotation == 0)
            {
                Blocks = Vertical;
            }
            else
            {
                Blocks = Horizontal;
            }
        }
    }
}