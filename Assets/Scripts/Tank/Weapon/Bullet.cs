using System;
using UnityEngine;
namespace TanksSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private ParticleSystem _hitParticle;

        private Rigidbody2D _rb;

        private Tank _sender;

        public void Initialize(Vector2 moveDiretion, Tank sender)
        {
            _sender = sender;

            _rb = GetComponent<Rigidbody2D>();

            Move(moveDiretion * _speed);
        }

        public void Move(Vector2 velocity)
        {
            _rb.velocity = velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Tank tank) && tank == _sender)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                Instantiate(_hitParticle, transform.position, Quaternion.identity);

                Destroy(gameObject);

            }
        }
    }
}