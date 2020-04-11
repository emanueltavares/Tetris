using Tetris.Models;
using Tetris.Views;
using UnityEngine;

namespace Tetris.Factories
{
    public class BoardFactory : MonoBehaviour, IBoardFactory
    {
        public (IBoardModel, IBoardView) GetBoard(Renderer blocksPrefab, Material[] blockMaterials, float blockScale, Transform blocksParent, int numLines, int numColumns)
        {
            // Create Builder
            BoardModel.Builder modelBuilder = new BoardModel.Builder();
            BoardView.Builder viewBuilder = new BoardView.Builder();

            IBoardModel boardModel = modelBuilder.Build(numLines, numColumns);
            IBoardView boardView = viewBuilder.Build(boardModel, blocksPrefab, blockMaterials, blockScale, blocksParent);
            return (boardModel, boardView);
        }
    }
}
