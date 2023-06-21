using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils;

namespace GameUI
{
    public class ScoreCounterUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmpText;
        [SerializeField]
        private string _text;
        [SerializeField]
        private float _animTime;

        private Counter _counter;
        private Coroutine _coroutine;

        public void Initialize( Counter counter)
        {
            _counter = counter;
        }

        private void OnEnable()
        {
            if (_counter == null)
            {
                return;
            }

            ShowValue();
        }

        private void ShowValue()
        {
            if (_counter == null)
            {
                return;
            }

            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ShowCoroutine());
        }

        private IEnumerator ShowCoroutine()
        {
            float timeElapsecNormalized = 0;

            while (timeElapsecNormalized != 1)
            {
                _tmpText.text = _text + (int)Mathf.Lerp(0, _counter.Value, timeElapsecNormalized);

                timeElapsecNormalized = Mathf.Clamp(timeElapsecNormalized + Time.deltaTime, 0, 1);
                yield return null;
            }

            _tmpText.text = _text + _counter.Value;
        }
    }
}
