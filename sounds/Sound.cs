using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable] // CAN VISIBLE OUTSIDE INSPECTOR
public class Sound
{
    public string name;

    public AudioClip audio;
    public AudioMixerGroup mixer;

    [Range(0f, 1f)]
    public float volume;

    [Range(-3f, 3f)]
    public float pitch = 1;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}
