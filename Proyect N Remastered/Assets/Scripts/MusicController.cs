using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicBackground;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayMusic()
    {
        if (musicBackground.isPlaying) return;
        musicBackground.Play();
    }

    public void StopMusic()
    {
        musicBackground.Stop();
    }
}
