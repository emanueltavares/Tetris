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
        [SerializeField] private InputController _inputController;
        [SerializeField] private float _startGravityInterval = 1f;
        [SerializeField] private float _startHoldDirectionMaxTime = 0.1f;

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
        private float _applyGravityInterval;
        private Material[] _blockMaterials;

        // Constants
        private const int StartLine = 0;
        private const int StartColumn = 3;

        protected virtual void OnEnable()
        {
            _applyGravityInterval = _startGravityInterval;

            // Initialize block materials
            _blockMaterials = new Material[8] { _noBlockMat, _lightBlueBlockMat, _darkBlueBlockMat, _orangeBlockMat, _yellowBlockMat, _greenBlockMat, _purpleBlockMat, _redBlockMat };

            // Initialize board model and board view
            (_boardModel, _boardView) = _boardFactory.GetBoard(_blockMaterials);

            // Start the game
            StartCoroutine(SpawnTetromino());
        }

        private IEnumerator SpawnTetromino()
        {
            // Create the first tetromino
            _currentTetromino = _tetrominosFactory.GetPiece(StartLine, StartColumn, Constants.IPieceType);

            // Show the tetromino
            DrawTetromino();

            // Update
            yield return StartCoroutine(MoveTetromino());
        }

        private IEnumerator MoveTetromino()
        {
            while (enabled) // while current tetromino is not locked
            {
                // Control tetromino
                yield return StartCoroutine(ControlTetromino());
                
                ClearTetromino();

                // Apply gravity
                _currentTetromino.CurrentLine += 1;

                DrawTetromino();
            }
        }

        private IEnumerator ControlTetromino()
        {
            // Update hold direction max time
            _inputController.HoldDirectionMaxTime = _startHoldDirectionMaxTime;

            for (float elapsedTime = 0f; elapsedTime < _applyGravityInterval; elapsedTime = Mathf.MoveTowards(elapsedTime, _applyGravityInterval, Time.deltaTime))
            {
                ClearTetromino();

                if (_inputController.CanMoveRight)
                {
                    _currentTetromino.CurrentColumn += 1;
                }

                if (_inputController.CanMoveLeft)
                {
                    _currentTetromino.CurrentColumn -= 1;
                }

                DrawTetromino();

                yield return null;
            }
        }

        private void ClearTetromino()
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

        private void DrawTetromino()
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