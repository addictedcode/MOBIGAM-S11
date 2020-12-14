using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform stagePanel;
    public StageData[] stageData;
    public GameObject pbButton;

    private void Start()
    {
        LoadStages();
    }

    private void LoadStages()
    {
        for (int i = 0; i < stageData.Length; ++i)
        {
            GameObject button = Instantiate(pbButton, stagePanel);
            int x = i;
            button.GetComponent<Button>().onClick.AddListener(() => StartStage(x));
            button.GetComponentInChildren<Text>().text = stageData[i].stageName;
        }
    }

    public void StartStage(int index) 
    {
        StageParameter.stageToLoad = stageData[index];
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
