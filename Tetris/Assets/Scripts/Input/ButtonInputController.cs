﻿using UnityEngine;

namespace Application.Controllers
{
    public class ButtonInputController : MonoBehaviour, IInputController
    {
        [SerializeField] private string _pauseButtonName;
        [SerializeField] private string _leftButtonName;
        [SerializeField] private string _rightButtonName;
        [SerializeField] private string _rotateClockwiseButtonName;
        [SerializeField] private string _rotateCounterClockwiseButtonName;
        [SerializeField] private string _dropHardButtonName;
        [SerializeField] private string _dropSoftButtonName;
        [SerializeField] private string _holdPieceButtonName;

        // Private Fields
        private float _holdInputStartTime = 0f;
        private float _holdInputMaxTime = 0f;
        private ILevelController _levelController;
        private IBoardController _boardController;

        // Properties
        public bool MoveLeft { get; private set; }
        public bool MoveRight { get; private set; }
        public bool RotateClockwise { get; private set; }
        public bool RotateCounterClockwise { get; private set; }
        public bool DropHard { get; private set; }
        public bool DropSoft { get; private set; }
        public bool HoldPiece { get; private set; }
        public bool Pause { get; private set; }

        protected virtual void OnEnable()
        {
            if (_levelController == null)
            {
                // Initializes level controller
                _levelController = GetComponent<ILevelController>();
                _levelController.AddClearedLines(0);
            }

            if (_boardController == null)
            {
                _boardController = GetComponent<IBoardController>();
                _boardController.Initialize();
            }

            _holdInputMaxTime = _levelController.GravityInterval / _boardController.BoardModel.NumColumns;
        }

        protected virtual void Update()
        {
            Pause = Input.GetButtonDown(_pauseButtonName);
            HoldPiece = Input.GetButtonDown(_holdPieceButtonName);
            RotateClockwise = Input.GetButtonDown(_rotateClockwiseButtonName);
            RotateCounterClockwise = Input.GetButtonDown(_rotateCounterClockwiseButtonName);
            DropHard = Input.GetButtonDown(_dropHardButtonName);
            DropSoft = Input.GetButton(_dropSoftButtonName);

            // Is holding left arrow key
            MoveLeft = false;
            if (Input.GetButton(_leftButtonName))
            {
                // first frame
                if (Input.GetButtonDown(_leftButtonName))
                {
                    MoveLeft = true;
                    _holdInputStartTime = Time.realtimeSinceStartup;
                }
                else if (_holdInputStartTime + _holdInputMaxTime < Time.realtimeSinceStartup)
                {
                    _holdInputStartTime = Time.realtimeSinceStartup;
                    MoveLeft = true;
                }
            }

            // Is holding right arrow key
            MoveRight = false;
            if (!MoveLeft && Input.GetButton(_rightButtonName))
            {
                // first frame
                if (Input.GetButtonDown(_rightButtonName))
                {
                    MoveRight = true;
                    _holdInputStartTime = Time.realtimeSinceStartup;
                }
                else if (_holdInputStartTime + _holdInputMaxTime < Time.realtimeSinceStartup)
                {
                    _holdInputStartTime = Time.realtimeSinceStartup;
                    MoveRight = true;
                }
            }
        }
    }
}
