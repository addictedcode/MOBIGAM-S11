using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenOrientationManager : MonoBehaviour
{
    #region Singleton
    public static ScreenOrientationManager instance;

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

    public EventHandler<OnOrientationChangeEventArg> OnOrientationChange;

    private ScreenOrientation orientation = ScreenOrientation.AutoRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScreenOrientation newOrientation;
        if (Screen.width < Screen.height)
        {
            newOrientation = ScreenOrientation.Portrait;
        }
        else
        {
            newOrientation = ScreenOrientation.Landscape;
        }

        if (orientation != newOrientation)
        {
            orientation = newOrientation;
            OnOrientationChangeEventArg args = new OnOrientationChangeEventArg(orientation);

            if (OnOrientationChange != null)
            {
                OnOrientationChange(this, args);
            }
        }
    }
}
