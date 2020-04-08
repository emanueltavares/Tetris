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

        protected virtual void OnEnable()
        {
            _blockMaterials = new Material[8] { _noBlockMat, _lightBlueBlockMat, _darkBlueBlockMat, _orangeBlockMat, _yellowBlockMat, _greenBlockMat, _purpleBlockMat, _redBlockMat };

            Create();

            _currentTetromino = _tetrominosFactory.GetPiece(Constants.IPieceType);
            ApplyTetrominoToModel(0, 0, _currentTetromino);
            _boardView.UpdateView(_boardModel, _blockMaterials);
        }

        private void Create()
        {            
            (_boardModel, _boardView) = _boardFactory.GetBoard(_blockMaterials);
        }

        private void ApplyTetrominoToModel(int startLine, int startCol, ITetrominoModel tetromino)
        {
            for (int line = startLine; line < tetromino.NumLines; line++)
            {
                for (int col = startCol; col < tetromino.NumColumns; col++)
                {
                    int blockType = tetromino.Blocks[line, col];
                    _boardModel.Blocks[startLine + line, startCol + col] = blockType;
                }
            }
        }
    }
}