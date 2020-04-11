using UnityEngine;

namespace Tetris.Controllers
{
    public class LevelController : MonoBehaviour, ILevelController
    {
        // Serialize Fields
        [Header("Gameplay")]
        [SerializeField] private int _startLevel = 0;
        [SerializeField] private float _startGravityInterval = 1f;                                  // seconds
        [Range(0f, 1f)] [SerializeField] private float _dropSoftGravityMultiplier = 0.1f;

        [Header("UI")]
        [SerializeField] private TMPro.TextMeshPro _levelText;
        [SerializeField] private TMPro.TextMeshPro _clearedLinesText;
        [SerializeField] private TMPro.TextMeshPro _totalScore;

        // Constants
        private const int BaseScore = 40;

        // Properties
        public float GravityInterval { get; private set; }
        public float DropSoftGravityInterval => GravityInterval * _dropSoftGravityMultiplier;
        public int TotalClearedLines { get; private set; }
        public int TotalScore { get; private set; }
        public int Level { get; private set; }

        protected virtual void OnEnable()
        {
            TotalScore = 0;
            TotalClearedLines = 0;
            Level = Mathf.FloorToInt(TotalClearedLines / 10f) + _startLevel;
            GravityInterval = _startGravityInterval / (Level + 1);

            _levelText.text = Level.ToString();
            _clearedLinesText.text = TotalClearedLines.ToString();
            _totalScore.text = TotalScore.ToString();
        }

        public void AddClearedLines(int clearedLines)
        {
            TotalClearedLines += clearedLines;

            Level = Mathf.FloorToInt(TotalClearedLines / 10f) + _startLevel;

            int score = (BaseScore * clearedLines) * (Level + 1);
            TotalScore += score;

            _levelText.text = Level.ToString();
            _clearedLinesText.text = TotalClearedLines.ToString();
            _totalScore.text = TotalScore.ToString();

            GravityInterval = _startGravityInterval / (Level + 1);
        }
    }

    public interface ILevelController
    {
        float GravityInterval { get; }
        float DropSoftGravityInterval { get; }
        int TotalClearedLines { get; }
        int TotalScore { get; }
        int Level { get; }
        void AddClearedLines(int clearedLines);
    }
}