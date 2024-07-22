using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance;

    [Header("Sounds in game: ")]
    public Sound[] soundsInGame;

    [Header("Event sounds: ")]
    public Sound[] eventSounds;

    [Header("Background music:")]
    public Sound backgroundMusic;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // LOAD SOUNDS
        foreach (Sound sound in soundsInGame)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audio;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.outputAudioMixerGroup =sound.mixer;
        }

        foreach (Sound sound in eventSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audio;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.outputAudioMixerGroup =sound.mixer;
        }
    }

    private void Awake() {
        // PLAY BACKGROUND MUSIC AUTOMATICALLY
        backgroundMusic.source = gameObject.AddComponent<AudioSource>();
        backgroundMusic.source.clip = backgroundMusic.audio;
        backgroundMusic.source.volume = backgroundMusic.volume;
        backgroundMusic.source.pitch = backgroundMusic.pitch;
        backgroundMusic.loop = true;

        backgroundMusic.source.outputAudioMixerGroup =backgroundMusic.mixer;

        backgroundMusic.source.Play();
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(soundsInGame, item => item.name == name);

        if(sound == null){
            sound = Array.Find(eventSounds, item => item.name == name);
        }

        try
        {
            sound.source.loop = sound.loop;

            if (!sound.source.isPlaying)
            {
                sound.source.Play();
            }
        }
        catch
        {
            Debug.Log(name);
        }
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(soundsInGame, item => item.name == name);

        if(sound == null){
            sound = Array.Find(eventSounds, item => item.name == name);
        }

        try
        {
            sound.source.Stop();
        }
        catch
        {
            Debug.Log(name);
        }
    }

    public void ClearInGameSounds(){
        foreach (Sound sound in soundsInGame){
            try{
                sound.source.Stop();
            }
            catch{
                Debug.Log("error");
            }
        }
    }
}
