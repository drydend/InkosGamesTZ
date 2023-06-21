using GameLogic;
using System;
using TMPro;
using UnityEngine;
using Utils;

namespace GameUI
{
    public class DecreasingCounterUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmpText;
        [SerializeField]
        private string _text;

        private int _initialValue;
        private Counter _counter;

        public void Initialize(int initialValue, Counter counter)
        {
            _initialValue = initialValue;
            _counter = counter;

            _counter.OnValueChanged += UpdateUI;
            UpdateUI();
        }

        private void OnEnable()
        {   
            if(_counter == null)
            {
                return;
            }

            _counter.OnValueChanged += UpdateUI;
            UpdateUI();
        }

        private void OnDisable()
        {
            _counter.OnValueChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            if (_counter == null)
            {
                return;
            }

            _tmpText.text = _text + (_initialValue - _counter.Value);
        }
    }
}