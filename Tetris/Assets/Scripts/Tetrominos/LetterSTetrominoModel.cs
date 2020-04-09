using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterSTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterSNumLines = 3;
        private const int LetterSNumColumns = 3;
        private const int MaxRotations = 2;

        // Readonlies
        private static readonly int[,] Horizontal = new int[LetterSNumLines, LetterSNumColumns]
            {
                { 0,                    0,                    0                    },
                { 0,                    Constants.SPieceType, Constants.SPieceType },
                { Constants.SPieceType, Constants.SPieceType, 0                    }
            };
        private static readonly int[,] Vertical = new int[LetterSNumLines, LetterSNumColumns]
            {
                { Constants.SPieceType,  0,                   0                    },
                { Constants.SPieceType, Constants.SPieceType, 0                    },
                { 0,                    Constants.SPieceType, 0                    }
            };
        

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterSNumLines;
        public int NumColumns => LetterSNumColumns;
        public int[,] Blocks { get; private set; }

        // Private
        private int _currentRotation = 0;

        public LetterSTetrominoModel()
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
                Blocks = Horizontal;
            }
            else
            {
                Blocks = Vertical;
            }
        }
    }
}