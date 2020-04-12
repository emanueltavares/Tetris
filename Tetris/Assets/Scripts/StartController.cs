using Application.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application.Controllers
{
    public class StartController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(TetrominoUtils.GameScene);
        }

        public void ShowCredits()
        {
            SceneManager.LoadScene(TetrominoUtils.CreditsScene);
        }

        public void QuitGame()
        {
            UnityEngine.Application.Quit();
        }
    }

}