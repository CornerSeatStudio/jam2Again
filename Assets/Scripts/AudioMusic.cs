using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMusic : MonoBehaviour
{
    public AudioClip[] musics;
    private AudioSource audioSource;
    public GameObject audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=audioManager.GetComponent<AudioSource>();
        audioSource.loop=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying){
        audioSource.clip=GetRandomCip();
        audioSource.Play();
        }
    }
    private AudioClip GetRandomCip()
    {
        AudioClip music = null;
        music = musics[Random.Range(0, musics.Length)];
        
        while(music == audioSource.clip)
        {
            music = musics[Random.Range(0, musics.Length)];
        }
        return music;
    }
}
