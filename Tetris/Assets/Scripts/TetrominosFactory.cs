using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Factories
{
    public class TetrominosFactory : ITetrominosFactory
    {
        public ITetrominoModel GetPiece(int startLine, int startColumn, int pieceType)
        {
            ITetrominoModel tetromino;
            switch (pieceType)
            {
                case Constants.OPieceType:
                    tetromino = new LetterOTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.LPieceType:
                    tetromino = new LetterLTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.SPieceType:
                    tetromino = new LetterSTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.TPieceType:
                    tetromino = new LetterTTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.ZPieceType:
                    tetromino = new LetterZTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.JPieceType:
                    tetromino = new LetterJTetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                case Constants.IPieceType:
                    tetromino = new LetterITetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
                default:
                    tetromino = new LetterITetrominoModel()
                    {
                        CurrentLine = startLine,
                        CurrentColumn = startColumn
                    };
                    break;
            }
            return tetromino;
        }
    }

    public interface ITetrominosFactory
    {
        ITetrominoModel GetPiece(int startLine, int startColumn, int pieceType);
    }
}