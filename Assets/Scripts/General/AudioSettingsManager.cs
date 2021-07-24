using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioSettings audioSettingSO;
    [SerializeField] private GameObject musicVolumeSlider;

    private float musicVolume;
    // Start is called before the first frame update
    void Start() {
        musicVolume = audioSettingSO.GetSettings();
        musicVolumeSlider.GetComponent<Slider>().value = musicVolume;
        Debug.Log(musicVolume);
    }

    public void Done() {
        musicVolume =  musicVolumeSlider.GetComponent<Slider>().value;
        audioSettingSO.SetSettings(musicVolume);
        GetComponent<BackgroundAudioPlayer>().UpdateVolumeSetting();
    }
}
