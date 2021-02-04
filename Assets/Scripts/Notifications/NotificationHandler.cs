using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationHandler : MonoBehaviour
{
    public void BuildDefaultNotificationChannel()
    {
        string channel_id = "default";
        string title = "Default Channel";

        Importance importance = Importance.Default;

        string description = "Default channel for this game";

        AndroidNotificationChannel channel = new AndroidNotificationChannel(channel_id, title, description, importance);
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void Awake()
    {
        BuildDefaultNotificationChannel();
    }

    private void CheckIntentData()
    {
        AndroidNotificationIntentData data = AndroidNotificationCenter.GetLastNotificationIntent();
    }

    private void Start()
    {
        CheckIntentData();
    }
}
