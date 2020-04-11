﻿using System.Collections;
using System.Collections.Generic;
using Tetris.Factories;
using Tetris.Models;
using Tetris.Utils;
using Tetris.Views;
using UnityEngine;

namespace Tetris.Controllers
{
    public partial class BoardController : MonoBehaviour, IBoardController
    {
        // Serialized Fields        
        [Header("Animation")]
        [SerializeField] private float _clearLineAnimationTime = 1f;
        [Range(0f, 1f)] [SerializeField] private float _clearLineMultiplier = 0.125f;

        [Header("Block Creation")]
        [SerializeField] private BlocksScriptableObject _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private Transform _blocksParent;
        [SerializeField] private int _maxNumLines;                                              // max number of lines of our board
        [SerializeField] private int _maxNumColumns;                                            // max number of columns of our board
        [SerializeField] private float _blockScale = 1f;

        // Private Fields
        private IBoardFactory _boardFactory;
        private IHoldController _holdController;
        private IInputController _inputController;
        private ILevelController _levelController;
        private ITetrominosFactory _tetrominosFactory;
        private ITetrominoModel _currentTetromino;
        private ITetrominoModel _ghostTetromino;
        private bool _isHoldPiece = false;

        // Properties
        public IBoardModel BoardModel { get; private set; }
        public IBoardView BoardView { get; private set; }

        protected virtual void OnEnable()
        {
            // Initialize board model and board view
            if (_boardFactory == null)
            {
                _boardFactory = GetComponent<IBoardFactory>();
            }
            (BoardModel, BoardView) = _boardFactory.GetBoard(_blockPrefab, _blocks, _blockScale, _blocksParent, _maxNumLines, _maxNumColumns);

            // Initialized hold input max time
            if (_inputController == null)
            {
                _inputController = GetComponent<IInputController>();
            }

            if (_levelController == null)
            {
                _levelController = GetComponent<ILevelController>();
                _levelController.AddClearedLines(0);
            }

            if (_holdController == null)
            {
                _holdController = GetComponent<IHoldController>();
            }

            // Start the game
            StartCoroutine(SpawnTetromino(false));
        }

        private IEnumerator SpawnTetromino(bool useHoldPiece)
        {
            // Create the first tetromino
            if (_tetrominosFactory == null)
            {
                _tetrominosFactory = GetComponent<ITetrominosFactory>();
            }

            if (!useHoldPiece)
            {
                _currentTetromino = _tetrominosFactory.GetNextPiece(0, 3);
                _isHoldPiece = false;
            }
            else
            {
                int previousHoldPieceType = _holdController.Hold(_currentTetromino.PieceType);
                if (previousHoldPieceType > Utils.TetrominoUtils.NoPiece)
                {
                    _currentTetromino = _tetrominosFactory.GetNextPiece(previousHoldPieceType, 0, 3);
                }
                else
                {
                    _currentTetromino = _tetrominosFactory.GetNextPiece(0, 3);
                }

                _isHoldPiece = true;
            }

            ClearTetromino(_currentTetromino, _ghostTetromino);

            if (ValidateTetrominoPosition(_currentTetromino))
            {
                // Show the tetromino
                _ghostTetromino = Utils.TetrominoUtils.CloneTetromino(_currentTetromino);                
                DrawTetromino(_currentTetromino, _ghostTetromino);

                yield return StartCoroutine(MoveTetromino());
            }
            else
            {
                // Show the tetromino                
                DrawTetromino(_currentTetromino, null);

                // Game Over
            }

        }

        private IEnumerator MoveTetromino()
        {
            bool isTetrominoLocked = false;
            bool activateDropHard = false;
            bool useHoldPiece = false;

            while (!isTetrominoLocked && !useHoldPiece) // while current tetromino is not locked or while player has not activated hold piece
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

                ClearTetromino(_currentTetromino, _ghostTetromino);

                if (_inputController.HoldPiece && !_isHoldPiece)
                {
                    useHoldPiece = true;
                }
                else
                {
                    // Apply gravity
                    _currentTetromino.CurrentLine += 1;

                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // rollback and lock the tetromino
                        _currentTetromino.CurrentLine -= 1;
                        isTetrominoLocked = true;
                    }

                    DrawTetromino(_currentTetromino, _ghostTetromino);
                }
            }

            if (!useHoldPiece)
            {
                List<int> clearedLines = GetClearedLines();
                if (clearedLines.Count > 0)
                {
                    yield return StartCoroutine(AnimateClearedLines(clearedLines));
                    RemoveClearedLines(clearedLines);
                    _levelController.AddClearedLines(clearedLines.Count);
                }
                else
                {
                    // Add a frame to refresh input
                    yield return null;
                }
            }

