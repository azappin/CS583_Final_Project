using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    //Instance
    public static AudioManager instance;

    //Reference to Music Objects
    public AudioSource[] music;
    public AudioSource[] soundEffects;

    //Level Reference
    public int levelMusicToPlay;

    //Current Track Reference
    private int currentTrack;

    //Reference to Audio Mixer
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(levelMusicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        { 
            currentTrack++;
            PlayMusic(currentTrack);
        }*/

    }

    //Function to play background music
    public void PlayMusic(int position)
    {
        for(int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }

        music[position].Play();
    }

    //Function to play SoundEffects
    public void PlaySFX(int position)
    {
        soundEffects[position].Play();
    }

    //Set Music Volume Through Slider
    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("MusicVol", UIManager.instance.musicVolSlider.value);
    }

    //Set SFX Level Through Slider
    public void SetSFXLevel()
    {
        sfxMixer.audioMixer.SetFloat("SFXVol", UIManager.instance.sfxVolSlider.value);
    }

}
