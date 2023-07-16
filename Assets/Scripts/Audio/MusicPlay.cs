using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlay : MonoBehaviour
{
    public AudioSource introSource,
        loopSource;
    public Slider volumeSlider;

    private float defaultVolume = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void VolumeApply()
    {
        introSource.volume = AudioListener.volume;
        loopSource.volume = AudioListener.volume;
    }

    public void DefaultVolume(string menuType)
    {
        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            VolumeApply();
        }
    }
}
