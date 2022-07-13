using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class BulletsPool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject bulletPrefab;
        [Header("Settings")]
        [SerializeField] int poolDefaultCapacity = 5;
        [SerializeField] int poolMaxSize = 10;

        private ObjectPool<Bullet> _bulletPool;

        private void Start()
        {
            _bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestoryBullet,
                collectionCheck: false, poolDefaultCapacity, poolMaxSize);
        }

        internal Bullet Get() => _bulletPool.Get();
        internal void Release(Bullet bullet) => _bulletPool.Release(bullet);

        private Bullet CreateBullet()
        {
            return Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Bullet>();
        }

        private void OnGetBullet(Bullet bullet) => bullet.gameObject.SetActive(true);

        private void OnReleaseBullet(Bullet bullet) => bullet.gameObject.SetActive(false);
        private void OnDestoryBullet(Bullet bullet) => Destroy(bullet.gameObject);
    }
}
