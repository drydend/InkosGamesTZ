using System;
using System.Linq;
using UnityEngine;

namespace TanksSystem
{
    [RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
    public class Tank : MonoBehaviour, IDamageable
    {
        protected readonly float MovementOffset = 0.05f;

        [SerializeField]
        private LayerMask _raycastLayers;

        [SerializeField]
        private AudioClip _shootSound;

        [SerializeField]
        private AudioSource _source;

        protected TankConfig _config;

        protected Weapon _weapon;
        protected Vector2 _gazeDirection = Vector2.right;

        protected bool _isPaused = false;

        protected float _reloadTime;

        private Rigidbody2D _rigidbody;
        private bool _isDestroyed;

        public event Action OnDie;

        public int Heals { get; private set; }

        public void Initialize(TankConfig config, Weapon weapon)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _source = GetComponent<AudioSource>();

            _config = config;
            _weapon = weapon;

            Heals = _config.MaxHeals;
        }

        public virtual void Move(Vector2 direction)
        {
            var distance = _config.MovementSpeed * Time.deltaTime;

            if (direction == Vector2.zero)
            {
                return;
            }
            
            _gazeDirection = direction;
            RotateInDirection(direction.normalized);

            if (!CanMove(direction, distance))
            {
                return;
            }

            transform.Translate(direction * distance, Space.World);

            var collisions = Physics2D.RaycastAll(transform.position, direction, _config.TankSize / 2 + distance, _raycastLayers);

            if (collisions.Length > 1)
            {
                var hitWithOtherCollider = collisions.FirstOrDefault(x => x.collider.gameObject != gameObject);
                transform.position = hitWithOtherCollider.point + new Vector2(_config.TankSize / 2, _config.TankSize / 2) * hitWithOtherCollider.normal;
            }
        }

        public void RotateInDirection(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public virtual void TakeDamage(int damage)
        {
            Heals -= damage;

            if (Heals == 0)
            {
                Die();
            }
        }

        public void DestroyTank()
        {
            if (_isDestroyed)
            {
                return;
            }

            _isDestroyed = true;
            Destroy(gameObject);
        }

        public virtual void TryShoot()
        {
            if (_reloadTime <= 0)
            {
                _source.PlayOneShot(_shootSound);
                _weapon.Shoot(transform.position, _gazeDirection, this);
                _reloadTime = _config.ReloadingTime;
            }
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        protected bool CanMove(Vector2 direction, float distance)
        {
            var boxSize = new Vector2(MovementOffset, _config.TankSize - MovementOffset);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + direction / 2 * _config.TankSize + direction * distance, boxSize,
                angle, _raycastLayers);

            return collisions.Length <= 1;
        }

        protected virtual void Die()
        {
            OnDie?.Invoke();
        }
    }
}
