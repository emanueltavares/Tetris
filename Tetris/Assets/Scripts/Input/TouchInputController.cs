using UnityEngine;

namespace Tetris.Controllers
{
    public class TouchInputController : MonoBehaviour, IInputController
    {
        // Serialize Fields
        [SerializeField] private Camera _mainCamera;
        [Range(0f, 0.1f)] [SerializeField] private float _minHorizontalDragDelta;
        [Range(0f, 0.1f)] [SerializeField] private float _minVerticalDragDelta;
        [SerializeField] private float _holdMinTime = 0.15f;
        [SerializeField] private float _dropHardMaxTime = 0.5f;

        // Private
        private float _lastDragInputX;
        private float _lastDragInputY;
        private float _touchStartTime = 0f;

        // Properties
        public bool MoveLeft { get; private set; }
        public bool MoveRight { get; private set; }
        public bool RotateClockwise { get; private set; }
        public bool RotateCounterClockwise { get; private set; }
        public bool DropHard { get; private set; }
        public bool DropSoft { get; private set; }
        public bool HoldPiece { get; private set; }
        public bool Pause { get; private set; }

        protected virtual void Update()
        {
            MoveLeft = false;
            MoveRight = false;
            DropHard = false;
            RotateClockwise = false;
            RotateCounterClockwise = false;

            Pause = Input.GetButtonDown("Cancel");
            HoldPiece = Input.GetButtonDown("Hold");

            if (Input.GetMouseButtonDown(0))
            {
                OnBeginTouch();
            }
            else if (Input.GetMouseButton(0))
            {
                OnTouch();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnEndTouch();
            }
        }

        private void OnBeginTouch()
        {
            Vector3 inputPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            _lastDragInputX = inputPosition.x;
            _lastDragInputY = inputPosition.y;
            _touchStartTime = Time.realtimeSinceStartup;
            DropSoft = false;
        }

        private void OnTouch()
        {
            float elapsedTime = Time.realtimeSinceStartup - _touchStartTime;
            if (elapsedTime > _holdMinTime)
            {
                Vector3 inputPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
                float horDragDelta = inputPosition.x - _lastDragInputX;
                float verDragDelta = inputPosition.y - _lastDragInputY;
                bool canDropHard = false;

                // Check Drop Soft
                if (!DropSoft)
                {
                    if (verDragDelta < 0f && Mathf.Abs(verDragDelta) > _minVerticalDragDelta)
                    {
                        canDropHard = elapsedTime <= _dropHardMaxTime;
                        if (!canDropHard)
                        {
                            DropSoft = true;
                            _lastDragInputY = inputPosition.y;
                        }
                    }
                }

                // Check Move Right and Left
                if (!canDropHard && Mathf.Abs(horDragDelta) > _minHorizontalDragDelta)
                {
                    if (horDragDelta > 0)
                    {
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                    }

                    _lastDragInputX = inputPosition.x;
                }

                Debug.LogFormat("Horizontal Drag Delta: [{0}] - Vertical Drag Delta: [{0}]", horDragDelta, verDragDelta);
            }
        }

        private void OnEndTouch()
        {
            Vector3 inputPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            float elapsedTime = Time.realtimeSinceStartup - _touchStartTime;
            if (elapsedTime <= _holdMinTime)
            {
                if (inputPosition.x > 0.5f)
                {
                    RotateClockwise = true;
                }
                else
                {
                    RotateCounterClockwise = true;
                }
            }
            else if (!DropSoft) // Check Drop Hard
            {
                float verDragDelta = inputPosition.y - _lastDragInputY;
                if (verDragDelta < 0f && Mathf.Abs(verDragDelta) > _minVerticalDragDelta && elapsedTime <= _dropHardMaxTime)
                {
                    DropHard = true;
                }
            }


            DropSoft = false;
        }
    }
}