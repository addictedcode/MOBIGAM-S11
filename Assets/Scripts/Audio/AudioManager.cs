using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

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
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip warningClip;

    [SerializeField] private OptionsValues options;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayExplosionSound()
    {
        source.PlayOneShot(explosionClip, options.sfxVolume);
    }

    public void PlayLaserSound()
    {
        source.PlayOneShot(laserClip, options.sfxVolume);
    }

    public void PlayWarningSound()
    {
        source.PlayOneShot(warningClip, options.sfxVolume);
    }
}
