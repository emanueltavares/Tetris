using Tetris.Utils;
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
                        //blocks[line, col] = Constants.NoPiece;
                        blocks[line, col] = Random.Range(0, 8);
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

        public void UpdateView(IBoardModel boardModel, Material[] blockMaterials)
        {
            UpdateView(boardModel, 0, 0, boardModel.NumLines, boardModel.NumColumns, blockMaterials);
        }

        public void UpdateView(IBoardModel boardModel, int startLine, int startColumn, int endLine, int endColumn, Material[] blockMaterials)
        {
            for (int line = startLine; line < endLine; line++)
            {
                for (int col = startColumn; col < endColumn; col++)
                {
                    int blockType = boardModel.Blocks[line, col];
                    Blocks[line, col].sharedMaterial = blockMaterials[blockType];
                }
            }
        }

        public class Builder
        {
            public IBoardView Build(IBoardModel boardModel, Renderer blockPrefab, Material[] blockMaterials, float blockScale, Transform parent)
            {
                Renderer[,] blocks = new Renderer[boardModel.NumLines, boardModel.NumColumns];

                float halfBoardWidth = blockScale * (boardModel.NumColumns - 1) * 0.5f;
                float halfBoardHeight = blockScale * (boardModel.NumLines - 1) * 0.5f;

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
                        Vector3 localScale = new Vector3(blockScale, blockScale, blockScale);

                        // Create a transform
                        Renderer blockInstance = Object.Instantiate(blockPrefab, parent);
                        blockInstance.transform.localPosition = localPosition;
                        blockInstance.transform.localScale = localScale;                        

                        int blockType = boardModel.Blocks[line, col];
                        blockInstance.sharedMaterial = blockMaterials[blockType];
                        blocks[line, col] = blockInstance;

                        blockInstance.gameObject.name = string.Format("Block [{0}, {1}] Value: {2}", line, col, blockType);
                    }
                }

                IBoardView boardView = new BoardView()
                {
                    Blocks = blocks
                };

                return boardView;
            }
        }
    }
}