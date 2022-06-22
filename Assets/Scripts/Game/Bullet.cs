using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject collisionEffect;
        [Header("Settings")]
        [SerializeField] float defaultDamage = 10f;

        internal float ChangeDamage(float value) => defaultDamage = value;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collisionEffect != null)
            {
                if (collision.collider.CompareTag("Player"))
                    collision.gameObject.GetComponent<Player.PlayerController>()?.Damage(defaultDamage);
                else if (collision.collider.CompareTag("Enemy"))
                    collision.gameObject.GetComponent<Mob.MobController>()?.Damage(defaultDamage);

                GameObject effect = Instantiate(collisionEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
            }

            Destroy(gameObject);
        }
    }
}
