using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIEvent {GameControl, Options, Stages};

public class MiddlePanelToggle : MonoBehaviour
{
    private Stack<UIEvent> history;

    private void Start()
    {
        history = new Stack<UIEvent>();
        history.Push(UIEvent.GameControl);
    }

    public void ResetHistory()
    {
        history.Clear();
        history.Push(UIEvent.GameControl);
    }

    public void TogglePanel(int value)
    {
        UIEvent uiEventValue = (UIEvent)value;

        if (history.Peek() == uiEventValue)
        {
            history.Pop();
            ToggleActivePanel();
            return;
        }
        if (history.Contains(uiEventValue))
        {
            RemoveEventInStack(uiEventValue);
        }

        history.Push(uiEventValue);
        ToggleActivePanel();
    }

    private void ToggleActivePanel()
    {
        //Deactive all panels
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //Activate recent panel
        transform.GetChild((int)history.Peek()).gameObject.SetActive(true);
    }

    private void RemoveEventInStack(UIEvent value)
    {
        Stack<UIEvent> tempHistory = new Stack<UIEvent>();
        for (int i = 0; i < history.Count; ++i)
        {
            UIEvent tempEventHolder = history.Pop();
            if (tempEventHolder != value)
            {
                tempHistory.Push(tempEventHolder);
            }
        }
        for (int i = 0; i < tempHistory.Count; ++i)
        {
            history.Push(tempHistory.Pop());
        }
    }
}
