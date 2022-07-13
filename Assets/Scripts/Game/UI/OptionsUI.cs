using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoUI
    {
        [Header("References")]
        [SerializeField] Slider mainVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider effectsVolumeSlider;
        [SerializeField] TMP_Dropdown resolutionDropdown;
        [SerializeField] TMP_Dropdown qualityDropdown;
        [SerializeField] Toggle fullscreenToggle;
        [Header("Resources")]
        [SerializeField] AudioMixer audioMixer;

        private const string _MAIN_VOLUME = "VolumeMaster";
        private const string _MUSIC_VOLUME = "VolumeMusic";
        private const string _EFFECTS_VOLUME = "VolumeEffects";

        private Dictionary<int, Resolution> _supportedResolutions;

        private void SetupResolutionDropdown()
        {
            _supportedResolutions = new Dictionary<int, Resolution>();
            Resolution[] resolutions = Screen.resolutions;
            int counter = 0;

            foreach (Resolution item in resolutions)
            {
                float aspect = (float)item.width / item.height;
                if (aspect > 1.5f && aspect < 1.8f)                
                {
                    resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = $"{item.width}x{item.height}, {item.refreshRate}" });
                    if (Screen.width == item.width && Screen.height == item.height)
                        resolutionDropdown.value = counter;

                    _supportedResolutions.Add(counter, item);
                    counter++;
                }
            }
        }

        #region OnClick
        public void OnChangeMainVolume(float value) => audioMixer.SetFloat(_MAIN_VOLUME, value);
        public void OnChangeMusicVolume(float value) => audioMixer.SetFloat(_MUSIC_VOLUME, value);
        public void OnChangeEffectsVolume(float value) => audioMixer.SetFloat(_EFFECTS_VOLUME, value);

        public void OnChangeResolution(int value)
        {
            if (_supportedResolutions == null)
                return;
            if (value >= _supportedResolutions.Count)
                return;

            Resolution resolution = _supportedResolutions[value];

            if (Screen.currentResolution.width == resolution.width && Screen.currentResolution.height == resolution.height)
                return;

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        }

        public void OnChangeQuality(int value)
        {
            if (QualitySettings.GetQualityLevel() == value)
                return;
            QualitySettings.SetQualityLevel(value);
        }

        public void OnChangeFullscreen(bool value)
        {
            if (Screen.fullScreen == value)
                return;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value);
        }
        #endregion

        #region StateMachine
        public override void OnEnter()
        {
            base.OnEnter();

            if (_supportedResolutions == null)
            {
                audioMixer.GetFloat(_MAIN_VOLUME, out float mainVolume);
                audioMixer.GetFloat(_MUSIC_VOLUME, out float musicVolume);
                audioMixer.GetFloat(_EFFECTS_VOLUME, out float effectsVolume);
                mainVolumeSlider.value = mainVolume;
                musicVolumeSlider.value = musicVolume;
                effectsVolumeSlider.value = effectsVolume;

                SetupResolutionDropdown();
                qualityDropdown.value = QualitySettings.GetQualityLevel();
                fullscreenToggle.isOn = Screen.fullScreen;
            }
        }
        #endregion
    }
}
