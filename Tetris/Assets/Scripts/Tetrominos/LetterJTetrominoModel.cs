namespace Tetris.Models
{
    public class LetterJTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterJNumLines = 3;
        private const int LetterJNumColumns = 3;

        // Readonlies
        private static readonly int[,] First = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    0,                    0                    },
                { Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType },
                { 0,                    0,                    Utils.TetrominoUtils.JPieceType }
            };

        private static readonly int[,] Second = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.JPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.JPieceType, 0                    },
                { Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType, 0                    }
            };

        private static readonly int[,] Third = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    0,                    0                    },
                { Utils.TetrominoUtils.JPieceType, 0,                    0                    },
                { Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType }
            };

        private static readonly int[,] Fourth = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType },
                { 0,                    Utils.TetrominoUtils.JPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.JPieceType, 0                    }
            };

        // Properties
        public int PieceType => Utils.TetrominoUtils.JPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterJNumLines;
        public int NumColumns => LetterJNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
            { 0, Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType, Utils.TetrominoUtils.JPieceType },
            { 0, 0,                    0,                    Utils.TetrominoUtils.JPieceType }
        };
        public int MaxRotations => 4;
        public int Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _rotation = MathExt.Mod(_rotation, MaxRotations);
                switch (_rotation)
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

        // Private
        private int _rotation = 0;

        public LetterJTetrominoModel()
        {
            Blocks = First;
        }
    }
}