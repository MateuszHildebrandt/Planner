using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Tools
{
    public class AudioClipPool : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int poolDefaultCapacity = 5;
        [SerializeField] int poolMaxSize = 20;

        private static AudioClipPool _instance;
        public static AudioClipPool I
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AudioClipPool>();
                return _instance;
            }
        }

        private ObjectPool<AudioSource> _audioPool;

        private void Start()
        {
            _audioPool = new ObjectPool<AudioSource>(CreateAudioClip, OnGetAudio, OnReleaseAudio, OnDestroyAudio,
                collectionCheck: false, poolDefaultCapacity, poolMaxSize);
        }

        private AudioSource CreateAudioClip()
        {
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.parent = transform;
            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSource.spatialBlend = 0f;
            return audioSource;
        }

        private void OnGetAudio(AudioSource audioSource) => audioSource.gameObject.SetActive(true);
        private void OnReleaseAudio(AudioSource audioSource) => audioSource.gameObject.SetActive(false);
        private void OnDestroyAudio(AudioSource audioSource) => Destroy(audioSource.gameObject);

        internal void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, AudioMixerGroup mixer)
        {
            AudioSource audioSource = _audioPool.Get();
            audioSource.clip = clip;
            audioSource.transform.position = position;
            audioSource.volume = volume;
            audioSource.outputAudioMixerGroup = mixer;
            audioSource.Play();

            StartCoroutine(ReleaseCor(audioSource, clip.length * Time.timeScale));
        }

        private IEnumerator ReleaseCor(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            _audioPool.Release(audioSource);
        }
    }
}
