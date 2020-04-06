using Tetris.Factories;
using Tetris.Models;
using UnityEngine;

namespace Tetris.Controllers
{
    public partial class BoardController : MonoBehaviour
    {
        [SerializeField] private int _maxNumLines;      // max number of lines of our board
        [SerializeField] private int _maxNumColumns;    // max number of columns of our board
        [SerializeField] private BoardFactory _boardFactory;


        protected virtual void OnEnable()
        {
            Create();
        }

        private void Create()
        {
            IBoardView boardView = _boardFactory.GetBoard(_maxNumLines, _maxNumColumns);            
        }
    }
}