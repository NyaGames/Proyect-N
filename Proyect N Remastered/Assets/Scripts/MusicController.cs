using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource musicBackground;
    private AudioSource mainMenuTheme;
    public AudioSource lobbyTheme;
    public AudioSource gameTheme;
    public AudioSource victoryTheme;
    public AudioSource defeatTheme;

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

    public void GameStart() {
        if (musicBackground.isPlaying)
        {
            musicBackground.Stop();
            musicBackground = gameTheme;
            musicBackground.Play();
        }
    }

    public void Victory() {
        if (musicBackground.isPlaying)
        {
            musicBackground.Stop();
            musicBackground = victoryTheme;
            musicBackground.Play();
        }
    }

    public void Defeat()
    {
        if (musicBackground.isPlaying)
        {
            musicBackground.Stop();
            musicBackground = defeatTheme;
            musicBackground.Play();
        }
    }

    public void returnToMainMenu()
    {
        if (musicBackground.isPlaying)
        {
            musicBackground.Stop();
            musicBackground = mainMenuTheme;
            musicBackground.Play();
        }
    }
}
