using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnPinchSpreadEventArg : EventArgs
{
    public Touch Finger1 { get; private set; }

    public Touch Finger2 { get; private set; }

    public float DistanceDifference { get; private set; }

    public GameObject HitObject { get; private set; }

    public OnPinchSpreadEventArg(Touch f1, Touch f2, float dist, GameObject obj)
    {
        Finger1 = f1;
        Finger2 = f2;
        DistanceDifference = dist;
        HitObject = obj;
    }
}
