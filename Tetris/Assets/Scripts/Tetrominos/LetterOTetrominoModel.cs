namespace Application.Models
{
    public class LetterOTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterONumLines = 3;
        private const int LetterONumColums = 2;

        private static readonly int[,] Vertical = new int[LetterONumLines, LetterONumColums]
            {
                {0,                    0 },
                { Utils.TetrominoUtils.OPieceType, Utils.TetrominoUtils.OPieceType},
                { Utils.TetrominoUtils.OPieceType, Utils.TetrominoUtils.OPieceType}
            };

        // Properties
        public int PieceType => Utils.TetrominoUtils.OPieceType;
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterONumLines;
        public int NumColumns => LetterONumColums;
        public int[,] Blocks { get; private set; }
        public int[,] BlocksPreview => new int[Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview]
        {
            {0, Utils.TetrominoUtils.OPieceType, Utils.TetrominoUtils.OPieceType, 0 },
            {0, Utils.TetrominoUtils.OPieceType, Utils.TetrominoUtils.OPieceType, 0 },
        };
        public int MaxRotations => 1;
        public int Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _rotation = MathExt.Mod(_rotation, MaxRotations);
            }
        }

        // Private
        private int _rotation = 0;

        public LetterOTetrominoModel()
        {
            Blocks = Vertical;
        }
    }
}