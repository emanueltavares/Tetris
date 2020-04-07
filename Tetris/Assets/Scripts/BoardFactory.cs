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
        [SerializeField] private Material _noBlockMat;         // GREY is the color of the empty block
        [SerializeField] private Material _lightBlueBlockMat;  // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlueBlockMat;   // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orangeBlockMat;     // ORANGE is the color of the L piece
        [SerializeField] private Material _yellowBlockMat;     // YELLOW is the color of the O piece
        [SerializeField] private Material _greenBlockMat;      // GREEN is the color of the S piece
        [SerializeField] private Material _purpleBlockMat;     // PURPLE is the color of the T piece
        [SerializeField] private Material _redBlockMat;        // RED is the color of the Z piece

        [Header("Tetrominos Settings")]
        [SerializeField] private Renderer _blockPrefab;
        [SerializeField] private float _blockScale = 1f;

        private readonly System.Random _sysRandom = new System.Random();

        public IBoardView GetBoard(Transform root, int numLines, int numColumns)
        {
            // Create Builder
            BoardModel.Builder builder = new BoardModel.Builder();

            // Create Board Model
            int[,] blocks = new int[numLines, numColumns];
            for (int line = 0; line < numLines; line++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    //blocks[line, col] = NoBlock;
                    blocks[line, col] = _sysRandom.Next(0, 8);
                }
            }

            IBoardModel boardModel = builder.Build(blocks);

            float halfBoardWidth = _blockScale * (numColumns - 1) * 0.5f;
            float halfBoardHeight = _blockScale * (numLines - 1) * 0.5f;

            // Create Board View from Model
            for (int line = 0; line < numLines; line++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // Set position and scale
                    float normalizedColumn = col / (numColumns - 1f);
                    float normalizedLine = line / (numLines - 1f);
                    float blockX = Mathf.Lerp(-halfBoardWidth, halfBoardWidth, normalizedColumn);
                    float blockY = Mathf.Lerp(halfBoardHeight, -halfBoardHeight, normalizedLine);
                    Vector3 localPosition = new Vector3(blockX, blockY, 0);
                    Vector3 localScale = new Vector3(_blockScale, _blockScale, _blockScale);

                    // Create a transform
                    Renderer blockInstance = Instantiate(_blockPrefab, root);
                    blockInstance.transform.localPosition = localPosition;
                    blockInstance.transform.localScale = localScale;
                    blockInstance.gameObject.name = string.Format("Block [{0}, {1}]", line, col);

                    int blockType = boardModel.Blocks[line, col];
                    switch (blockType)
                    {
                        case NoBlock:
                            blockInstance.sharedMaterial = _noBlockMat;
                            break;
                        case LightBlueBlock:
                            blockInstance.sharedMaterial = _lightBlueBlockMat;
                            break;
                        case DarkBlueBlock:
                            blockInstance.sharedMaterial = _darkBlueBlockMat;
                            break;
                        case OrangeBlock:
                            blockInstance.sharedMaterial = _orangeBlockMat;
                            break;
                        case YellowBlock:
                            blockInstance.sharedMaterial = _yellowBlockMat;
                            break;
                        case GreenBlock:
                            blockInstance.sharedMaterial = _greenBlockMat;
                            break;
                        case PurpleBlock:
                            blockInstance.sharedMaterial = _purpleBlockMat;
                            break;
                        case RedBlock:
                            blockInstance.sharedMaterial = _redBlockMat;
                            break;
                    }
                }
            }

            return default;
        }
    }
}
