using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSounds : MonoBehaviour
{
    public static ShortSounds instance {get; private set;}
    private AudioSource audioSource;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        // KEEP THIS OBJ
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // DELETE DUPLICATE
        else if(instance != null && instance != this){
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }

    public void Play(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }
}
