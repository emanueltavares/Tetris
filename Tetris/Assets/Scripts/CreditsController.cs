using Application.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application.Controllers
{
    public class CreditsController : MonoBehaviour
    {
        public void GoToMenu()
        {
            SceneManager.LoadScene(TetrominoUtils.StartScene);
        }
    }

}