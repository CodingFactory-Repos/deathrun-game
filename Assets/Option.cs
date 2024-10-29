using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public List<AudioSource> ambienceSources;
    public AudioSource musicSource;

    public Slider musicSlider;
    public Slider  ambienceSlider;

    private float musicVolume = 0.5f;
    private float ambienceVolume = 0;

    // public void SetVolume(float volume)
    // {
       
    // }

    // public void SetAllAmbienceVolume(float volume)
    // {
      
    // }

    private void Update() {
      
        musicSource.volume = musicSlider.value;

        foreach (AudioSource audioSource in ambienceSources)
        {
            audioSource.volume = ambienceSlider.value;
        }
    }

    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen =isFullScreen;
    }
}
