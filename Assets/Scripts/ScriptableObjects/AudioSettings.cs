using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioSettings : ScriptableObject
{
    [SerializeField] private float musicVolume;
    [SerializeField] private float playerSoundVolume;
    [SerializeField] private float enemySoundVolume;
    [SerializeField] private float environmentSoundVolume;

    public Dictionary<AudioSettingsEnum, float> GetSettings() {
        Dictionary<AudioSettingsEnum, float> settings = new Dictionary<AudioSettingsEnum, float>() {
            {AudioSettingsEnum.musicVolume, musicVolume},
            {AudioSettingsEnum.playerSoundVolume, playerSoundVolume},
            {AudioSettingsEnum.enemySoundVolume, enemySoundVolume},
            {AudioSettingsEnum.environmentSoundVolume, environmentSoundVolume}
        };

        return settings;
    }
    public float GetSettings(AudioSettingsEnum nameSettings) {
        switch (nameSettings) {
            case AudioSettingsEnum.musicVolume:
                return musicVolume;
            case AudioSettingsEnum.playerSoundVolume:
                return playerSoundVolume;
            case AudioSettingsEnum.enemySoundVolume:
                return enemySoundVolume;
            case AudioSettingsEnum.environmentSoundVolume:
                return environmentSoundVolume;
            default:
                return 1f;
        }
    }
    public void SetSettings(Dictionary<AudioSettingsEnum, float> settings) {
        this.musicVolume = settings[AudioSettingsEnum.musicVolume];
        this.playerSoundVolume = settings[AudioSettingsEnum.playerSoundVolume];
        this.enemySoundVolume = settings[AudioSettingsEnum.enemySoundVolume];
        this.environmentSoundVolume = settings[AudioSettingsEnum.environmentSoundVolume];
    }
    public void SetSettings(AudioSettingsEnum settingsName, float volume) {
        switch (settingsName) {
            case AudioSettingsEnum.musicVolume:
                musicVolume = volume;
                break;
            case AudioSettingsEnum.playerSoundVolume:
                playerSoundVolume = volume;
                break;
            case AudioSettingsEnum.enemySoundVolume:
                enemySoundVolume = volume;
                break;
            case AudioSettingsEnum.environmentSoundVolume:
                environmentSoundVolume = volume;
                break;
        }
    }
}
