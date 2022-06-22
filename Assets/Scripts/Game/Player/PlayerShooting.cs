using UnityEngine;
using UnityEngine.Audio;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer), typeof(PlayerMovement))]
    public class PlayerShooting : MonoBehaviour, Game.IInputActionsReceiver
    {
        [Header("References")]
        [SerializeField] GameObject bulletPrefab;
        [Header("Resources")]
        [SerializeField] AudioClip audioEffect;
        [SerializeField] PlayerData playerData;
        [Header("Settings")]
        [SerializeField] AudioMixerGroup audioMixer;
        [SerializeField] float bulletForce = 10f;
        [SerializeField] float spellCost = 1f;

        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _playerMovement;
        private InputActions _inputActions;


        private float _bulletOffset = 0.01f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerMovement = GetComponent<PlayerMovement>();       
        }

        private void Start()
        {
            _inputActions.Player.Fire.performed += (_) => Shoot();
        }

        internal void Shoot()
        {          
            if (playerData.magic - spellCost >= 0)
            {
                playerData.ClampMagic(playerData.magic - spellCost);
                Vector2 bulletStart = SetBulletPosition();
                GameObject bullet = Instantiate(bulletPrefab, bulletStart, Quaternion.identity, transform);
                Rigidbody2D rigidbody2d = bullet.GetComponent<Rigidbody2D>();
                rigidbody2d.AddForce(SetForceDirection() * bulletForce, ForceMode2D.Impulse);
                Tools.SimpleAudio.PlayClipAtPoint(audioEffect, transform.position, 1, audioMixer);
                Destroy(bullet, 10f);
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
                return new Vector2(bounds.max.x + _bulletOffset, bounds.center.y);
            else if (direction.x < 0)
                return new Vector2(bounds.min.x - _bulletOffset, bounds.center.y);
            else if (direction.y > 0)
                return new Vector2(bounds.center.x + _bulletOffset, bounds.max.y);
            else
                return new Vector2(bounds.center.x, bounds.min.y - _bulletOffset);

        }

        public void SetupInputActions(InputActions input) => _inputActions = input;
    }
}