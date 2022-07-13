using Game;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Mob
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MobShooting : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] AudioClip audioEffect;
        [Header("Settings")]
        [SerializeField] AudioMixerGroup audioMixer;
        [SerializeField] float bulletForce = 10f;
        [SerializeField] float cooldown = 3f;

        private SpriteRenderer _spriteRenderer;
        private MobController _mobController;   
        private BulletsPool _bulletsPool;

        private float _bulletOffset = 0.01f;
        private float _timer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mobController = GetComponent<MobController>();
        }     

        private void Update()
        {
            if (_timer < cooldown)
                _timer += Time.deltaTime;
        }

        [Inject]
        private void Installer(BulletsPool bulletsPool)
        {
            _bulletsPool = bulletsPool;
        }

        internal void Shoot(float damage)
        {
            if (_timer > cooldown)
            {
                Vector2 bulletStart = SetBulletPosition();               

                Bullet bullet = _bulletsPool.Get();
                bullet.transform.position = bulletStart;
                bullet.ChangeDamage(damage);
                bullet.onRelease = (Bullet item) => _bulletsPool.Release(item);

                Vector2 direction = ((Vector2)_mobController.target.position - bulletStart).normalized;
                //Debug.DrawRay(bulletStart, direction, Color.red, 2);

                bullet.AddForce(direction * bulletForce, ForceMode2D.Impulse);
                Tools.SimpleAudio.PlayClipAtPoint(audioEffect, transform.position, 1, audioMixer);             
                _timer = 0;
            }
        }

        private Vector2 SetBulletPosition()
        {
            Bounds bounds = _spriteRenderer.bounds;
            float angleRad = Mathf.Atan2(transform.position.y - _mobController.target.position.y, transform.position.x - _mobController.target.position.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            //4 directions
            if (angleDeg >= 45 && angleDeg <= 135)
                return new Vector2(bounds.center.x, bounds.min.y - _bulletOffset); //Down
            else if (angleDeg >= -135 && angleDeg <= -45)
                return new Vector2(bounds.center.x + _bulletOffset, bounds.max.y); //Up
            else if (angleDeg > -45 && angleDeg < 45)
                return new Vector2(bounds.min.x - _bulletOffset, bounds.center.y); //Left
            else
                return new Vector2(bounds.max.x + _bulletOffset, bounds.center.y); //Right
        }
    }
}
