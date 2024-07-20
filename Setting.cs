using UnityEngine;
using UnityEditor.UI;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{

    public Slider volumeSlider;
    public AudioMixer mixer;

    private float db;

    // // SAVE SETTING
    // public int score {get; set;}
    // public bool hardMode;

    private void Start() {
        mixer.GetFloat("volume", out db);    
        volumeSlider.value = db;
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    void Update()
    {

    }

    // public void SetHardMode(){
    //     hardMode = true;
    // }

    // public void RemoveHardMode(){
    //     hardMode = false;
    // }

    public void ChangeVolume(){
        mixer.SetFloat("volume", volumeSlider.value);
    }
}