using Tetris.Factories;
using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Controllers
{
    public partial class BoardController : MonoBehaviour
    {
        // Serialized Fields
        [SerializeField] private BoardFactory _boardFactory;

        // Private Fields
        private IBoardModel _boardModel;
        private IBoardView _boardView;
        private ITetrominosFactory _tetrominosFactory = new TetrominosFactory();
        private ITetrominoModel _currentTetromino;

        protected virtual void OnEnable()
        {
            Create();

            _currentTetromino = _tetrominosFactory.GetPiece(Constants.IPieceType);
            //ApplyTetrominoToModel(_currentTetromino);
        }

        private void Create()
        {
            (_boardModel, _boardView) = _boardFactory.GetBoard();
        }

        //private void ApplyTetrominoToModel(ITetrominoModel tetromino)
        //{

        //}
    }
}