using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapReceiever : MonoBehaviour
{
    public GameObject spawn;

    private void Start()
    {
        GestureManager.instance.OnTap += OnTap;
    }

    private void OnDisable()
    {
        GestureManager.instance.OnTap -= OnTap;
    }

    private void OnTap(object sender, OnTapEventArg e)
    {
        if (e.HitObject == null)
        {
            Ray r = Camera.main.ScreenPointToRay(e.TapPosition);
            Spawn(r.GetPoint(10));
        }
        else
        {
            
        }
    }

    private void Spawn(Vector3 pos)
    {
        Instantiate(spawn, pos, Quaternion.identity);
    }
}
