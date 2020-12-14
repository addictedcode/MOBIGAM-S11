using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDOnOrientationChange : MonoBehaviour
{
    public Transform horizontalPanel;
    public Transform verticalPanel;

    // Start is called before the first frame update
    void Start()
    {
        ScreenOrientationManager.instance.OnOrientationChange += OnOrientationChange;
    }

    private void OnDisable()
    {
        ScreenOrientationManager.instance.OnOrientationChange -= OnOrientationChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOrientationChange(object sender, OnOrientationChangeEventArg args)
    {
        ScreenOrientation orientation = args.Orientation;
        if (orientation == ScreenOrientation.Landscape)
        {
            if (verticalPanel.childCount > 0)
            {
                horizontalPanel.gameObject.SetActive(true);
                verticalPanel.gameObject.SetActive(false);
                int numChild = verticalPanel.childCount;
                for (int i = 0; i < numChild; i++)
                {
                    verticalPanel.GetChild(0).SetParent(horizontalPanel);
                }
            }
        }
        else
        {
            if (horizontalPanel.childCount > 0)
            {
                horizontalPanel.gameObject.SetActive(false);
                verticalPanel.gameObject.SetActive(true);
                int numChild = horizontalPanel.childCount;
                for (int i = 0; i < numChild; i++)
                {
                    horizontalPanel.GetChild(0).SetParent(verticalPanel);
                }
            }
        }
    }
}
