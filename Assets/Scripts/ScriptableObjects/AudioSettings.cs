using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioSettings : ScriptableObject
{
    [SerializeField] private float musicVolume;

    public float GetSettings() {
        return musicVolume;
    }
    public void SetSettings(float volume) {
        this.musicVolume = volume;
    }
}
