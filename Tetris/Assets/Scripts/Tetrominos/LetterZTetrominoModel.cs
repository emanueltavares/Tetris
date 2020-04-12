namespace Application.Models
{
    public class LetterZTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterZNumLines = 3;
        private const int LetterZNumColumns = 3;

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
        public int PieceType => Utils.TetrominoUtils.ZPieceType;
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
        public int MaxRotations => 2;
        public int Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _rotation = MathExt.Mod(_rotation, MaxRotations);
                if (_rotation == 0)
                {
                    Blocks = Horizontal;
                }
                else
                {
                    Blocks = Vertical;
                }
            }
        }

        // Private
        private int _rotation = 0;

        public LetterZTetrominoModel()
        {
            Blocks = Horizontal;
        }
    }
}