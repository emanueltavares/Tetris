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
        void UpdateView(IBoardModel boardModel, Theme blocks);
        void UpdateView(IBoardModel boardModel, int startLine, int startColumn, int endLine, int endColumn, Theme blocks);
        void HideView(Theme blocks);
    }
}

namespace Tetris.Factories
{
    public interface IBoardFactory
    {
        (IBoardModel, IBoardView) GetBoard(Renderer blocksPrefab, Theme _blocks, float blockScale, Transform blocksParent, int numLines, int numColumns);
    }
}