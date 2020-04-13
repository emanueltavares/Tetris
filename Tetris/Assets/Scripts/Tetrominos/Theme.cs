using Application.Utils;
using UnityEngine;

namespace Application.Models
{
    [CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme")]
    public class Theme : ScriptableObject
    {
        // Serialized Fields
        [Header("Tetrominos Colors")]
        [SerializeField] private Material _ghost;
        [SerializeField] private Material _background;
        [SerializeField] private Material _IPieceMaterial;
        [SerializeField] private Material _JPieceMaterial;
        [SerializeField] private Material _LPieceMaterial;
        [SerializeField] private Material _SPieceMaterial;
        [SerializeField] private Material _OPieceMaterial;
        [SerializeField] private Material _TPieceMaterial;
        [SerializeField] private Material _ZPieceMaterial;

        // Properties
        public Material Background { get => _background; set => _background = value; }
        public Material Ghost { get => _ghost; set => _ghost = value; }

        public Material GetMaterial(int pieceType)
        {
            switch (pieceType)
            {
                case TetrominoUtils.IPieceType:
                    return _IPieceMaterial;
                case TetrominoUtils.JPieceType:
                    return _JPieceMaterial;
                case TetrominoUtils.LPieceType:
                    return _LPieceMaterial;
                case TetrominoUtils.SPieceType:
                    return _SPieceMaterial;
                case TetrominoUtils.OPieceType:
                    return _OPieceMaterial;
                case TetrominoUtils.TPieceType:
                    return _TPieceMaterial;
                case TetrominoUtils.ZPieceType:
                    return _ZPieceMaterial;
                case TetrominoUtils.NoPiece:
                    return _background;
                default:
                    return _ghost;
            }
        }
    };
}
