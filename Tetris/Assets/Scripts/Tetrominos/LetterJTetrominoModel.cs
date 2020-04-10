﻿using Tetris.Utils;

namespace Tetris.Models
{
    public class LetterJTetrominoModel : ITetrominoModel
    {
        // Constants
        private const int LetterJNumLines = 3;
        private const int LetterJNumColumns = 3;
        private const int MaxRotations = 4;

        // Readonlies
        private static readonly int[,] First = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    0,                    0                    },
                { Constants.JPieceType, Constants.JPieceType, Constants.JPieceType },
                { 0,                    0,                    Constants.JPieceType }                
            };

        private static readonly int[,] Second = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    Constants.JPieceType, 0                    },
                { 0,                    Constants.JPieceType, 0                    },
                { Constants.JPieceType, Constants.JPieceType, 0                    }
            };

        private static readonly int[,] Third = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    0,                    0                    },
                { Constants.JPieceType, 0,                    0                    },
                { Constants.JPieceType, Constants.JPieceType, Constants.JPieceType }
            };

        private static readonly int[,] Fourth = new int[LetterJNumLines, LetterJNumColumns]
            {
                { 0,                    Constants.JPieceType, Constants.JPieceType },
                { 0,                    Constants.JPieceType, 0                    },
                { 0,                    Constants.JPieceType, 0                    }
            };

        // Properties
        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }
        public int NumLines => LetterJNumLines;
        public int NumColumns => LetterJNumColumns;
        public int[,] Blocks { get; private set; }

        // Private
        private int _currentRotation = 0;

        public LetterJTetrominoModel()
        {
            Blocks = First;
        }

        public void RotateClockwise()
        {
            _currentRotation += 1;
            _currentRotation = MathExt.Mod(_currentRotation, MaxRotations);
            SetRotation();
        }

        public void RotateCounterClockwise()
        {
            _currentRotation -= 1;
            _currentRotation = MathExt.Mod(_currentRotation, MaxRotations);

            SetRotation();
        }

        private void SetRotation()
        {
            switch (_currentRotation)
            {
                case 0:
                    Blocks = First;
                    break;
                case 1:
                    Blocks = Second;
                    break;
                case 2:
                    Blocks = Third;
                    break;
                case 3:
                    Blocks = Fourth;
                    break;
            }
        }
    }
}