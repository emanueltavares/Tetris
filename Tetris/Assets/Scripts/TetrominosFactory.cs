using System.Collections.Generic;
using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Factories
{
    public class TetrominosFactory : MonoBehaviour, ITetrominosFactory
    {        
        // Serialize Fields
        [SerializeField] private Transform[] _nextPiecesParents = new Transform[0];
        [SerializeField] private BlocksScriptableObject _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private float _blockScale = 1f;

        // Constants
        private static readonly List<int> PieceTypes = new List<int>()
        {
            Constants.IPieceType,
            Constants.JPieceType,
            Constants.LPieceType,
            Constants.SPieceType,
            Constants.OPieceType,
            Constants.TPieceType,
            Constants.ZPieceType
        };

        // Private 
        private System.Random _random = new System.Random();
        private IBoardFactory _boardFactory;
        private (IBoardModel, IBoardView)[] _nextPieces = new (IBoardModel, IBoardView)[0];

        // Properties
        public List<int> NextPieceTypes { get; private set; } = new List<int>();

        protected virtual void OnEnable()
        {
            int pieceTypesIndex = _random.Next(PieceTypes.Count - 1);
            int randomPieceType = PieceTypes[pieceTypesIndex];
            NextPieceTypes.Add(randomPieceType);

            // Add other random pieces
            for (int i = 1; i < _nextPiecesParents.Length; i++)
            {
                // remove previous piece type
                List<int> pieceTypes = new List<int>(PieceTypes);
                pieceTypes.Remove(randomPieceType);

                pieceTypesIndex = _random.Next(pieceTypes.Count - 1);
                randomPieceType = pieceTypes[pieceTypesIndex];
                NextPieceTypes.Add(randomPieceType);
            }

            CreateBoards();
            DrawNextPieces();
        }

        public ITetrominoModel GetNextPiece(int startLine, int startColumn)
        {
            int pieceType = NextPieceTypes[0];

            // get next piece type
            List<int> pieceTypes = new List<int>(PieceTypes);
            pieceTypes.Remove(NextPieceTypes[NextPieceTypes.Count - 1]);
            int nextPieceType = pieceTypes[_random.Next(pieceTypes.Count - 1)];

            NextPieceTypes.RemoveAt(0);
            NextPieceTypes.Add(nextPieceType);

            DrawNextPieces();

            // next tetromino
            ITetrominoModel tetromino = GetTetromino(pieceType);
            tetromino.CurrentLine = startLine;
            tetromino.CurrentColumn = startColumn;
            return tetromino;
        }

        private ITetrominoModel GetTetromino(int pieceType)
        {
            switch (pieceType)
            {
                case Constants.OPieceType:
                    return new LetterOTetrominoModel();
                case Constants.LPieceType:
                    return new LetterLTetrominoModel();
                case Constants.SPieceType:
                    return new LetterSTetrominoModel();
                case Constants.TPieceType:
                    return new LetterTTetrominoModel();
                case Constants.ZPieceType:
                    return new LetterZTetrominoModel();
                case Constants.JPieceType:
                    return new LetterJTetrominoModel();
                case Constants.IPieceType:
                    return new LetterITetrominoModel();
                default:
                    return new LetterITetrominoModel();
            }
        }

        private void CreateBoards()
        {
            if (_boardFactory == null)
            {
                _boardFactory = GetComponent<IBoardFactory>();
            }

            _nextPieces = new (IBoardModel, IBoardView)[_nextPiecesParents.Length];
            for (int i = 0; i < _nextPiecesParents.Length; i++)
            {
                Transform nextPiecesParent = _nextPiecesParents[i];
                (IBoardModel, IBoardView) board = _boardFactory.GetBoard(_blockPrefab,
                                                                         _blocks.Materials,
                                                                         _blockScale,
                                                                         nextPiecesParent,
                                                                         Constants.MaxNumLinesPreview,
                                                                         Constants.MaxNumColumnsPreview);
                _nextPieces[i] = board;
            }
        }

        private void DrawNextPieces()
        {
            for (int i = 0; i < NextPieceTypes.Count; i++)
            {
                int nextPieceType = NextPieceTypes[i];

                IBoardModel boardModel = _nextPieces[i].Item1;
                IBoardView boardView = _nextPieces[i].Item2;

                ITetrominoModel tetromino = GetTetromino(nextPieceType);

                for (int line = 0; line < Constants.MaxNumLinesPreview; line++)
                {
                    for (int column = 0; column < Constants.MaxNumColumnsPreview; column++)
                    {
                        int blockType = tetromino.BlocksPreview[line, column];
                        boardModel.Blocks[line, column] = blockType;
                    }
                }

                // Update view after showing tetromino
                boardView.UpdateView(boardModel, _blocks.Materials);
            }
        }
    }

    public interface ITetrominosFactory
    {
        List<int> NextPieceTypes { get; }
        ITetrominoModel GetNextPiece(int startLine, int startColumn);
    }
}