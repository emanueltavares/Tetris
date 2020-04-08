using UnityEngine;

namespace Tetris.Models
{
    public interface IBoardModel
    {
        int NumLines { get; }
        int NumColumns { get; }
        int[,] Blocks { get; }
    }

    public interface IBoardView
    {
        Renderer[,] Blocks { get; }
    }

    public interface IBoardFactory
    {
        (IBoardModel, IBoardView) GetBoard();
    }
}