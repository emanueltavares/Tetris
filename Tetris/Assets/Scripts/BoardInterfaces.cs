using Tetris.Models;
using Tetris.Views;
using UnityEngine;

namespace Tetris.Models
{
    public interface IBoardModel
    {
        int NumLines { get; }
        int NumColumns { get; }
        int[,] Blocks { get; }
    }    
}

namespace Tetris.Views
{
    public interface IBoardView
    {
        Renderer[,] Blocks { get; }
        void UpdateView(IBoardModel boardModel, BlocksScriptableObject blocks);
        void UpdateView(IBoardModel boardModel, int startLine, int startColumn, int endLine, int endColumn, BlocksScriptableObject blocks);
    }
}

namespace Tetris.Factories
{
    public interface IBoardFactory
    {
        (IBoardModel, IBoardView) GetBoard(Renderer blocksPrefab, BlocksScriptableObject _blocks, float blockScale, Transform blocksParent, int numLines, int numColumns);
    }
}