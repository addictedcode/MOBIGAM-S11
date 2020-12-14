using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnDragEventArg : EventArgs
{
    private Touch targetFinger;
    private GameObject hitObject;

    public OnDragEventArg(Touch finger, GameObject obj)
    {
        targetFinger = finger;
        hitObject = obj;
    }

    public Touch TargetFinger
    {
        get
        {
            return targetFinger;
        }
    }

    public GameObject HitObject
    {
        get
        {
            return hitObject;
        }
    }
}
