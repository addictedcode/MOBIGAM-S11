using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private OptionsValues optionsValue;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private MainMenu mainMenu;

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
        playerStats.money += value;
    }

    public void GenerateDebugNotification()
    {
        string title = "Space Guy Shooting";
        string text = "DEBUG, Notification is working!";
        System.DateTime fireTime = System.DateTime.Now.AddSeconds(3);
        AndroidNotification notif = new AndroidNotification(title, text, fireTime);

        notif.LargeIcon = "spaceguy";

        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    public void UnlockAllStages()
    {
        playerStats.latestStageIndex = int.MaxValue;
        mainMenu.ReloadStages();
    }
}
