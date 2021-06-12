using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip[] audioClips;

    public void Play(AudioSource source) {
        if(audioClips.Length == 0) return;

        source.clip = audioClips[Random.Range(0, audioClips.Length)];
        source.Play();

    }

}
