using System.Collections;
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

        [Header("Tetrominos Colors")]
        [SerializeField] private Material _noBlockMat;         // GREY is the color of the empty block
        [SerializeField] private Material _lightBlueBlockMat;  // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlueBlockMat;   // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orangeBlockMat;     // ORANGE is the color of the L piece
        [SerializeField] private Material _yellowBlockMat;     // YELLOW is the color of the O piece
        [SerializeField] private Material _greenBlockMat;      // GREEN is the color of the S piece
        [SerializeField] private Material _purpleBlockMat;     // PURPLE is the color of the T piece
        [SerializeField] private Material _redBlockMat;        // RED is the color of the Z piece

        // Private Fields
        private IBoardModel _boardModel;
        private IBoardView _boardView;
        private ITetrominosFactory _tetrominosFactory = new TetrominosFactory();
        private ITetrominoModel _currentTetromino;
        private Material[] _blockMaterials;

        // Constants
        private const int StartLine = 0;
        private const int StartColumn = 3;

        protected virtual void OnEnable()
        {
            // Initialize block materials
            _blockMaterials = new Material[8] { _noBlockMat, _lightBlueBlockMat, _darkBlueBlockMat, _orangeBlockMat, _yellowBlockMat, _greenBlockMat, _purpleBlockMat, _redBlockMat };

            // Initialize board model and board view
            (_boardModel, _boardView) = _boardFactory.GetBoard(_blockMaterials);

            // Create the first tetromino
            _currentTetromino = _tetrominosFactory.GetPiece(StartLine, StartColumn, Constants.IPieceType);

            ShowTetromino();

            StartCoroutine(UpdateBoard());
        }

        private IEnumerator UpdateBoard()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(1f);

                HideTetromino(); 

                _currentTetromino.CurrentLine += 1;

                ShowTetromino();
            }
        }

        private void HideTetromino()
        {
            for (int line = 0; line < _currentTetromino.NumLines; line++)
            {
                for (int col = 0; col < _currentTetromino.NumColumns; col++)
                {
                    _boardModel.Blocks[_currentTetromino.CurrentLine + line, _currentTetromino.CurrentColumn + col] = Constants.NoPiece;
                }
            }

            // Update view after hiding tetromino
            int endLine = _currentTetromino.CurrentLine + _currentTetromino.NumLines;
            int endColumn = _currentTetromino.CurrentColumn + _currentTetromino.NumColumns;
            _boardView.UpdateView(_boardModel, _currentTetromino.CurrentLine, _currentTetromino.CurrentColumn, endLine, endColumn, _blockMaterials);
        }

        private void ShowTetromino()
        {
            for (int line = 0; line < _currentTetromino.NumLines; line++)
            {
                for (int col = 0; col < _currentTetromino.NumColumns; col++)
                {
                    int blockType = _currentTetromino.Blocks[line, col];
                    _boardModel.Blocks[_currentTetromino.CurrentLine + line, _currentTetromino.CurrentColumn + col] = blockType;
                }
            }

            // Update view after showing tetromino
            int endLine = _currentTetromino.CurrentLine + _currentTetromino.NumLines;
            int endColumn = _currentTetromino.CurrentColumn + _currentTetromino.NumColumns;
            _boardView.UpdateView(_boardModel, _currentTetromino.CurrentLine, _currentTetromino.CurrentColumn, endLine, endColumn, _blockMaterials);
        }
    }
}