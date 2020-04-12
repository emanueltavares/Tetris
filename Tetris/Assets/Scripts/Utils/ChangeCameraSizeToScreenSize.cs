using UnityEngine;

namespace Application.Utils
{
    /// <summary>
    /// Update Camera's orthographic size to screen size
    /// </summary>
    public class ChangeCameraSizeToScreenSize : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector2 _targetScreenSize = new Vector2(1920f, 1080f);

        private float _originalOrthoGraphicSize;

        protected virtual void Awake()
        {
            _originalOrthoGraphicSize = _camera.orthographicSize;
        }

        protected virtual void Update()
        {
           
            float targetAspectRatio = _targetScreenSize.x / _targetScreenSize.y;
            _camera.orthographicSize = Mathf.Max(targetAspectRatio / _camera.aspect, 1) * _originalOrthoGraphicSize;
        }
    }
}
