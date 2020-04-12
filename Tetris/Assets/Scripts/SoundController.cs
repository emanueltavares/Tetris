using UnityEngine;

namespace Application.Controllers
{
    public class SoundController : MonoBehaviour, ISoundController
    {
        [SerializeField] private AudioSource _clearLineAudioSource;
        [SerializeField] private AudioSource _moveTetrominoAudioSource;
        [SerializeField] private AudioSource _placeTetrominoAudioSource;
        [SerializeField] private AudioSource _rotateTetrominoAudioSource;
        [SerializeField] private AudioSource _holdTetrominoAudioSource;
        [SerializeField] private AudioSource _bgMusicAudioSource;
        [Range(-3f, 3f)] [SerializeField] private float _bgMusicFastPitch = 1.15f;
        [Range(-3f, 3f)] [SerializeField] private float _bgMusicDefaultPitch = 1f;

        public void PlayClearLine()
        {
            _clearLineAudioSource.Play();
        }

        public void PlayHoldTetromino()
        {
            _holdTetrominoAudioSource.Play();
        }

        public void PlayMoveTetromino()
        {
            _moveTetrominoAudioSource.Play();
        }

        public void PlayPlaceTetromino()
        {
            _placeTetrominoAudioSource.Play();
        }

        public void PlayRotateTetromino()
        {
            _rotateTetrominoAudioSource.Play();
        }

        public void SetFastBgMusicPitch()
        {
            _bgMusicAudioSource.pitch = _bgMusicFastPitch;
        }

        public void SetDefaultBgMusicPitch()
        {
            _bgMusicAudioSource.pitch = _bgMusicDefaultPitch;
        }
    }

    public interface ISoundController
    {
        void PlayClearLine();
        void PlayMoveTetromino();
        void PlayRotateTetromino();
        void PlayPlaceTetromino();
        void PlayHoldTetromino();
        void SetFastBgMusicPitch();
        void SetDefaultBgMusicPitch();
    }
}