using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnTwoFingerPanEventArg : EventArgs
{
    public Touch Finger1 { get; private set; }

    public Touch Finger2 { get; private set; }

    public OnTwoFingerPanEventArg(Touch f1, Touch f2)
    {
        Finger1 = f1;
        Finger2 = f2;
    }
}
