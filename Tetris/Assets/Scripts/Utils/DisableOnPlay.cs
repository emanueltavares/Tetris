using UnityEngine;

namespace Application.Utils
{
    public class DisableOnPlay : MonoBehaviour
    {
        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
