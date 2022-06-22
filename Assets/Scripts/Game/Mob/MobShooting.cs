using UnityEngine;
using UnityEngine.Audio;

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

        private SpriteRenderer spriteRenderer;
        private MobController mobController;   
        private float bulletOffset = 0.01f;
        private float timer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mobController = GetComponent<MobController>();
        }     

        private void Update()
        {
            if (timer < cooldown)
                timer += Time.deltaTime;
        }

        internal void Shoot(float damage)
        {
            if (timer > cooldown)
            {
                Vector2 bulletStart = SetBulletPosition();
                GameObject bullet = Instantiate(bulletPrefab, bulletStart, Quaternion.identity, transform);
                Rigidbody2D rigidbody2d = bullet.GetComponent<Rigidbody2D>();
                bullet.GetComponent<Game.Bullet>().ChangeDamage(damage);

                Vector2 direction = ((Vector2)mobController.target.position - bulletStart).normalized;
                //Debug.DrawRay(bulletStart, direction, Color.red, 2);
                rigidbody2d.AddForce(direction * bulletForce, ForceMode2D.Impulse);
                Tools.SimpleAudio.PlayClipAtPoint(audioEffect, transform.position, 1, audioMixer);
                Destroy(bullet, 10f);
                timer = 0;
            }
        }

        private Vector2 SetBulletPosition()
        {
            Bounds bounds = spriteRenderer.bounds;
            float angleRad = Mathf.Atan2(transform.position.y - mobController.target.position.y, transform.position.x - mobController.target.position.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            //4 directions
            if (angleDeg >= 45 && angleDeg <= 135)
                return new Vector2(bounds.center.x, bounds.min.y - bulletOffset); //Down
            else if (angleDeg >= -135 && angleDeg <= -45)
                return new Vector2(bounds.center.x + bulletOffset, bounds.max.y); //Up
            else if (angleDeg > -45 && angleDeg < 45)
                return new Vector2(bounds.min.x - bulletOffset, bounds.center.y); //Left
            else
                return new Vector2(bounds.max.x + bulletOffset, bounds.center.y); //Right
        }
    }
}
