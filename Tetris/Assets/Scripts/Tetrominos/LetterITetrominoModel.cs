namespace Tetris.Models
{
    public class LetterITetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterINumLines = 4;
        private const int LetterINumColumns = 4;
        //private const int MaxRotations = 2;

        // Readonlies
        private static readonly int[,] Vertical = new int[LetterINumLines, LetterINumColumns]
            {
                {0, 0, Utils.TetrominoUtils.IPieceType, 0 },
                {0, 0, Utils.TetrominoUtils.IPieceType, 0 },
                {0, 0, Utils.TetrominoUtils.IPieceType, 0 },
                {0, 0, Utils.TetrominoUtils.IPieceType, 0 }
            };
        private static readonly int[,] Horizontal = new int[LetterINumLines, LetterINumColumns]
            {
                {0, 0, 0, 0 },
                { Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType },
                {0, 0, 0, 0 },
                {0, 0, 0, 0 }
            };

        // Properties
        public int MaxRotations => 2;
        public int PieceType => Utils.TetrominoUtils.IPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterINumLines;
        public int NumColumns => LetterINumColumns;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview] 
        {
            { Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType, Utils.TetrominoUtils.IPieceType },
            {0, 0, 0, 0 }
        };
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

        private int _rotation = 0;

        public LetterITetrominoModel()
        {
            Blocks = Horizontal;
        }
    }
}