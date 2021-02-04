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
    private AudioClip explosionClip;
    private AudioClip laserClip;
    private AudioClip warningClip;

    [SerializeField] private OptionsValues options;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        explosionClip = AssetBundleManager.instance.GetAsset<AudioClip>("audio", "laser1");
        laserClip = AssetBundleManager.instance.GetAsset<AudioClip>("audio", "explode");
        warningClip = AssetBundleManager.instance.GetAsset<AudioClip>("audio", "alarm");
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
