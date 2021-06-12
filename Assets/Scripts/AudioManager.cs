using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Jumpin;
    public AudioSource Swingin;
    public AudioSource Walkin;
    void Start()
    {
        Jumpin = gameObject.AddComponent<AudioSource>();
        Swingin = gameObject.AddComponent<AudioSource>();
        Walkin = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
