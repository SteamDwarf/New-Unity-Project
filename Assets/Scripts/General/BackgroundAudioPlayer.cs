using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSettings audioSettingSO;
    [SerializeField] private List<AudioClip> audioClips;

    private AudioSource audioSource;
    private List<int> playedAudio;
    private float musicVolume;
    void Start() {
        int audioInd = Random.Range(0, audioClips.Count);

        playedAudio = new List<int>();
        playedAudio.Add(audioInd);

        musicVolume = audioSettingSO.GetSettings(AudioSettingsEnum.musicVolume);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicVolume;
        audioSource.clip = audioClips[audioInd];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update() {
        if(audioSource.isPlaying) {
            return;
        }

        int audioInd;

        if(playedAudio.Count == audioClips.Count) {
            playedAudio = new List<int>();
        }

        do {
            audioInd = Random.Range(0, audioClips.Count);
        } while (playedAudio.Contains(audioInd));
        

        audioSource.clip = audioClips[audioInd];
        audioSource.Play();
    }

    public void UpdateVolumeSetting() {
        this.musicVolume = audioSettingSO.GetSettings(AudioSettingsEnum.musicVolume);
        audioSource.volume = musicVolume;
    }
}
