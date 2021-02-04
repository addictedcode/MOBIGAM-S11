using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform stagePanel;
    private StageData[] stageData;
    public GameObject pbButton;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        AdsManager.instance.OnAdDone += OnAdFinish;
        AssetBundleManager.instance.LoadBundle("bosses");
        stageData = AssetBundleManager.instance.GetAllAsset<StageData>("stage");
        LoadStages();
    }

    private void OnDisable()
    {
        AdsManager.instance.OnAdDone -= OnAdFinish;
    }

    private void LoadStages()
    {
        for (int i = 0; i < stageData.Length; ++i)
        {
            GameObject button = Instantiate(pbButton, stagePanel);
            int x = i;
            button.GetComponentInChildren<Text>().text = stageData[i].stageName;
            if (playerStats.latestStageIndex >= stageData[i].stageIndex)
            {
                button.GetComponent<Button>().onClick.AddListener(() => StartStage(x));
            }
            else
            {
                button.GetComponent<Button>().interactable = false;

            }
        }
    }

    public void ReloadStages()
    {
        foreach (Transform child in stagePanel.transform){
            Destroy(child.gameObject);
        }
        LoadStages();
    }

    public void StartStage(int index) 
    {
        AdsManager.instance.ShowVideoAd();
        AdsManager.instance.HideBannerAd();
        StageParameter.stageToLoad = stageData[index];
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    private void OnAdFinish(object sender, AdFinishEventArgs e)
    {
        if (e.PlacementID == AdsManager.videoAdID)
        {
            SceneManager.LoadScene(1);
        }
    }
}
