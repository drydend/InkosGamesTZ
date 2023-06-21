using System;
using UnityEngine;
using Utils;

namespace TanksSystem
{
    public class EnemyTank : Tank
    {
        private Path _path;

        public int ScoreForKill { get; private set; }
        public event Action<EnemyTank> OnDie;

        public void Initialize(EnemyTankConfig tank, Weapon weapon)
        {
            ScoreForKill = tank.ScoreForKill;
            _reloadTime = tank.ReloadingTime;
            base.Initialize(tank, weapon);
        }

        public void SetPath(Path path)
        {
            _path = path;

            foreach (var item in _path._points)
            {
                Debug.Log(item);
            }
        }

        protected override void Die()
        {
            OnDie?.Invoke(this);
            Destroy(gameObject);
        }

        private void Update()
        {
            if (_isPaused)
            {
                return;
            }

            if (_reloadTime >= 0)
            {
                _reloadTime -= Time.deltaTime;
            }

            TryShoot();
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            if (_path.IsAtEnd)
            {
                return;
            }

            _gazeDirection = (_path.DesiredPoint - (Vector2)transform.position).normalized;
            RotateInDirection(_gazeDirection);

            if (CanMove(_path.DesiredPoint - (Vector2)transform.position, _config.MovementSpeed * Time.deltaTime))
            {
                var newPosition = Vector2.MoveTowards(transform.position, _path.DesiredPoint, _config.MovementSpeed * Time.deltaTime);
                transform.position = newPosition;

                if (Vector2.Distance( _path.DesiredPoint, newPosition) < MovementOffset)
                {   
                    transform.position = _path.DesiredPoint;
                    
                    _path.MoveNext();
                    return;
                }
            }
        }
    }
}