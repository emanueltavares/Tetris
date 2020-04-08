using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Factories
{
    public class TetrominosFactory : ITetrominosFactory
    {
        public ITetrominoModel GetPiece(int pieceType)
        {
            switch (pieceType)
            {
                case Constants.OPieceType:
                case Constants.LPieceType:
                case Constants.SPieceType:
                case Constants.TPieceType:
                case Constants.ZPieceType:
                case Constants.JPieceType:
                case Constants.IPieceType:
                default:
                    ITetrominoModel tetrominoModel = new LetterITetrominoModel();
                    return tetrominoModel;
            }            
        }
    }

    public interface ITetrominosFactory
    {
        ITetrominoModel GetPiece(int pieceType);
    }
}