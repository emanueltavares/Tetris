using Application.Utils;
using UnityEngine;

namespace Application.Models
{
    [CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme")]
    public class Theme : ScriptableObject
    {
        // Serialized Fields
        [Header("Tetrominos Colors")]
        [SerializeField] private Material _ghost;                                                  // GHOST is the color of the ghost block
        [SerializeField] private Material _background;                                             // BACKGROUND is the color of the empty block
        [SerializeField] private Material _lightBlue;                                              // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlue;                                               // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orange;                                                 // ORANGE is the color of the L piece
        [SerializeField] private Material _yellow;                                                 // YELLOW is the color of the O piece
        [SerializeField] private Material _green;                                                  // GREEN is the color of the S piece
        [SerializeField] private Material _purple;                                                 // PURPLE is the color of the T piece
        [SerializeField] private Material _red;                                                    // RED is the color of the Z piece

        // Properties
        public Material Background { get => _background; set => _background = value; }
        public Material Ghost { get => _ghost; set => _ghost = value; }

        public Material GetMaterial(int pieceType)
        {
            switch (pieceType)
            {
                case TetrominoUtils.IPieceType:
                    return _lightBlue;
                case TetrominoUtils.JPieceType:
                    return _darkBlue;
                case TetrominoUtils.LPieceType:
                    return _orange;
                case TetrominoUtils.SPieceType:
                    return _yellow;
                case TetrominoUtils.OPieceType:
                    return _green;
                case TetrominoUtils.TPieceType:
                    return _purple;
                case TetrominoUtils.ZPieceType:
                    return _red;
                case TetrominoUtils.NoPiece:
                    return _background;
                default:
                    return _ghost;
            }
        }
    };
}
