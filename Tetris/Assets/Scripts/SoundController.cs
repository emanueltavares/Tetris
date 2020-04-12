using System.Collections;
using System.Collections.Generic;
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
    }

    public interface ISoundController
    {
        void PlayClearLine();
        void PlayMoveTetromino();
        void PlayRotateTetromino();
        void PlayPlaceTetromino();
        void PlayHoldTetromino();
    }
}