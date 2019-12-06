using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicBackground;
    public AudioSource lobbyTheme;
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

    public void LobbyStart()
    {
        if (musicBackground.isPlaying) {
            musicBackground.Stop();
            musicBackground = lobbyTheme;
            musicBackground.Play();
        }
    }
}
