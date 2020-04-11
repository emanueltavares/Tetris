namespace Tetris.Models
{
    public class LetterZTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterZNumLines = 3;
        private const int LetterZNumColumns = 3;
        private const int MaxRotations = 2;

        // Readonlies
        private static readonly int[,] Horizontal = new int[LetterZNumLines, LetterZNumColumns]
            {
                { 0,                    0,                    0                    },
                { Utils.TetrominoUtils.ZPieceType, Utils.TetrominoUtils.ZPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.ZPieceType, Utils.TetrominoUtils.ZPieceType }
                
            };
        private static readonly int[,] Vertical = new int[LetterZNumLines, LetterZNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.ZPieceType, 0                    },
                { Utils.TetrominoUtils.ZPieceType, Utils.TetrominoUtils.ZPieceType, 0                    },
                { Utils.TetrominoUtils.ZPieceType,  0,                   0                    }
            };


        // Properties
        public int Type => Utils.TetrominoUtils.ZPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterZNumLines;
        public int NumColumns => LetterZNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
             { 0, Utils.TetrominoUtils.ZPieceType, Utils.TetrominoUtils.ZPieceType, 0                    },
             { 0, 0,                    Utils.TetrominoUtils.ZPieceType, Utils.TetrominoUtils.ZPieceType }
        };

        // Private
        private int _currentRotation = 0;

        public LetterZTetrominoModel()
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