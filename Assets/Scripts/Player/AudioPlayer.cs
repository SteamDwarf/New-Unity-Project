using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private bool isPlaying;
    private AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayContiniousSound(AudioClip audioClip) {
        if(isPlaying) {
            return;
        }

        audioSource.PlayOneShot(audioClip);
        StartCoroutine(PlayCoroutine(0.1f));
    }
    public void PlayContiniousSound(AudioClip audioClip, float delay) {
        if(isPlaying) {
            return;
        }

        audioSource.PlayOneShot(audioClip);
        StartCoroutine(PlayCoroutine(delay));
    }
    public void PlayDoubleSound(AudioClip audioClip, AudioClip audioClip2, float delay) {
        if(isPlaying) {
            return;
        }

        StartCoroutine(DoubleSoundCoroutine(audioClip, audioClip2, delay));
    }
    public void StopPlayer() {
        isPlaying = false;
        StopAllCoroutines();
        audioSource.Stop();
    }
    
    private IEnumerator PlayCoroutine(float delay) {
        isPlaying = true;
        yield return new WaitForSeconds(delay);
        isPlaying = false;
    }
    private IEnumerator DoubleSoundCoroutine(AudioClip audioClip, AudioClip audioClip2, float delay) {
        isPlaying = true;
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(delay + 0.1f);
        audioSource.PlayOneShot(audioClip2);
        yield return new WaitForSeconds(delay);
        isPlaying = false;
    }
}
