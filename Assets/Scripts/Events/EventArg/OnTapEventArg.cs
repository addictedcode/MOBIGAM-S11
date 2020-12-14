using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnTapEventArg : EventArgs
{
    private Vector2 _tapPosition;
    private GameObject _hitObject;

    public OnTapEventArg(Vector2 pos, GameObject obj = null)
    {
        _tapPosition = pos;
        _hitObject = obj;
    }

    public Vector2 TapPosition
    {
        get
        {
            return _tapPosition;
        }
    }

    public GameObject HitObject
    {
        get
        {
            return _hitObject;
        }
    }
}
