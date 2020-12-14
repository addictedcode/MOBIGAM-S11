using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSliderText : MonoBehaviour
{
    public void OnValueChange(float value)
    {
        Text textComponent = this.GetComponent<Text>();
        textComponent.text = value.ToString();
    }
}
