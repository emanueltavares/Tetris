namespace Tetris.Models
{
    public class LetterTTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterTNumLines = 3;
        private const int LetterTNumColumns = 3;
        private const int MaxRotations = 4;

        // Readonlies
        private static readonly int[,] First = new int[LetterTNumLines, LetterTNumColumns]
            {
                { 0,                    0,                    0                    },
                { Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType },
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    }
            };

        private static readonly int[,] Second = new int[LetterTNumLines, LetterTNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    },
                { Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    },
                
            };

        private static readonly int[,] Third = new int[LetterTNumLines, LetterTNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    },
                { Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType },
                { 0,                    0,                    0                    }
            };

        private static readonly int[,] Fourth = new int[LetterTNumLines, LetterTNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType },
                { 0,                    Utils.TetrominoUtils.TPieceType, 0                    },
            };

        // Properties
        public int Type => Utils.TetrominoUtils.TPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterTNumLines;
        public int NumColumns => LetterTNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
            { 0, Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType, Utils.TetrominoUtils.TPieceType },
            { 0,  0,                   Utils.TetrominoUtils.TPieceType, 0                    }
        };

        // Private
        private int _currentRotation = 0;

        public LetterTTetrominoModel()
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