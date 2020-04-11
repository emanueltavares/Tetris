using Tetris.Factories;
using Tetris.Models;
using Tetris.Utils;
using Tetris.Views;
using UnityEngine;

namespace Tetris.Controllers
{
    public class HoldPieceController : MonoBehaviour, IHoldController
    {
        // Serialize Fields
        [SerializeField] private Transform _swapBoardTransform;
        [SerializeField] private BlocksScriptableObject _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private float _blockScale = 1f;

        private IBoardView _boardView;
        private IBoardModel _boardModel;
        private IBoardFactory _boardFactory;
        private int _holdPieceType = TetrominoUtils.NoPiece;

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

        protected virtual void OnEnable()
        {
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
    }

    public interface IHoldController
    {
        int Hold(int holdPieceType);
    }
}