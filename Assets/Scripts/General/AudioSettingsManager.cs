using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSettings audioSettingSO;
    [SerializeField] private BackgroundAudioPlayer backgroundAudioPlayer;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider playerVolumeSlider;
    [SerializeField] private Slider enemyVolumeSlider;
    [SerializeField] private Slider environmentVolumeSlider;

    private float musicVolume;
    private float playerSoundVolume;
    private float enemySoundVolume;
    private float environmentSoundVolume;
    // Start is called before the first frame update
    void Start() {
        Dictionary<AudioSettingsEnum, float> settings = new Dictionary<AudioSettingsEnum, float>();
        settings = audioSettingSO.GetSettings();

        musicVolume = settings[AudioSettingsEnum.musicVolume];
        playerSoundVolume = settings[AudioSettingsEnum.playerSoundVolume];
        enemySoundVolume = settings[AudioSettingsEnum.enemySoundVolume];
        environmentSoundVolume = settings[AudioSettingsEnum.environmentSoundVolume];

        musicVolumeSlider.value = musicVolume;
        playerVolumeSlider.value = playerSoundVolume;
        enemyVolumeSlider.value = enemySoundVolume;
        environmentVolumeSlider.value = environmentSoundVolume;

        this.gameObject.SetActive(false);
    }

    public void Done() {
        if(CheckNewSettings(ref musicVolume, musicVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.musicVolume, musicVolume);
            backgroundAudioPlayer.GetComponent<BackgroundAudioPlayer>().UpdateVolumeSetting();
        }
        if(CheckNewSettings(ref playerSoundVolume, playerVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.playerSoundVolume, playerSoundVolume);
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioPlayer>().UpdateAudioSettings();
        }
        if(CheckNewSettings(ref enemySoundVolume, enemyVolumeSlider)) {
            GameObject[] enemies;
            audioSettingSO.SetSettings(AudioSettingsEnum.enemySoundVolume, enemySoundVolume);
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemies) {
                enemy.GetComponent<AudioPlayer>().UpdateAudioSettings();
            }
        }
        if(CheckNewSettings(ref environmentSoundVolume, environmentVolumeSlider)) {
            GameObject[] items;
            GameObject[] doors;
            audioSettingSO.SetSettings(AudioSettingsEnum.environmentSoundVolume, environmentSoundVolume);
            items = GameObject.FindGameObjectsWithTag("Item");
            doors = GameObject.FindGameObjectsWithTag("Door");

            foreach (var item in items) {
                if(item.GetComponent<AudioPlayer>() != null) {
                    item.GetComponent<AudioPlayer>().UpdateAudioSettings();
                }
                if(item.GetComponent<IUpdateAudioSettings>() != null) {
                    item.GetComponent<IUpdateAudioSettings>().UpdateAudioSettings(environmentSoundVolume);
                }
            }

            foreach (var door in doors) {
                if(door.GetComponent<AudioPlayer>() != null) {
                    door.GetComponent<AudioPlayer>().UpdateAudioSettings();
                }
            }
        }
    }

    public void SaveSettings() {
        if(CheckNewSettings(ref musicVolume, musicVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.musicVolume, musicVolume);
            backgroundAudioPlayer.GetComponent<BackgroundAudioPlayer>().UpdateVolumeSetting();
        }
        if(CheckNewSettings(ref playerSoundVolume, playerVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.playerSoundVolume, playerSoundVolume);
        }
        if(CheckNewSettings(ref enemySoundVolume, enemyVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.enemySoundVolume, enemySoundVolume);
        }
        if(CheckNewSettings(ref environmentSoundVolume, environmentVolumeSlider)) {
            audioSettingSO.SetSettings(AudioSettingsEnum.environmentSoundVolume, environmentSoundVolume);
        }
    }

    public void CloseSettings() {
        musicVolumeSlider.value = musicVolume;
        playerVolumeSlider.value = playerSoundVolume;
        enemyVolumeSlider.value = enemySoundVolume;
        environmentVolumeSlider.value = environmentSoundVolume;
    }

    private bool CheckNewSettings(ref float currentVolume, Slider slider) {
        float sliderValue = slider.value;

        if(currentVolume != sliderValue) {
            currentVolume = sliderValue;
            return true;
        } else {
            return false;
        }
    }
}
