using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Directions
{
    UP, DOWN, LEFT, RIGHT
}

public class OnSwipeEventArg : EventArgs
{
    private Vector2 swipePosition;
    private Vector2 swipeVector;
    private Directions swipeDirection;
    private GameObject hitObject;

    public OnSwipeEventArg(Vector2 pos, Vector2 v, Directions dir, GameObject hit)
    {
        swipePosition = pos;
        swipeVector = v;
        swipeDirection = dir;
        hitObject = hit;
    }

    public Vector2 SwipePosition{
        get{
            return swipePosition;
        }
    }

    public Vector2 SwipeVector
    {
        get
        {
            return swipeVector;
        }
    }

    public Directions SwipeDirection
    {
        get
        {
            return swipeDirection;
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
