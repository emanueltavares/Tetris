﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tetris.Controllers
{
    public class ForceHighlight : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelected;
        [SerializeField] private EventSystem _eventSystem;

        protected virtual void OnEnable()
        {
            StartCoroutine(OnEnableCoroutine());
        }

        private IEnumerator OnEnableCoroutine()
        {
            // Button select does not highlight: https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html
            yield return null;

            _eventSystem.SetSelectedGameObject(null);
            _eventSystem.SetSelectedGameObject(_firstSelected);
        }
    }
}