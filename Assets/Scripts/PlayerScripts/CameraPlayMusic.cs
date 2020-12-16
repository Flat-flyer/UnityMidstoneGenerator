using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayMusic : MonoBehaviour
{

    private AudioSource Music;
    // Start is called before the first frame update
    void Start()
    {
        Music = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMusic()
    {
        
        if (Music != null && Music.isPlaying == false)
        {
            Music.Play();
        }
    }
}
