using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private OptionsValues optionsValue;

    public void SFXSlider(float value)
    {
        optionsValue.sfxVolume = value;
    }

    public void MusicSlider(float value)
    {
        optionsValue.musicVolume = value;
        MusicManager.instance.UpdateVolume();
    }

    public void AddMoney(int value)
    {
        Player.instance.stats.money += value;
    }
}
