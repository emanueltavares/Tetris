using UnityEngine;

namespace Application.Controllers
{
    public class TouchInputController : MonoBehaviour, IInputController
    {
        // Serialize Fields
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Collider2D _boardCollider;
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
        private bool _hasMovedTetromino = false;

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
            DropHard = false;

            // Reset Application Lost Focus
            Pause = _applicationLostFocus;
            _applicationLostFocus = false;

            HoldPiece = false;

            if (!_boardController.IsPaused)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _touchStartTime = Time.realtimeSinceStartup;
                    _hasMovedTetromino = false;

                    OnBeginTouch();
                }
                else if (Input.GetMouseButton(0) && _touchStartTime.HasValue)
                {
                    OnTouch();
                }
                else if (Input.GetMouseButtonUp(0) && _touchStartTime.HasValue)
                {
                    OnEndTouch();
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

        private void OnTouch()
        {
            Vector3 inputPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            float horDragDelta = inputPosition.x - _lastDragInputX;
            float verDragDelta = inputPosition.y - _lastDragInputY;

            // Check Drop Soft
            if (!DropSoft && verDragDelta < 0f && Mathf.Abs(verDragDelta) > _minVerticalDragDelta)
            {
                DropSoft = true;
                _lastDragInputY = inputPosition.y;
            }

            // Check Move Right and Left
            if (!DropSoft && Mathf.Abs(horDragDelta) > _minHorizontalDragDelta)
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

        private void OnEndTouch()
        {
            float elapsedTime = Time.realtimeSinceStartup - _touchStartTime.Value;
            Vector3 inputViewportPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            if (elapsedTime <= _holdMinTime)
            {
                // Check hold
                Vector3 inputWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (_holdCollider.OverlapPoint(inputWorldPosition))
                {
                    HoldPiece = true;
                }
                else if (_boardCollider.OverlapPoint(inputWorldPosition))
                {
                    if (DropSoft)
                    {
                        DropHard = true;
                    }
                    else
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
                }
                else
                {
                    Pause = true;
                }

            }

            _touchStartTime = null;
            DropSoft = false;
        }
    }
}