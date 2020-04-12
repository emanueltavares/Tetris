using System.Collections;
using System.Collections.Generic;
using Tetris.Factories;
using Tetris.Models;
using Tetris.Utils;
using Tetris.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.Controllers
{
    public partial class BoardController : MonoBehaviour, IBoardController
    {
        // Serialized Fields 
        [Header("UI")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Animation")]
        [SerializeField] private float _clearLineAnimationTime = 1f;
        [Range(0f, 1f)] [SerializeField] private float _clearLineMultiplier = 0.125f;

        [Header("Block Creation")]
        [SerializeField] private Theme _blocks;
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private Transform _blocksParent;
        [SerializeField] private int _maxNumLines;                                              // max number of lines of our board
        [SerializeField] private int _maxNumColumns;                                            // max number of columns of our board
        [SerializeField] private float _blockScale = 1f;

        // Private Fields
        private ISoundController _soundController;
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
        public bool IsInitialized { get; private set; }
        public bool IsPaused { get; private set; }

        protected virtual void OnEnable()
        {
            Initialize();

            // Start the game
            StartCoroutine(SpawnTetromino(false));
        }

        public void Initialize()
        {
            if (!IsInitialized)
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

                if (_soundController == null)
                {
                    _soundController = GetComponent<ISoundController>();
                }

                _pausePanel.SetActive(IsPaused);

                IsInitialized = true;
            }
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
                // Play this as a
                _soundController.PlayHoldTetromino();

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

            if (CanPlaceTetrominoWithWallKick(_currentTetromino))
            {
                ClearTetromino(_currentTetromino);

                // Show the tetromino
                _ghostTetromino = Utils.TetrominoUtils.CloneTetromino(_currentTetromino);

                UpdateGhostTetromino(_currentTetromino, _ghostTetromino);
                DrawGhostTetromino(_ghostTetromino);
                DrawTetromino(_currentTetromino);

                yield return StartCoroutine(MoveTetromino());
            }
            else
            {
                yield return StartCoroutine(ShowGameOver());
            }

        }

        private IEnumerator ShowGameOver()
        {
            ClearTetromino(_currentTetromino);
            DrawTetromino(_currentTetromino);

            yield return new WaitForSeconds(1f);

            // Game Over
            _gameOverPanel.SetActive(true);
        }

        private void UpdateGhostTetromino(ITetrominoModel tetromino, ITetrominoModel ghostTetromino)
        {
            ghostTetromino.CurrentLine = tetromino.CurrentLine;
            ghostTetromino.CurrentColumn = tetromino.CurrentColumn;
            ghostTetromino.Rotation = tetromino.Rotation;

            for (int line = ghostTetromino.CurrentLine; line < BoardModel.NumLines; line++)
            {
                int previousLine = ghostTetromino.CurrentLine;
                ghostTetromino.CurrentLine = line;

                if (!CanPlaceTetromino(ghostTetromino))
                {
                    ghostTetromino.CurrentLine = previousLine;
                    break;
                }
            }
        }

        private void DrawGhostTetromino(ITetrominoModel ghostTetromino)
        {
            // Draw ghost tetromino
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

                ClearTetromino(_ghostTetromino);
                ClearTetromino(_currentTetromino);

                if (_inputController.HoldPiece && !_isHoldPiece)
                {
                    useHoldPiece = true;
                }
                else
                {
                    // Apply gravity
                    _currentTetromino.CurrentLine += 1;

                    if (!CanPlaceTetromino(_currentTetromino))
                    {
                        // rollback and lock the tetromino
                        _currentTetromino.CurrentLine -= 1;
                        isTetrominoLocked = true;
                    }
                    UpdateGhostTetromino(_currentTetromino, _ghostTetromino);
                    DrawGhostTetromino(_ghostTetromino);
                    DrawTetromino(_currentTetromino);
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
                    _soundController.PlayPlaceTetromino();

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
                    BoardModel.Blocks[line, column] = Mathf.Max(listBlocks[line][column], Utils.TetrominoUtils.NoPiece);
                }
            }

            BoardView.UpdateView(BoardModel, _blocks);
        }

        private IEnumerator AnimateClearedLines(List<int> clearedLines)
        {
            // Play sound
            _soundController.PlayClearLine();

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
            float elapsedTime = 0f;
            while (elapsedTime < gravityInterval)
            {
                if (_inputController.Pause)
                {
                    if (!IsPaused)
                    {
                        ClearTetromino(_ghostTetromino);
                        ClearTetromino(_currentTetromino);
                        BoardView.HideView(_blocks);
                    }
                    else
                    {
                        BoardView.UpdateView(BoardModel, _blocks);
                    }

                    IsPaused = !IsPaused;
                    _pausePanel.SetActive(IsPaused);
                }

                if (!IsPaused)
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

                    ClearTetromino(_ghostTetromino);
                    ClearTetromino(_currentTetromino);

                    if (_inputController.MoveLeft)
                    {
                        _currentTetromino.CurrentColumn -= 1;

                        if (!CanPlaceTetromino(_currentTetromino))
                        {
                            // Disable previous move
                            _currentTetromino.CurrentColumn += 1;
                        }
                        else
                        {
                            _soundController.PlayMoveTetromino();
                        }
                    }

                    if (_inputController.MoveRight)
                    {
                        _currentTetromino.CurrentColumn += 1;
                        if (!CanPlaceTetromino(_currentTetromino))
                        {
                            // Disable previous move
                            _currentTetromino.CurrentColumn -= 1;
                        }
                        else
                        {
                            _soundController.PlayMoveTetromino();
                        }
                    }

                    if (_inputController.RotateClockwise)
                    {
                        _currentTetromino.Rotation += 1;
                        if (!CanPlaceTetrominoWithWallKick(_currentTetromino))
                        {
                            // Disable previous rotation
                            _currentTetromino.Rotation -= 1;
                        }
                        else if (_currentTetromino.MaxRotations > 1)
                        {
                            _soundController.PlayRotateTetromino();
                        }
                    }

                    if (_inputController.RotateCounterClockwise)
                    {
                        _currentTetromino.Rotation -= 1;
                        if (!CanPlaceTetrominoWithWallKick(_currentTetromino))
                        {
                            // Disable previous rotation
                            _currentTetromino.Rotation += 1;
                        }
                        else if (_currentTetromino.MaxRotations > 1)
                        {
                            _soundController.PlayRotateTetromino();
                        }
                    }

                    elapsedTime = Mathf.MoveTowards(elapsedTime, gravityInterval, Time.deltaTime);

                    UpdateGhostTetromino(_currentTetromino, _ghostTetromino);
                    DrawGhostTetromino(_ghostTetromino);
                    DrawTetromino(_currentTetromino);
                }

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

        private bool CanPlaceTetrominoWithWallKick(ITetrominoModel tetromino)
        {
            // first try
            int currentColumnCache = tetromino.CurrentColumn;
            if (!CanPlaceTetromino(tetromino))
            {
                // second try
                tetromino.CurrentColumn -= 1;
                if (!CanPlaceTetromino(tetromino))
                {
                    // additional try that can only be used for I Pieces
                    if (tetromino.PieceType == TetrominoUtils.IPieceType)
                    {
                        tetromino.CurrentColumn -= 1;
                        if (CanPlaceTetromino(tetromino))
                        {
                            return true;
                        }
                    }

                    // last try
                    tetromino.CurrentColumn = currentColumnCache;
                    tetromino.CurrentColumn += 1;
                    if (!CanPlaceTetromino(tetromino))
                    {
                        // additional try that can only be used for I Pieces
                        if (tetromino.PieceType == TetrominoUtils.IPieceType)
                        {
                            tetromino.CurrentColumn += 1;
                            if (CanPlaceTetromino(tetromino))
                            {
                                return true;
                            }
                        }

                        tetromino.CurrentColumn = currentColumnCache;
                        return false;
                    }

                }
            }
            return true;
        }

        private bool CanPlaceTetromino(ITetrominoModel tetromino)
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

        private void DrawTetromino(ITetrominoModel tetromino)
        {
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

        public void GoToMenu()
        {
            SceneManager.LoadScene(TetrominoUtils.StartScene);
        }

        public void ResumeGame()
        {
            BoardView.UpdateView(BoardModel, _blocks);

            IsPaused = false;
            _pausePanel.SetActive(false);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(TetrominoUtils.GameScene);
        }
    }

    public interface IBoardController
    {
        bool IsPaused { get; }
        bool IsInitialized { get; }
        void Initialize();
        void StartGame();
        void ResumeGame();
        void GoToMenu();
        IBoardModel BoardModel { get; }
        IBoardView BoardView { get; }
    }
}