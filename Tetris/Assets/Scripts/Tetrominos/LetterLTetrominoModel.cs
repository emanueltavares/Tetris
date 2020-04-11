using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterLTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterLNumLines = 3;
        private const int LetterLNumColumns = 3;
        private const int MaxRotations = 4;

        // Readonlies
        private static readonly int[,] First = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    0,                    0                    },
                { Constants.LPieceType, Constants.LPieceType, Constants.LPieceType },
                { Constants.LPieceType, 0,                    0                    }
            };

        private static readonly int[,] Second = new int[LetterLNumLines, LetterLNumColumns]
            {
                { Constants.LPieceType, Constants.LPieceType, 0                    },
                { 0,                    Constants.LPieceType, 0                    },
                { 0,                    Constants.LPieceType, 0                    }
            };

        private static readonly int[,] Third = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    0,                    0                    },
                { 0,                    0,                    Constants.LPieceType },
                { Constants.LPieceType, Constants.LPieceType, Constants.LPieceType },
            };

        private static readonly int[,] Fourth = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    Constants.LPieceType, 0                    },
                { 0,                    Constants.LPieceType, 0                    },
                { 0,                    Constants.LPieceType, Constants.LPieceType }
            };

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterLNumLines;
        public int NumColumns => LetterLNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Constants.MaxNumLinesPreview, Constants.MaxNumColumnsPreview]
        {
            { 0, Constants.LPieceType, Constants.LPieceType, Constants.LPieceType },
            { 0, Constants.LPieceType, 0,                    0                    }
        };

        // Private
        private int _currentRotation = 0;

        public LetterLTetrominoModel()
        {
            Blocks = First;
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
            switch (_currentRotation)
            {
                case 0:
                    Blocks = First;
                    break;
                case 1:
                    Blocks = Second;
                    break;
                case 2:
                    Blocks = Third;
                    break;
                case 3:
                    Blocks = Fourth;
                    break;
            }
        }
    }
}