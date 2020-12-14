using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DragProperty
{
    [Tooltip("How long must the finger be on the screen before we send the drag event")]
    public float dragBufferTime = 0.8f;
}
