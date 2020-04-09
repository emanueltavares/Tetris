using UnityEngine;

namespace Tetris.Controllers
{
    public class InputController : MonoBehaviour, IInputController
    {
        [SerializeField] private string _leftButtonName;
        [SerializeField] private string _rightButtonName;

        // Private Fields
        private float _holdLeftStartTime = 0f;
        private float _holdRightStartTime = 0f;

        public bool CanMoveLeft { get; private set; }
        public bool CanMoveRight { get; private set; }
        public float HoldDirectionMaxTime { get; set; }          // max time in milliseconds that you can hold the direction buttons before moving the tetromino again

        protected virtual void Update()
        {
            CanMoveLeft = false;
            CanMoveRight = false;

            // Is holding left arrow key
            if (Input.GetButton(_leftButtonName))
            {
                // first frame
                if (Input.GetButtonDown(_leftButtonName))
                {
                    CanMoveLeft = true;
                    _holdLeftStartTime = Time.realtimeSinceStartup;
                }
                else if (_holdLeftStartTime + HoldDirectionMaxTime < Time.realtimeSinceStartup)
                {
                    _holdLeftStartTime = Time.realtimeSinceStartup;
                    CanMoveLeft = true;
                }
            }

            // Is holding right arrow key
            if (Input.GetButton(_rightButtonName))
            {
                // first frame
                if (Input.GetButtonDown(_rightButtonName))
                {
                    CanMoveRight = true;
                    _holdRightStartTime = Time.realtimeSinceStartup;
                }
                else if (_holdRightStartTime + HoldDirectionMaxTime < Time.realtimeSinceStartup)
                {
                    _holdRightStartTime = Time.realtimeSinceStartup;
                    CanMoveRight = true;
                }
            }
        }
    }

    public interface IInputController
    {
        float HoldDirectionMaxTime { get; set; }
        bool CanMoveLeft { get; }
        bool CanMoveRight { get; }
    }
}
