using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private AudioSource source;
    [SerializeField] private AudioClip[] musicClips;

    [SerializeField] private OptionsValues options;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        UpdateVolume();
        PlayMusic(0);
    }

    public void PlayMusic(int index)
    {
        source.Stop();
        source.clip = musicClips[index];
        source.Play();
    }

    public void PauseMusic()
    {
        source.Pause();
    }

    public void ResumeMusic() 
    {
        source.Play();
    }

    public void StopMusic()
    {
        source.Stop();
    }

    public void UpdateVolume()
    {
        source.volume = options.musicVolume;
    }
}
