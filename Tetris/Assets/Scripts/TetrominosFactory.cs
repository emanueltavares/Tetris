using System.Collections.Generic;
using Tetris.Models;
using Tetris.Utils;
using UnityEngine;

namespace Tetris.Factories
{
    public class TetrominosFactory : ITetrominosFactory
    {
        // Serialize Field
        [SerializeField] private int _maxNextPieces = 5;

        // Private 
        private List<int> _nextPieceTypes = new List<int>();
        private System.Random _random = new System.Random();

        // Static readonlies
        private static readonly List<int> PieceTypes = new List<int>()
        {
            Constants.IPieceType,
            Constants.JPieceType,
            Constants.LPieceType,
            Constants.SPieceType,
            Constants.OPieceType,
            Constants.TPieceType,
            Constants.ZPieceType
        };

        private void InitNextPieces()
        {
            int pieceTypesIndex = _random.Next(PieceTypes.Count - 1);
            int randomPieceType = PieceTypes[pieceTypesIndex];
            _nextPieceTypes.Add(randomPieceType);

            // Add other random pieces
            for (int i = 1; i < _maxNextPieces; i++)
            {
                // remove previous piece type
                List<int> pieceTypes = new List<int>(PieceTypes);
                pieceTypes.Remove(randomPieceType);

                pieceTypesIndex = _random.Next(pieceTypes.Count - 1);
                randomPieceType = pieceTypes[pieceTypesIndex];
                _nextPieceTypes.Add(randomPieceType);
            }
        }

        public ITetrominoModel GetNextPiece(int startLine, int startColumn)
        {
            if (_nextPieceTypes.Count <= 0)
            {
                InitNextPieces();
            }

            int pieceType = _nextPieceTypes[0];

            // get next piece type
            List<int> pieceTypes = new List<int>(PieceTypes);
            pieceTypes.Remove(_nextPieceTypes[_nextPieceTypes.Count - 1]);
            int nextPieceType = pieceTypes[_random.Next(pieceTypes.Count - 1)];

            _nextPieceTypes.RemoveAt(0);
            _nextPieceTypes.Add(nextPieceType);

            // next tetromino
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
        ITetrominoModel GetNextPiece(int startLine, int startColumn);
    }
}