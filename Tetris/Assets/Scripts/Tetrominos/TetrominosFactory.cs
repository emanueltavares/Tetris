using System.Collections.Generic;
using Application.Controllers;
using Application.Models;
using Application.Views;
using UnityEngine;

namespace Application.Factories
{
    public class TetrominosFactory : MonoBehaviour, ITetrominosFactory
    {        
        // Serialize Fields
        [SerializeField] private Transform[] _nextPiecesParents = new Transform[0];
        [SerializeField] private Theme _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private float _blockScale = 1f;

        // Constants
        private static readonly List<int> PieceTypes = new List<int>()
        {
            Utils.TetrominoUtils.IPieceType,
            Utils.TetrominoUtils.JPieceType,
            Utils.TetrominoUtils.LPieceType,
            Utils.TetrominoUtils.SPieceType,
            Utils.TetrominoUtils.OPieceType,
            Utils.TetrominoUtils.TPieceType,
            Utils.TetrominoUtils.ZPieceType
        };

        // Private 
        private IBoardFactory _boardFactory;
        private IBoardController _boardController;
        private (IBoardModel, IBoardView)[] _nextPieces = new (IBoardModel, IBoardView)[0];
        private bool _isInitialized;
        private bool _isHidingNextPieces = false;

        // Properties
        public List<int> NextPieceTypes { get; private set; } = new List<int>();

        protected virtual void OnEnable()
        {
            if (_boardController == null)
            {
                _boardController = GetComponent<IBoardController>();
            }

            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }
        }

        protected virtual void Update()
        {
            if (_boardController.IsPaused != _isHidingNextPieces)
            {
                for (int i = 0; i < NextPieceTypes.Count; i++)
                {
                    IBoardModel boardModel = _nextPieces[i].Item1;
                    IBoardView boardView = _nextPieces[i].Item2;

                    if (!_isHidingNextPieces)
                    {
                        boardView.HideView(_blocks);
                    }
                    else
                    {
                        boardView.HideView(_blocks);
                        boardView.UpdateView(boardModel, _blocks);
                    }

                }

                _isHidingNextPieces = _boardController.IsPaused;
            }
        }

        private void Initialize()
        {
            int pieceTypesIndex = Utils.TetrominoUtils.GetRandomValue(PieceTypes.Count);
            int randomPieceType = PieceTypes[pieceTypesIndex];
            NextPieceTypes.Add(randomPieceType);

            // Add other random pieces
            for (int i = 1; i < _nextPiecesParents.Length; i++)
            {
                // remove previous piece type
                List<int> pieceTypes = new List<int>(PieceTypes);
                pieceTypes.Remove(randomPieceType);

                pieceTypesIndex = Utils.TetrominoUtils.GetRandomValue(pieceTypes.Count);
                randomPieceType = pieceTypes[pieceTypesIndex];
                NextPieceTypes.Add(randomPieceType);
            }

            CreateBoards();
        }

        public ITetrominoModel GetNextPiece(int startLine, int startColumn)
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }

            int pieceType = NextPieceTypes[0];
            
            // get next piece type
            List<int> pieceTypes = new List<int>(PieceTypes);
            pieceTypes.Remove(NextPieceTypes[NextPieceTypes.Count - 1]);
            int nextPieceType = pieceTypes[Utils.TetrominoUtils.GetRandomValue(pieceTypes.Count)];

            NextPieceTypes.RemoveAt(0);
            NextPieceTypes.Add(nextPieceType);

            DrawNextPieces();

            return GetNextPiece(pieceType, startLine, startColumn);
        }

        public ITetrominoModel GetNextPiece(int pieceType, int startLine, int startColumn)
        {
            // next tetromino
            ITetrominoModel tetromino = Utils.TetrominoUtils.GetTetromino(pieceType);
            tetromino.CurrentLine = startLine;
            tetromino.CurrentColumn = startColumn;
            return tetromino;
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
                                                                         _blocks,
                                                                         _blockScale,
                                                                         nextPiecesParent,
                                                                         Utils.TetrominoUtils.MaxNumLinesPreview,
                                                                         Utils.TetrominoUtils.MaxNumColumnsPreview);
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

                ITetrominoModel tetromino = Utils.TetrominoUtils.GetTetromino(nextPieceType);
                DrawTetromino(boardModel, boardView, tetromino);
            }
        }

        private void DrawTetromino(IBoardModel boardModel, IBoardView boardView, ITetrominoModel tetromino)
        {
            for (int line = 0; line < boardModel.NumLines; line++)
            {
                for (int column = 0; column < boardModel.NumColumns; column++)
                {
                    int blockType = tetromino.BlocksPreview[line, column];
                    boardModel.Blocks[line, column] = blockType;
                }
            }

            // Update view after showing tetromino
            boardView.UpdateView(boardModel, _blocks);
        }        
    }

    public interface ITetrominosFactory
    {
        List<int> NextPieceTypes { get; }
        ITetrominoModel GetNextPiece(int startLine, int startColumn);
        ITetrominoModel GetNextPiece(int pieceType, int startLine, int startColumn);
    }
}