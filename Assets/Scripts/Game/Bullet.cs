using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour
    {      
        [Header("References")]
        [SerializeField] GameObject collisionEffect;
        [Header("Settings")]
        [SerializeField] float defaultDamage = 10f;
        [SerializeField] float lifetime = 5f;
        [SerializeField] float collisionEffectDuration = 0.5f;

        internal Action<Bullet> onRelease;

        private float _timer;
        private bool _isActive;

        private void Start()
        {
            DisableEffect();
        }

        private void OnEnable()
        {
            _isActive = true;
            SetBullet(true);          
        }

        private void Update()
        {
            if (_isActive == false)
                return;

            _timer += Time.deltaTime;

            if (_timer >= lifetime)
            {
                _timer = 0;
                _isActive = false;
                DisableEffect();
                onRelease?.Invoke(this);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collisionEffect != null)
            {
                if (collision.gameObject.IsPlayer())
                    collision.gameObject.GetComponent<Player.PlayerController>()?.Damage(defaultDamage);
                else if (collision.collider.CompareTag("Enemy"))
                    collision.gameObject.GetComponent<Mob.MobController>()?.Damage(defaultDamage);

                SetBullet(false);
                EnableEffect();
            }
        }

        internal float ChangeDamage(float value) => defaultDamage = value;

        internal void AddForce(Vector2 force, ForceMode2D mode) => GetComponent<Rigidbody2D>().AddForce(force, mode);

        private void SetBullet(bool isActive)
        {
            GetComponent<SpriteRenderer>().enabled = isActive;
            GetComponent<Rigidbody2D>().simulated = isActive;
            GetComponent<BoxCollider2D>().enabled = isActive;
        }

        private void EnableEffect()
        {
            collisionEffect.SetActive(true);
            _timer = lifetime - collisionEffectDuration;
        }
        private void DisableEffect() => collisionEffect.SetActive(false);
    }
}
