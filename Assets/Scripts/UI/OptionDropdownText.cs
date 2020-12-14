using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDropdownText : MonoBehaviour
{
    public void OnValueChange(int value)
    {
        Text textComponent = this.GetComponent<Text>();
        string valueText = "";
        switch (value) {
            case 0:
                valueText = "Kongou";
                break;
            case 1:
                valueText = "Haruna";
                break;
            case 2:
                valueText = "Hiei";
                break;
            case 3:
                valueText = "Kirishima";
                break;
        }
        textComponent.text = valueText;
    }
}
