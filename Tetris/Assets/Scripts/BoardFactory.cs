using Tetris.Models;
using UnityEngine;

namespace Tetris.Factories
{
    public class BoardFactory : MonoBehaviour, IBoardFactory
    {
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private Transform _blocksParent;
        [SerializeField] private int _maxNumLines;             // max number of lines of our board
        [SerializeField] private int _maxNumColumns;           // max number of columns of our board
        [SerializeField] private float _blockScale = 1f;

        public (IBoardModel, IBoardView) GetBoard(Material[] blockMaterials)
        {
            // Create Builder
            BoardModel.Builder modelBuilder = new BoardModel.Builder();
            BoardView.Builder viewBuilder = new BoardView.Builder();

            IBoardModel boardModel = modelBuilder.Build(_maxNumLines, _maxNumColumns);
            IBoardView boardView = viewBuilder.Build(boardModel, _blockPrefab, blockMaterials, _blockScale, _blocksParent);
            return (boardModel, boardView);
        }
    }
}
