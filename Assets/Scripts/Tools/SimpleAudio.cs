using UnityEngine;
using UnityEngine.Audio;

namespace Tools
{
    public static class SimpleAudio
    {
        public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, AudioMixerGroup mixer)
        {
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            audioSource.clip = clip;
            audioSource.spatialBlend = 0f;
            audioSource.volume = volume;
            audioSource.outputAudioMixerGroup = mixer;
            audioSource.Play();
            Object.Destroy(gameObject, clip.length * Time.timeScale);
        }
    }
}
