using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Factories
{
    public class BoardFactory : MonoBehaviour, IBoardFactory
    {
        [Header("Tetrominos Colors")]
        [SerializeField] private Material _noBlockMat;         // GREY is the color of the empty block
        [SerializeField] private Material _lightBlueBlockMat;  // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlueBlockMat;   // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orangeBlockMat;     // ORANGE is the color of the L piece
        [SerializeField] private Material _yellowBlockMat;     // YELLOW is the color of the O piece
        [SerializeField] private Material _greenBlockMat;      // GREEN is the color of the S piece
        [SerializeField] private Material _purpleBlockMat;     // PURPLE is the color of the T piece
        [SerializeField] private Material _redBlockMat;        // RED is the color of the Z piece

        [Header("Tetrominos Creation")]
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private Transform _blocksParent;
        [SerializeField] private int _maxNumLines;             // max number of lines of our board
        [SerializeField] private int _maxNumColumns;           // max number of columns of our board
        [SerializeField] private float _blockScale = 1f;

        
        public (IBoardModel, IBoardView) GetBoard()
        {
            Material[] _blocksMaterial = new Material[8] { _noBlockMat, _lightBlueBlockMat, _darkBlueBlockMat, _orangeBlockMat, _yellowBlockMat, _greenBlockMat, _purpleBlockMat, _redBlockMat };

            // Create Builder
            BoardModel.Builder modelBuilder = new BoardModel.Builder();
            BoardView.Builder viewBuilder = new BoardView.Builder();

            IBoardModel boardModel = modelBuilder.Build(_maxNumLines, _maxNumColumns);
            IBoardView boardView = viewBuilder.Build(boardModel, _blockPrefab, _blocksMaterial, _blockScale, _blocksParent);
            return (boardModel, boardView);
        }
    }
}
