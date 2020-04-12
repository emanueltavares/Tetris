using System.Collections;
using System.Collections.Generic;
using Tetris.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.Controllers
{
    public class CreditsController : MonoBehaviour
    {
        public void GoToMenu()
        {
            SceneManager.LoadScene(TetrominoUtils.StartScene);
        }
    }

}