namespace Application.Models
{
    public class LetterLTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterLNumLines = 3;
        private const int LetterLNumColumns = 3;

        // Readonlies
        private static readonly int[,] First = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    0,                    0                    },
                { Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType },
                { Utils.TetrominoUtils.LPieceType, 0,                    0                    }
            };

        private static readonly int[,] Second = new int[LetterLNumLines, LetterLNumColumns]
            {
                { Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.LPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.LPieceType, 0                    }
            };

        private static readonly int[,] Third = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    0,                    0                    },
                { 0,                    0,                    Utils.TetrominoUtils.LPieceType },
                { Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType },
            };

        private static readonly int[,] Fourth = new int[LetterLNumLines, LetterLNumColumns]
            {
                { 0,                    Utils.TetrominoUtils.LPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.LPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType }
            };

        // Properties
        public int PieceType => Utils.TetrominoUtils.LPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterLNumLines;
        public int NumColumns => LetterLNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
            { 0, Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType, Utils.TetrominoUtils.LPieceType },
            { 0, Utils.TetrominoUtils.LPieceType, 0,                    0                    }
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

        public LetterLTetrominoModel()
        {
            Blocks = First;
        }
    }
}