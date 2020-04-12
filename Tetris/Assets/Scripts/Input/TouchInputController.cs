using UnityEngine;

namespace Tetris.Controllers
{
    public class TouchInputController : MonoBehaviour, IInputController
    {
        // Serialize Fields
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Collider2D _holdCollider;
        [SerializeField] private float _minHorizontalDragDelta;
        [SerializeField] private float _minVerticalDragDelta;
        [SerializeField] private float _holdMinTime = 0.15f;
        [SerializeField] private float _dropHardMaxTime = 0.5f;

        // Private
        private IBoardController _boardController;
        private float _lastDragInputX;
        private float _lastDragInputY;
        private float? _touchStartTime = 0f;
        private bool _applicationLostFocus = false;

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
            if (_boardController == null)
            {
                _boardController = GetComponent<IBoardController>();
            }
        }

        protected virtual void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                _applicationLostFocus = true;
            }
        }

        protected virtual void Update()
        {
            MoveLeft = false;
            MoveRight = false;
            DropHard = false;
            RotateClockwise = false;
            RotateCounterClockwise = false;
            
            // Reset Application Lost Focus
            Pause = _applicationLostFocus;
            _applicationLostFocus = false;
            
            HoldPiece = false;

            if (!_boardController.IsPaused)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _touchStartTime = Time.realtimeSinceStartup;
                    OnBeginTouch();
                }
                else if (Input.GetMouseButton(0))
                {
                    if (_touchStartTime.HasValue) 
                    {
                        float elapsedTime = Time.realtimeSinceStartup - _touchStartTime.Value;
                        OnTouch(elapsedTime);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (_touchStartTime.HasValue)
                    {
                        float elapsedTime = Time.realtimeSinceStartup - _touchStartTime.Value;

                        OnEndTouch(elapsedTime);

                        _touchStartTime = null;
                    }
                }
            }            
        }

        private void OnBeginTouch()
        {
            Vector3 inputPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            _lastDragInputX = inputPosition.x;
            _lastDragInputY = inputPosition.y;
            DropSoft = false;
        }

        private void OnTouch(float touchElapsedTime)
        {            
            if (touchElapsedTime > _holdMinTime)
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
                        canDropHard = touchElapsedTime <= _dropHardMaxTime;
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

        private void OnEndTouch(float touchElapsedTime)
        {
            Vector3 inputViewportPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            if (touchElapsedTime <= _holdMinTime)
            {
                // Check hold
                Vector3 inputWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (_holdCollider.OverlapPoint(inputWorldPosition))
                {
                    HoldPiece = true;
                }
                else if (inputViewportPosition.x >= 0.1f && inputViewportPosition.x <= 0.9f)
                {
                    if (inputViewportPosition.x > 0.5f) // Check Rotate Clockwise
                    {
                        RotateClockwise = true;
                    }
                    else // Check Rotate Counter Clockwise
                    {
                        RotateCounterClockwise = true;
                    }
                }
                else
                {
                    Pause = true;
                }

            }
            else if (!DropSoft) // Check Drop Hard
            {
                float verDragDelta = inputViewportPosition.y - _lastDragInputY;
                if (verDragDelta < 0f && Mathf.Abs(verDragDelta) > _minVerticalDragDelta && touchElapsedTime <= _dropHardMaxTime)
                {
                    DropHard = true;
                }
            }


            DropSoft = false;
        }
    }
}