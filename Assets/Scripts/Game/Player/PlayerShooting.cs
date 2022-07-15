using Game;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer), typeof(PlayerMovement))]
    public class PlayerShooting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject bulletPrefab;
        [Header("Resources")]
        [SerializeField] AudioClip audioEffect;
        [SerializeField] PlayerData playerData;
        [Header("Settings")]
        [SerializeField] AudioMixerGroup audioMixer;
        [SerializeField] float damage = 10f;
        [SerializeField] float bulletForce = 10f;
        [SerializeField] float spellCost = 1f;

        private const float BULLET_OFFSET = 0.01f;

        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _playerMovement;
        private InputActions _inputActions;

        private BulletsPool _bulletsPool;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            _inputActions.Player.Fire.performed += (_) => Shoot();
        }

        [Inject]
        private void Construct(InputActions inputActions, BulletsPool bulletsPool)
        {
            _inputActions = inputActions;
            _bulletsPool = bulletsPool;
        }

        internal void Shoot()
        {          
            if (playerData.magic - spellCost >= 0)
            {
                playerData.ClampMagic(playerData.magic - spellCost);

                Bullet bullet = _bulletsPool.Get();
                bullet.transform.position = SetBulletPosition();
                bullet.ChangeDamage(damage);
                bullet.onRelease = (Bullet item) => _bulletsPool.Release(item);

                bullet.AddForce(SetForceDirection() * bulletForce, ForceMode2D.Impulse);
                Tools.AudioClipPool.I.PlayClipAtPoint(audioEffect, transform.position, 1, audioMixer);
            }
        }

        private Vector2 SetForceDirection()
        {
            Vector2 direction = _playerMovement.GetDirection();
            if (direction == Vector2.zero)
                return Vector2.down;
            else
                return new Vector2(CustomCeil(direction.x), CustomCeil(direction.y));

            float CustomCeil(float value) => Mathf.Sign(value) * Mathf.Ceil(Mathf.Abs(value));
        }

        private Vector2 SetBulletPosition()
        {
            Vector2 direction = _playerMovement.GetDirection();
            Bounds bounds = _spriteRenderer.bounds;

            //4 directions
            if (direction.x > 0)
                return new Vector2(bounds.max.x + BULLET_OFFSET, bounds.center.y);
            else if (direction.x < 0)
                return new Vector2(bounds.min.x - BULLET_OFFSET, bounds.center.y);
            else if (direction.y > 0)
                return new Vector2(bounds.center.x + BULLET_OFFSET, bounds.max.y);
            else
                return new Vector2(bounds.center.x, bounds.min.y - BULLET_OFFSET);

        }

        public void SetupInputActions(InputActions input) => _inputActions = input;
    }
}