            yield return StartCoroutine(SpawnTetromino(useHoldPiece));
        }

        private List<int> GetClearedLines()
        {
            List<int> clearedLines = new List<int>();
            for (int line = BoardModel.NumLines - 1; line >= 0; line--)
            {
                bool hasEmptyBlock = false;
                for (int column = 0; column < BoardModel.NumColumns; column++)
                {
                    if (BoardModel.Blocks[line, column] <= Utils.TetrominoUtils.NoPiece)
                    {
                        hasEmptyBlock = true;
                        break;
                    }
                }

                if (!hasEmptyBlock)
                {
                    clearedLines.Add(line);
                }
            }

            return clearedLines;
        }

        private void RemoveClearedLines(List<int> clearedLines)
        {
            List<int[]> listBlocks = new List<int[]>();
            for (int line = 0; line < BoardModel.NumLines; line++)
            {
                int[] columns = new int[BoardModel.NumColumns];
                for (int column = 0; column < BoardModel.NumColumns; column++)
                {
                    columns[column] = BoardModel.Blocks[line, column];
                }
                listBlocks.Add(columns);
            }

            int[] emptyLine = new int[BoardModel.NumColumns];

            for (int i = 0; i < clearedLines.Count; i++)
            {
                int clearedLine = clearedLines[i] + i;
                listBlocks.RemoveAt(clearedLine);
                listBlocks.Insert(0, emptyLine);
            }

            for (int line = 0; line < BoardModel.NumLines; line++)
            {
                for (int column = 0; column < BoardModel.NumColumns; column++)
                {
                    BoardModel.Blocks[line, column] = Mathf.Max(listBlocks[line][column], TetrominoUtils.NoPiece);
                }
            }

            BoardView.UpdateView(BoardModel, _blocks);
        }

        private IEnumerator AnimateClearedLines(List<int> clearedLines)
        {
            // blink animation
            bool blink = true;
            for (float elapsedTime = 0; elapsedTime < _clearLineAnimationTime; elapsedTime = Mathf.MoveTowards(elapsedTime, _clearLineAnimationTime, _clearLineAnimationTime * _clearLineMultiplier))
            {
                for (int i = 0; i < clearedLines.Count; i++)
                {
                    int line = clearedLines[i];
                    for (int column = 0; column < BoardModel.NumColumns; column++)
                    {
                        if (blink)
                        {
                            BoardView.Blocks[line, column].sharedMaterial = _blocks.Gray;
                        }
                        else
                        {
                            int blockType = BoardModel.Blocks[line, column];
                            BoardView.Blocks[line, column].sharedMaterial = _blocks.GetMaterial(blockType);
                        }
                    }
                }

                blink = !blink;

                yield return new WaitForSeconds(_clearLineAnimationTime * _clearLineMultiplier);
            }
        }

        private IEnumerator ControlTetromino()
        {
            float gravityInterval = _levelController.GravityInterval;
            for (float elapsedTime = 0f; elapsedTime < gravityInterval; elapsedTime = Mathf.MoveTowards(elapsedTime, gravityInterval, Time.deltaTime))
            {
                // if drop hard is activated, we skip control
                if (_inputController.DropHard)
                {
                    yield break;
                }

                if (_inputController.HoldPiece && !_isHoldPiece)
                {
                    yield break;
                }

                if (_inputController.DropSoft)
                {
                    gravityInterval = _levelController.DropSoftGravityInterval;
                }

                ClearTetromino(_currentTetromino, _ghostTetromino);

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
                    _currentTetromino.Rotation += 1;
                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous rotation
                        _currentTetromino.Rotation -= 1;
                    }

                }

                if (_inputController.RotateCounterClockwise)
                {
                    _currentTetromino.Rotation -= 1;
                    if (!ValidateTetrominoPosition(_currentTetromino))
                    {
                        // Disable previous rotation
                        _currentTetromino.Rotation += 1;
                    }
                }

                _ghostTetromino.Rotation = _currentTetromino.Rotation;
                DrawTetromino(_currentTetromino, _ghostTetromino);

                yield return null;
            }
        }

        private void ClearTetromino(ITetrominoModel tetromino, ITetrominoModel ghostTetromino = null)
        {
            if (ghostTetromino != null)
            {
                for (int blockLine = 0; blockLine < ghostTetromino.NumLines; blockLine++)
                {
                    for (int blockColumn = 0; blockColumn < ghostTetromino.NumColumns; blockColumn++)
                    {
                        // Converts the block line and column to board line and column
                        int boardLine = ghostTetromino.CurrentLine + blockLine;
                        int boardColumn = ghostTetromino.CurrentColumn + blockColumn;

                        if (boardLine >= 0 && boardLine < BoardModel.NumLines &&
                            boardColumn >= 0 && boardColumn < BoardModel.NumColumns)
                        {
                            if (ghostTetromino.Blocks[blockLine, blockColumn] != Utils.TetrominoUtils.NoPiece)
                            {
                                BoardModel.Blocks[boardLine, boardColumn] = Utils.TetrominoUtils.NoPiece;
                            }
                        }
                    }
                }

                // Update view after hiding tetromino
                int ghostEndLine = ghostTetromino.CurrentLine + ghostTetromino.NumLines;
                int ghostEndColumn = ghostTetromino.CurrentColumn + ghostTetromino.NumColumns;
                BoardView.UpdateView(BoardModel, ghostTetromino.CurrentLine, ghostTetromino.CurrentColumn, ghostEndLine, ghostEndColumn, _blocks);
            }

            for (int blockLine = 0; blockLine < tetromino.NumLines; blockLine++)
            {
                for (int blockColumn = 0; blockColumn < tetromino.NumColumns; blockColumn++)
                {
                    // Converts the block line and column to board line and column
                    int boardLine = tetromino.CurrentLine + blockLine;
                    int boardColumn = tetromino.CurrentColumn + blockColumn;

                    if (boardLine >= 0 && boardLine < BoardModel.NumLines &&
                        boardColumn >= 0 && boardColumn < BoardModel.NumColumns)
                    {
                        if (tetromino.Blocks[blockLine, blockColumn] != Utils.TetrominoUtils.NoPiece)
                        {
                            BoardModel.Blocks[boardLine, boardColumn] = Utils.TetrominoUtils.NoPiece;
                        }
                    }
                }
            }

            // Update view after hiding tetromino
            int endLine = tetromino.CurrentLine + tetromino.NumLines;
            int endColumn = tetromino.CurrentColumn + tetromino.NumColumns;
            BoardView.UpdateView(BoardModel, tetromino.CurrentLine, tetromino.CurrentColumn, endLine, endColumn, _blocks);
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

                    if (boardLine >= 0 && boardLine < BoardModel.NumLines && boardColumn >= 0 && boardColumn < BoardModel.NumColumns)
                    {
                        if (tetromino.Blocks[blockLine, blockColumn] > Utils.TetrominoUtils.NoPiece && BoardModel.Blocks[boardLine, boardColumn] > Utils.TetrominoUtils.NoPiece)
                        {
                            return false;
                        }
                    }
                    else if (tetromino.Blocks[blockLine, blockColumn] > Utils.TetrominoUtils.NoPiece)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void DrawTetromino(ITetrominoModel tetromino, ITetrominoModel ghostTetromino = null)
        {
            // Draw ghost tetromino
            if (ghostTetromino != null)
            {
                for (int blockLine = 0; blockLine < ghostTetromino.NumLines; blockLine++)
                {
                    for (int blockColumn = 0; blockColumn < ghostTetromino.NumColumns; blockColumn++)
                    {
                        int blockType = ghostTetromino.Blocks[blockLine, blockColumn];
                        if (blockType > Utils.TetrominoUtils.NoPiece)
                        {
                            // Converts the block line and column to board line and column
                            int boardLine = ghostTetromino.CurrentLine + blockLine;
                            int boardColumn = ghostTetromino.CurrentColumn + blockColumn;

                            if (boardLine >= 0 && boardLine < BoardModel.NumLines && boardColumn >= 0 && boardColumn < BoardModel.NumColumns)
                            {
                                BoardModel.Blocks[boardLine, boardColumn] = Utils.TetrominoUtils.GhostPiece;
                            }
                        }
                    }
                }

                // Update view after showing tetromino
                int ghostEndLine = ghostTetromino.CurrentLine + ghostTetromino.NumLines;
                int ghostEndColumn = ghostTetromino.CurrentColumn + ghostTetromino.NumColumns;
                BoardView.UpdateView(BoardModel, ghostTetromino.CurrentLine, ghostTetromino.CurrentColumn, ghostEndLine, ghostEndColumn, _blocks);
            }

            // Draw normal tetromino
            for (int blockLine = 0; blockLine < tetromino.NumLines; blockLine++)
            {
                for (int blockColumn = 0; blockColumn < tetromino.NumColumns; blockColumn++)
                {
                    int blockType = tetromino.Blocks[blockLine, blockColumn];
                    if (blockType > Utils.TetrominoUtils.NoPiece)
                    {
                        // Converts the block line and column to board line and column
                        int boardLine = tetromino.CurrentLine + blockLine;
                        int boardColumn = tetromino.CurrentColumn + blockColumn;

                        if (boardLine >= 0 && boardLine < BoardModel.NumLines && boardColumn >= 0 && boardColumn < BoardModel.NumColumns)
                        {
                            BoardModel.Blocks[boardLine, boardColumn] = blockType;
                        }
                    }
                }
            }

            // Update view after showing tetromino
            int endLine = tetromino.CurrentLine + tetromino.NumLines;
            int endColumn = tetromino.CurrentColumn + tetromino.NumColumns;
            BoardView.UpdateView(BoardModel, tetromino.CurrentLine, tetromino.CurrentColumn, endLine, endColumn, _blocks);
        }
    }

    public interface IBoardController
    {
        IBoardModel BoardModel { get; }
        IBoardView BoardView { get; }
    }
}