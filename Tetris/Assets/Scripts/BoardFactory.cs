using Tetris.Models;
using UnityEngine;

namespace Tetris.Factories
{
    public class BoardFactory : MonoBehaviour, IBoardFactory
    {
        private const int NoBlock = 0;
        private const int LightBlueBlock = 1;
        private const int DarkBlueBlock = 2;
        private const int OrangeBlock = 3;
        private const int GreenBlock = 4;
        private const int YellowBlock = 5;
        private const int PurpleBlock = 6;
        private const int RedBlock = 7;

        [Header("Tetrominos Colors")]
        [SerializeField] private Material _lightBlueBlockMat;  // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlueBlockMat;   // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orangeBlockMat;     // ORANGE is the color of the L piece
        [SerializeField] private Material _yellowBlockMat;     // YELLOW is the color of the O piece
        [SerializeField] private Material _greenBlockMat;      // GREEN is the color of the S piece
        [SerializeField] private Material _purpleBlockMat;     // PURPLE is the color of the T piece
        [SerializeField] private Material _redBlockMat;        // RED is the color of the Z piece

        [Header("Tetrominos Settings")]
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private float _blockWidth = 1f;
        [SerializeField] private float _blockHeight = 1f;

        public IBoardView GetBoard(int numLines, int numColumns)
        {
            // Create Builder
            BoardModel.Builder builder = new BoardModel.Builder();

            // Create Board Model
            int[,] blocks = new int[5, 5]
            {
                {0, 0, 0, 0, 0 },   // line 1
                {1, 2, 3, 4, 5 },   // line 2
                {6, 7, 1, 2, 3 },   // line 3
                {4, 5, 6, 7, 1 },   // line 4
                {0, 0, 0, 0, 0 }    // line 5
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
                    blockInstance.name = string.Format("Block [{0}, {1}]", line, col);

                    // Set position and scale
                    float normalizedColumn = col / (float)numColumns;
                    float normalizedLine = line / (float)numLines;
                    float blockX = Mathf.Lerp(-halfBoardWidth, halfBoardWidth, normalizedColumn);
                    float blockY = Mathf.Lerp(-halfBoardHeight, halfBoardHeight, normalizedLine);
                    blockInstance.transform.localPosition = new Vector3(blockX, blockY, 0);
                    blockInstance.transform.localScale = new Vector3(_blockWidth, _blockHeight, 1f);

                    Renderer renderer = blockInstance.GetComponent<Renderer>();
                    int blockType = boardModel.Blocks[line, col];
                    switch (blockType)
                    {                        
                        case LightBlueBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _lightBlueBlockMat;
                            break;
                        case DarkBlueBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _darkBlueBlockMat;
                            break;
                        case OrangeBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _orangeBlockMat;
                            break;
                        case YellowBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _yellowBlockMat;
                            break;
                        case GreenBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _greenBlockMat;
                            break;
                        case PurpleBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _purpleBlockMat;
                            break;
                        case RedBlock:
                            blockInstance.SetActive(true);
                            renderer.sharedMaterial = _redBlockMat;
                            break;
                        default:
                            blockInstance.SetActive(false);
                            break;
                    }
                }
            }
            
            return default;
        }
    }
}
