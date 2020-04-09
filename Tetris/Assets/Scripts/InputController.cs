﻿using UnityEngine;

namespace Tetris.Controllers
{
    public class InputController : MonoBehaviour, IInputController
    {
        [SerializeField] private string _leftButtonName;
        [SerializeField] private string _rightButtonName;
        [SerializeField] private string _rotateClockwiseButtonName;
        [SerializeField] private string _rotateCounterClockwiseButtonName;

        // Private Fields
        private float _holdInputStartTime = 0f;

        // Properties
        public float HoldInputMaxTime { get; set; }                                  // max time in milliseconds that you can hold the direction buttons before moving the tetromino again
        public bool MoveLeft { get; private set; }
        public bool MoveRight { get; private set; }
        public bool RotateClockwise { get; private set; }
        public bool RotateCounterClockwise { get; private set; }        

        protected virtual void Update()
        {            
            RotateClockwise = Input.GetButtonDown(_rotateClockwiseButtonName);
            RotateCounterClockwise = Input.GetButtonDown(_rotateCounterClockwiseButtonName);

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
                else if (_holdInputStartTime + HoldInputMaxTime < Time.realtimeSinceStartup)
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
                else if (_holdInputStartTime + HoldInputMaxTime < Time.realtimeSinceStartup)
                {
                    _holdInputStartTime = Time.realtimeSinceStartup;
                    MoveRight = true;
                }
            }
        }
    }

    public interface IInputController
    {
        float HoldInputMaxTime { get; set; }
        bool MoveLeft { get; }
        bool MoveRight { get; }
        bool RotateCounterClockwise { get; }
        bool RotateClockwise { get; }
    }
}
