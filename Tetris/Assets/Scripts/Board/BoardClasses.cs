using Tetris.Utils;
using Tetris.Views;
using UnityEngine;

namespace Tetris.Models
{
    public class BoardModel : IBoardModel
    {
        public int NumLines { get; private set; }
        public int NumColumns { get; private set; }
        public int[,] Blocks { get; private set; }

        public class Builder
        {
            public IBoardModel Build(int maxNumLines, int maxNumColumns)
            {
                int[,] blocks = new int[maxNumLines, maxNumColumns];
                for (int line = 0; line < maxNumLines; line++)
                {
                    for (int col = 0; col < maxNumColumns; col++)
                    {
                        blocks[line, col] = Utils.TetrominoUtils.NoPiece;
                        //blocks[line, col] = Random.Range(0, 8);
                    }
                }

                IBoardModel boardModel = new BoardModel()
                {
                    NumLines = blocks.GetLength(0),
                    NumColumns = blocks.GetLength(1),
                    Blocks = blocks
                };
                return boardModel;
            }
        }
    }

    public class BoardView : IBoardView
    {
        public Renderer[,] Blocks { get; private set; }

        public void HideView(Theme blocks)
        {
            int numLines = Blocks.GetLength(0);
            int numColumns = Blocks.GetLength(1);
            for (int line = 0; line < numLines; line++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    if (line >= 0 && line < numLines && column >= 0 && column < numColumns)
                    {
                        Blocks[line, column].sharedMaterial = blocks.Background;
                    }
                }
            }
        }

        public void UpdateView(IBoardModel boardModel, int startLine, int startColumn, int endLine, int endColumn, Theme blocks)
        {
            for (int line = startLine; line < endLine; line++)
            {
                for (int column = startColumn; column < endColumn; column++)
                {
                    if (line >= 0 && line < boardModel.NumLines && column >= 0 && column < boardModel.NumColumns)
                    {
                        int blockType = boardModel.Blocks[line, column];
                        Blocks[line, column].sharedMaterial = blocks.GetMaterial(blockType);
                    }
                }
            }
        }

        public void UpdateView(IBoardModel boardModel, Theme blocks)
        {
            for (int line = 0; line < boardModel.NumLines; line++)
            {
                for (int column = 0; column < boardModel.NumColumns; column++)
                {
                    if (line >= 0 && line < boardModel.NumLines && column >= 0 && column < boardModel.NumColumns)
                    {
                        int blockType = boardModel.Blocks[line, column];
                        Blocks[line, column].sharedMaterial = blocks.GetMaterial(blockType);
                    }
                }
            }
        }

        public class Builder
        {
            public IBoardView Build(IBoardModel boardModel, Renderer blockPrefab, Theme blocks, float blockScale, Transform parent)
            {
                Renderer[,] blockRenderers = new Renderer[boardModel.NumLines, boardModel.NumColumns];

                float halfBoardWidth = blockScale * (boardModel.NumColumns - 1) * 0.5f;
                float halfBoardHeight = blockScale * (boardModel.NumLines - 1) * 0.5f;
                Vector3 localScale = new Vector3(blockScale, blockScale, blockScale - Mathf.Epsilon);

                // Create Board View from Model
                for (int line = 0; line < boardModel.NumLines; line++)
                {
                    for (int col = 0; col < boardModel.NumColumns; col++)
                    {
                        // Set position and scale
                        float normalizedColumn = col / (boardModel.NumColumns - 1f);
                        float normalizedLine = line / (boardModel.NumLines - 1f);
                        float blockX = Mathf.Lerp(-halfBoardWidth, halfBoardWidth, normalizedColumn);
                        float blockY = Mathf.Lerp(halfBoardHeight, -halfBoardHeight, normalizedLine);
                        Vector3 localPosition = new Vector3(blockX, blockY, 0);

                        // Create a transform
                        Renderer blockInstance = Object.Instantiate(blockPrefab, parent);
                        blockInstance.transform.localPosition = localPosition;
                        blockInstance.transform.localScale = localScale;

                        int blockType = boardModel.Blocks[line, col];
                        blockInstance.sharedMaterial = blocks.GetMaterial(blockType);
                        blockRenderers[line, col] = blockInstance;

                        blockInstance.gameObject.name = string.Format("Block [{0}, {1}] Value: {2}", line, col, blockType);
                    }
                }

                IBoardView boardView = new BoardView()
                {
                    Blocks = blockRenderers
                };

                return boardView;
            }
        }
    }
}