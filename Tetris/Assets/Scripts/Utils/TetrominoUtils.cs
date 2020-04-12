using Tetris.Models;

namespace Tetris.Utils
{
    public static class TetrominoUtils
    {
        public const int GhostPiece = -1;
        public const int NoPiece = 0;
        public const int IPieceType = 1;
        public const int JPieceType = 2;
        public const int LPieceType = 3;
        public const int SPieceType = 4;
        public const int OPieceType = 5;
        public const int TPieceType = 6;
        public const int ZPieceType = 7;

        public const int MaxNumLinesPreview = 2;
        public const int MaxNumColumnsPreview = 4;

        public const string StartScene = "Start";
        public const string GameScene = "Game";
        public const string CreditsScene = "Credits";

        private static readonly System.Random _random = new System.Random();

        public static int GetRandomValue(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public static ITetrominoModel GetTetromino(int pieceType)
        {
            switch (pieceType)
            {
                case OPieceType:
                    return new LetterOTetrominoModel();
                case LPieceType:
                    return new LetterLTetrominoModel();
                case SPieceType:
                    return new LetterSTetrominoModel();
                case TPieceType:
                    return new LetterTTetrominoModel();
                case ZPieceType:
                    return new LetterZTetrominoModel();
                case JPieceType:
                    return new LetterJTetrominoModel();
                default:
                    return new LetterITetrominoModel();
            }
        }

        public static ITetrominoModel CloneTetromino(ITetrominoModel tetrominoModel)
        {
            ITetrominoModel clone = GetTetromino(tetrominoModel.PieceType);
            //clone.CurrentLine = tetrominoModel.CurrentLine;
            //clone.CurrentColumn = tetrominoModel.CurrentColumn;
            clone.Rotation = tetrominoModel.Rotation;
            return clone;
        }
    }
}