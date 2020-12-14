using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagesSpawnButton : MonoBehaviour
{
    [SerializeField] private GameObject pb_button;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 100; ++i)
        {
            GameObject newButton = Instantiate(pb_button, transform);
            newButton.transform.GetChild(0).GetComponent<Text>().text = i.ToString("00");
        }
    }

}
