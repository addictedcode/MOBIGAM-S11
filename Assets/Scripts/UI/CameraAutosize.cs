using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutosize : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ScreenOrientationManager.instance.OnOrientationChange += OnOrientationChange;
    }

    private void OnDisable()
    {
        ScreenOrientationManager.instance.OnOrientationChange -= OnOrientationChange;
    }

    private void OnOrientationChange(object sender, OnOrientationChangeEventArg args)
    {
        ScreenOrientation orientation = args.Orientation;
        if (orientation == ScreenOrientation.Portrait)
        {
            this.GetComponentInParent<Camera>().orthographicSize = ((float)Screen.height / (float)Screen.width) * 5.0f;
        }
        else
        {
            this.GetComponentInParent<Camera>().orthographicSize = 5.0f;
        }
    }
}
