namespace Application.Models
{
    public class LetterSTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterSNumLines = 3;
        private const int LetterSNumColumns = 3;

        // Readonlies
        private static readonly int[,] Horizontal = new int[LetterSNumLines, LetterSNumColumns]
            {
                { 0,                    0,                    0                    },
                { 0,                    Utils.TetrominoUtils.SPieceType, Utils.TetrominoUtils.SPieceType },
                { Utils.TetrominoUtils.SPieceType, Utils.TetrominoUtils.SPieceType, 0                    }
            };
        private static readonly int[,] Vertical = new int[LetterSNumLines, LetterSNumColumns]
            {
                { Utils.TetrominoUtils.SPieceType,  0,                   0                    },
                { Utils.TetrominoUtils.SPieceType, Utils.TetrominoUtils.SPieceType, 0                    },
                { 0,                    Utils.TetrominoUtils.SPieceType, 0                    }
            };


        // Properties
        public int PieceType => Utils.TetrominoUtils.SPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterSNumLines;
        public int NumColumns => LetterSNumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
            { 0,    0,                    Utils.TetrominoUtils.SPieceType, Utils.TetrominoUtils.SPieceType },
            { 0,    Utils.TetrominoUtils.SPieceType, Utils.TetrominoUtils.SPieceType, 0                    }
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

        public LetterSTetrominoModel()
        {
            Blocks = Horizontal;
        }

        public void RotateClockwise()
        {
            _rotation += 1;
            _rotation = MathExt.Mod(_rotation, MaxRotations);
            SetRotation();
        }

        public void RotateCounterClockwise()
        {
            _rotation -= 1;
            _rotation = MathExt.Mod(_rotation, MaxRotations);

            SetRotation();
        }

        private void SetRotation()
        {
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
}