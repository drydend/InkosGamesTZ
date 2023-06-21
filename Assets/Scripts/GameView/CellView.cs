using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameView
{
    public class CellView : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private bool _isBreackable;
        [SerializeField]
        private int _maxDurability;

        private int _currentDurability;

        public event Action<CellView> OnDestroyed;

        public void TakeDamage(int damage)
        {
            if (!_isBreackable)
            {
                return;
            }

            _currentDurability -= damage;

            if(_currentDurability <= 0)
            {
                OnDestroyed?.Invoke(this);
            }

            Destroy(gameObject);
        }
    }
}