using Tetris.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.Controllers
{
    public class PlayController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(TetrominoUtils.GameScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }

}