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
        Transform[,] Blocks { get; }
    }

    public interface IBoardFactory
    {
        IBoardView GetBoard(Transform root, int numLines, int numColumns);
    }
}