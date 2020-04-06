using Tetris.Models;
using UnityEngine;

namespace Tetris.Factories
{
    public class BoardFactory : MonoBehaviour, IBoardFactory
    {
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private float _blockWidth = 1f;
        [SerializeField] private float _blockHeight = 1f;

        public IBoardView GetBoard(int numLines, int numColumns)
        {
            // Create Builder
            BoardModel.Builder builder = new BoardModel.Builder();

            // Create Board Model
            bool[,] blocks = new bool[5, 5]
            {
                {true, false, true, false, false },   // line 1
                {true, false, true, false, false },   // line 2
                {true, false, true, false, true },    // line 3
                {true, false, true, false, false },   // line 4
                {true, false, true, false, false }    // line 5
            };

            IBoardModel boardModel = builder.Build(blocks);

            float halfBoardWidth = _blockWidth * numLines * 0.5f;
            float halfBoardHeight = _blockHeight * numColumns * 0.5f;

            // Create Board View from Model
            for (int line = 0; line < numLines; line++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // Create a transform
                    // TODO: Use an object pool instead
                    GameObject blockInstance = Instantiate(_blockPrefab);
                    blockInstance.name = string.Format("Cell [{0}, {1}]", line, col);

                    // Set position and scale
                    float normalizedColumn = col / (float)numColumns;
                    float normalizedLine = line / (float)numLines;
                    float blockX = Mathf.Lerp(-halfBoardWidth, halfBoardWidth, normalizedColumn);
                    float blockY = Mathf.Lerp(-halfBoardHeight, halfBoardHeight, normalizedLine);
                    blockInstance.transform.localPosition = new Vector3(blockX, blockY, 0);
                    blockInstance.transform.localScale = new Vector3(_blockWidth, _blockHeight, 1f);

                    bool hasBlock = boardModel.Blocks[line, col];
                    blockInstance.SetActive(hasBlock);
                }
            }
            
            return default;
        }
    }
}
