using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{

    public Slider volumeSlider;
    public AudioMixer mixer;

    private float db;
    
    private void Start() {
        mixer.GetFloat("volume", out db);    
        volumeSlider.value = db;
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void ChangeVolume(){
        mixer.SetFloat("volume", volumeSlider.value);
    }

    public void SetHardMode(){
        PlayerPrefs.SetInt("gameMode", 1);
    }

    public void SetEasyMode(){
        PlayerPrefs.SetInt("gameMode", 0);
    }
}