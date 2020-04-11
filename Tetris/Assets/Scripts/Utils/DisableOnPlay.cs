using UnityEngine;

namespace Tetris.Utils
{
    public class DisableOnPlay : MonoBehaviour
    {
        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
