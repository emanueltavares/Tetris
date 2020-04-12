using Application.Factories;
using Application.Models;
using Application.Utils;
using Application.Views;
using UnityEngine;

namespace Application.Controllers
{
    public class HoldPieceController : MonoBehaviour, IHoldController
    {
        // Serialize Fields
        [SerializeField] private Transform _swapBoardTransform;
        [SerializeField] private Theme _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private float _blockScale = 1f;

        private IBoardView _boardView;
        private IBoardModel _boardModel;
        private IBoardFactory _boardFactory;
        private IBoardController _boardController;
        private int _holdPieceType = TetrominoUtils.NoPiece;
        private bool _isHidingHoldPiece = false;

        protected virtual void OnEnable()
        {
            if (_boardController == null)
            {
                _boardController = GetComponent<IBoardController>();
            }

            if (_boardFactory == null)
            {
                _boardFactory = GetComponent<IBoardFactory>();
            }

            (_boardModel, _boardView) = _boardFactory.GetBoard(_blockPrefab, _blocks, _blockScale, _swapBoardTransform, Utils.TetrominoUtils.MaxNumLinesPreview, Utils.TetrominoUtils.MaxNumColumnsPreview);

            // Draw Board
            for (int line = 0; line < _boardModel.NumLines; line++)
            {
                for (int column = 0; column < _boardModel.NumColumns; column++)
                {
                    _boardModel.Blocks[line, column] = TetrominoUtils.NoPiece;
                }
            }

            _boardView.UpdateView(_boardModel, _blocks);
        }

        protected virtual void Update()
        {
            if (_boardController.IsPaused != _isHidingHoldPiece)
            {
                if (!_isHidingHoldPiece)
                {
                    _boardView.HideView(_blocks);
                }
                else 
                {
                    _boardView.UpdateView(_boardModel, _blocks);
                }

                _isHidingHoldPiece = _boardController.IsPaused;
            }
        }

        public int Hold(int pieceType)
        {
            int previousHoldPieceType = _holdPieceType;
            _holdPieceType = pieceType;

            ITetrominoModel tetrominoModel = TetrominoUtils.GetTetromino(_holdPieceType);
            DrawHoldPiece(_boardModel, _boardView, tetrominoModel);

            return previousHoldPieceType;
        }

        private void DrawHoldPiece(IBoardModel boardModel, IBoardView boardView, ITetrominoModel tetromino)
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

    public interface IHoldController
    {
        int Hold(int holdPieceType);
    }
}