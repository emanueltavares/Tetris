using Tetris.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.Controllers
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
            Application.Quit();
        }
    }

}