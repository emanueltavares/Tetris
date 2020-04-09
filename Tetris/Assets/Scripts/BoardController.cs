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
        [SerializeField] private float _applyGravityInterval = 1f;
        [Range(0f, 1f)] [SerializeField] private float _dropSoftGravityMultiplier = 0.1f;

        [Header("Tetrominos Colors")]
        [SerializeField] private Material _noBlockMat;                                                     // GREY is the color of the empty block
        [SerializeField] private Material _lightBlueBlockMat;                                              // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlueBlockMat;                                               // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orangeBlockMat;                                                 // ORANGE is the color of the L piece
        [SerializeField] private Material _yellowBlockMat;                                                 // YELLOW is the color of the O piece
        [SerializeField] private Material _greenBlockMat;                                                  // GREEN is the color of the S piece
        [SerializeField] private Material _purpleBlockMat;                                                 // PURPLE is the color of the T piece
        [SerializeField] private Material _redBlockMat;                                                    // RED is the color of the Z piece

        // Private Fields
        private IBoardFactory _boardFactory;
        private IInputController _inputController;
        private IBoardModel _boardModel;
        private IBoardView _boardView;
        private ITetrominosFactory _tetrominosFactory = new TetrominosFactory();
        private ITetrominoModel _currentTetromino;
        private Material[] _blockMaterials;

        private System.Random _random = new System.Random();

        protected virtual void OnEnable()
        {
            // Initialize block materials
            _blockMaterials = new Material[8] { _noBlockMat, _lightBlueBlockMat, _darkBlueBlockMat, _orangeBlockMat, _yellowBlockMat, _greenBlockMat, _purpleBlockMat, _redBlockMat };

            // Initialize board model and board view
            if (_boardFactory == null)
            {
                _boardFactory = GetComponent<IBoardFactory>();
            }
            (_boardModel, _boardView) = _boardFactory.GetBoard(_blockMaterials);

            // Initialized hold input max time
            if (_inputController == null)
            {
                _inputController = GetComponent<IInputController>();
            }
            _inputController.HoldInputMaxTime = _applyGravityInterval / _boardModel.NumColumns;

            // Start the game
            StartCoroutine(SpawnTetromino());
        }

        private IEnumerator SpawnTetromino()
        {
            // Create the first tetromino
            
            int randomPieceType = _random.Next(Constants.IPieceType, Constants.ZPieceType + 1);
            int startLine = randomPieceType == Constants.OPieceType ? 0 : -1;
            int startColumn = 3;
            _currentTetromino = _tetrominosFactory.GetPiece(startLine, startColumn, randomPieceType);

            if (ValidateTetrominoPosition(_currentTetromino))
            {
                // Show the tetromino
                DrawTetromino(_currentTetromino);
                yield return StartCoroutine(MoveTetromino());
            }
            else
            {
                // Show the tetromino
                DrawTetromino(_currentTetromino);

                // Game Over
            }

        }

        private IEnumerator MoveTetromino()
        {
            bool isTetrominoLocked = false;
            bool activateDropHard = false;

            while (!isTetrominoLocked) // while current tetromino is not locked
            {
                // if drop hard was activated, we skip tetromino control
                if (!activateDropHard)
                {
                    // Control tetromino
                    yield return StartCoroutine(ControlTetromino());
                }

                // Check if player has clicked the drop hard button while controlling the tetromino
                if (_inputController.DropHard)
                {
                    activateDropHard = true;
                }

                ClearTetromino(_currentTetromino);

                // Apply gravity
                Debug.Log("apply gravity");
                _currentTetromino.CurrentLine += 1;

                if (!ValidateTetrominoPosition(_currentTetromino))
                {
                    // rollback and lock the tetromino
                    _currentTetromino.CurrentLine -= 1;
                    isTetrominoLocked = true;
                }

                DrawTetromino(_currentTetromino);
            }

            // Skip a frame so we can reset our input
            yield return null;

            yield return StartCoroutine(SpawnTetromino());
        }

        private IEnumerator ControlTetromino()
        {
            float applyGravityInterval = _applyGravityInterval;
            for (float elapsedTime = 0f; elapsedTime < applyGravityInterval; elapsedTime = Mathf.MoveTowards(elapsedTime, applyGravityInterval, Time.deltaTime))
            {
                // if drop hard is activated, we skip control
                if (_inputController.DropHard)
                {
                    yield break;
                }

                if (_inputController.DropSoft)
                {
                    applyGravityInterval = _applyGravityInterval * _dropSoftGravityMultiplier;
                }

                ClearTetromino(_currentTetromino);

                if (_inputController.MoveLeft)
                {
                    _currentTetromino.CurrentColumn -= 1;

                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous move
                        _currentTetromino.CurrentColumn += 1;
                    }
                }

                if (_inputController.MoveRight)
                {
                    _currentTetromino.CurrentColumn += 1;
                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous move
                        _currentTetromino.CurrentColumn -= 1;
                    }
                }

                if (_inputController.RotateClockwise)
                {
                    _currentTetromino.RotateClockwise();
                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous rotation
                        _currentTetromino.RotateCounterClockwise();
                    }

                }

                if (_inputController.RotateCounterClockwise)
                {
                    _currentTetromino.RotateCounterClockwise();
                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous rotation
                        _currentTetromino.RotateClockwise();
                    }
                }

                DrawTetromino(_currentTetromino);

                yield return null;
            }
        }

        private void ClearTetromino(ITetrominoModel tetromino)
        {
            for (int blockLine = 0; blockLine < tetromino.NumLines; blockLine++)
            {
                for (int blockColumn = 0; blockColumn < tetromino.NumColumns; blockColumn++)
                {
                    // Converts the block line and column to board line and column
                    int boardLine = tetromino.CurrentLine + blockLine;
                    int boardColumn = tetromino.CurrentColumn + blockColumn;

                    if (boardLine >= 0 && boardLine < _boardModel.NumLines &&
                        boardColumn >= 0 && boardColumn < _boardModel.NumColumns)
                    {
                        if (_currentTetromino.Blocks[blockLine, blockColumn] != Constants.NoPiece)
                        {
                            _boardModel.Blocks[boardLine, boardColumn] = Constants.NoPiece;
                        }
                    }
                }
            }

            // Update view after hiding tetromino
            int endLine = tetromino.CurrentLine + tetromino.NumLines;
            int endColumn = tetromino.CurrentColumn + tetromino.NumColumns;
            _boardView.UpdateView(_boardModel, tetromino.CurrentLine, tetromino.CurrentColumn, endLine, endColumn, _blockMaterials);
        }

        private bool ValidateTetrominoPosition(ITetrominoModel tetromino)
        {
            for (int blockLine = 0; blockLine < tetromino.NumLines; blockLine++)
            {
                for (int blockColumn = 0; blockColumn < tetromino.NumColumns; blockColumn++)
                {
                    // Converts the block line and column to board line and column
                    int boardLine = tetromino.CurrentLine + blockLine;
                    int boardColumn = tetromino.CurrentColumn + blockColumn;

                    if (boardLine >= 0 && boardLine < _boardModel.NumLines && boardColumn >= 0 && boardColumn < _boardModel.NumColumns)
                    {
                        if (tetromino.Blocks[blockLine, blockColumn] != Constants.NoPiece && _boardModel.Blocks[boardLine, boardColumn] != Constants.NoPiece)
                        {
                            return false;
                        }
                    }
                    else if (tetromino.Blocks[blockLine, blockColumn] != Constants.NoPiece)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void DrawTetromino(ITetrominoModel tetromino)
        {
            for (int blockLine = 0; blockLine < tetromino.NumLines; blockLine++)
            {
                for (int blockColumn = 0; blockColumn < tetromino.NumColumns; blockColumn++)
                {
                    int blockType = tetromino.Blocks[blockLine, blockColumn];
                    if (blockType != Constants.NoPiece)
                    {
                        // Converts the block line and column to board line and column
                        int boardLine = tetromino.CurrentLine + blockLine;
                        int boardColumn = tetromino.CurrentColumn + blockColumn;

                        if (boardLine >= 0 && boardLine < _boardModel.NumLines && boardColumn >= 0 && boardColumn < _boardModel.NumColumns)
                        {
                            _boardModel.Blocks[boardLine, boardColumn] = blockType;
                        }
                    }
                }
            }

            // Update view after showing tetromino
            int endLine = tetromino.CurrentLine + tetromino.NumLines;
            int endColumn = tetromino.CurrentColumn + tetromino.NumColumns;
            _boardView.UpdateView(_boardModel, tetromino.CurrentLine, tetromino.CurrentColumn, endLine, endColumn, _blockMaterials);
        }
    }
}