using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnOrientationChangeEventArg : EventArgs
{
    public ScreenOrientation Orientation { get; private set; }

    public OnOrientationChangeEventArg(ScreenOrientation orient)
    {
        Orientation = orient;
    }
}
