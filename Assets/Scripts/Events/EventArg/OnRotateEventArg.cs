using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RotationDirections
{
    CW,
    CCW
}

public class OnRotateEventArg : EventArgs
{
    public Touch Finger1 { get; private set; }

    public Touch Finger2 { get; private set; }

    public float Angle { get; private set; }

    public RotationDirections RotationDirection { get; private set; }

    public GameObject HitObject { get; private set; }

    public OnRotateEventArg(Touch f1, Touch f2, float a, RotationDirections dir, GameObject obj)
    {
        Finger1 = f1;
        Finger2 = f2;
        Angle = a;
        RotationDirection = dir;
        HitObject = obj;
    }
}
