using System;
using UnityEngine;

namespace TanksSystem
{
    public class Weapon
    {
        private Bullet _bulletPrefab;

        public Weapon(Bullet bullet)
        {
            _bulletPrefab = bullet;
        }

        public virtual void Shoot(Vector2 origin, Vector2 direction, Tank sender)
        {
            var bullet = UnityEngine.Object.Instantiate(_bulletPrefab, origin, Quaternion.identity);
            bullet.Initialize(direction, sender);
        }
    }
}