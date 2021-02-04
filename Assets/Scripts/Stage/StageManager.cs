using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.UI;

public struct EnemySpawnData
{
    public EnemyType enemyType;
    public float spawnTime;
}

public class StageManager : MonoBehaviour
{
    #region Singleton
    public static StageManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public RectTransform playableArea;
    public float padding = 0.0f;
    public Transform enemyHolder;
    private GameObject baseEnemyObject;

    private EnemyData[] enemyData = new EnemyData[3];

    private StageData stageData;

    private Queue<EnemySpawnData> enemiesToSpawn;
    private float timeElapsed = 0.0f;

    public GameObject startStagePanel;
    private bool isLoaded = false;

    public GameObject bossPanel;

    public GameObject stageEndPanel;

    private WaitForSeconds timerDelay = new WaitForSeconds(1);

    // Start is called before the first frame update
    void Start()
    {
        if (StageParameter.stageToLoad != null)
        {
            stageData = StageParameter.stageToLoad;
        }
        StartCoroutine(LoadingScreen());
        LoadBundles();
        LoadStageData();
        StartCoroutine(CheckIfBossCanSpawn());
    }

    private void LoadBundles()
    {
        AssetBundleManager.instance.LoadBundle("textures");
        AssetBundleManager.instance.LoadBundle("effects");
        baseEnemyObject = AssetBundleManager.instance.GetAsset<GameObject>("enemies", "Enemy");
        enemyData[0] = AssetBundleManager.instance.GetAsset<EnemyData>("enemies", "Asteroid");
        enemyData[1] = AssetBundleManager.instance.GetAsset<EnemyData>("enemies", "Ship");
        enemyData[2] = AssetBundleManager.instance.GetAsset<EnemyData>("enemies", "Projectile");
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplicationManager.instance.isApplicationPause || !isLoaded)
        {
            return;
        }
        timeElapsed += Time.deltaTime;
        if (enemiesToSpawn.Count > 0)
        {
            while (timeElapsed >= enemiesToSpawn.Peek().spawnTime)
            {
                SpawnEnemy(enemiesToSpawn.Dequeue().enemyType);
                if (enemiesToSpawn.Count <= 0)
                {
                    break;
                }
            }
        }
    }
    private void LoadStageData()
    {
        enemiesToSpawn = new Queue<EnemySpawnData>();
        for (int i = 0; i < stageData.enemyType.Length; ++i)
        {
            EnemySpawnData enemySpawnData = new EnemySpawnData();
            enemySpawnData.enemyType = stageData.enemyType[i];
            enemySpawnData.spawnTime = stageData.enemySpawnTime[i];
            enemiesToSpawn.Enqueue(enemySpawnData);
        }
    }

    private IEnumerator LoadingScreen()
    {
        startStagePanel.GetComponentInChildren<Text>().text = stageData.stageName;
        startStagePanel.SetActive(true);
        yield return new WaitForSeconds(3);
        startStagePanel.SetActive(false);
        isLoaded = true;
    }

    public void DisplayEndPanel(bool isDead)
    {
        MusicManager.instance.PlayMusic(0);
        ApplicationManager.instance.GameFinish();
        if (isDead)
        {
            stageEndPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You Lose";
        }
        else
        {
            stageEndPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You Win";
            SendWinNotification();
            if (Player.instance.stats.latestStageIndex < stageData.stageIndex)
            {
                Player.instance.stats.latestStageIndex = stageData.stageIndex;
            }
        }

        stageEndPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = Player.instance.score.ToString("000000000");
        stageEndPanel.SetActive(true);
    }

    private IEnumerator CheckIfBossCanSpawn()
    {
        while (true)
        {
            if (enemyHolder.childCount <= 0 && enemiesToSpawn.Count <= 0)
            {
                break;
            }
            yield return timerDelay;
        }
        StartCoroutine(SpawnBoss());
    }

    private IEnumerator SpawnBoss()
    {
        bossPanel.transform.GetChild(1).GetComponent<Text>().text = stageData.bossAnnouncement;
        bossPanel.SetActive(true);
        bossPanel.transform.GetChild(0).gameObject.SetActive(true);
        bossPanel.transform.GetChild(1).gameObject.SetActive(false);
        MusicManager.instance.StopMusic();
        AudioManager.instance.PlayWarningSound();
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayWarningSound();
        bossPanel.transform.GetChild(0).gameObject.SetActive(false);
        bossPanel.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        MusicManager.instance.PlayMusic(stageData.bossMusicIndex);
        bossPanel.SetActive(false);
        Quaternion rotation = Random.rotation;
        rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);
        Instantiate(stageData.pbBoss, new Vector3(0,0,100), rotation, enemyHolder);
    }

    public void CheckIfBossIsDead()
    {
        if (enemyHolder.childCount <= 0)
        {
            DisplayEndPanel(false);
        }
    }

    public void SpawnEnemy(EnemyType type)
    {
        Quaternion rotation = Random.rotation;
        rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);
        GameObject test = Instantiate(baseEnemyObject, GetRandomPointInPlayableArea(), rotation , enemyHolder);
        test.GetComponent<Enemy>().Initialize(enemyData[(int)type]);
    }
    private Vector3 GetRandomPointInPlayableArea()
    {
        float playableAreaBorderX = ((playableArea.rect.width * playableArea.lossyScale.x) / 2) - padding;
        float playableAreaBorderY = ((playableArea.rect.height * playableArea.lossyScale.y) / 2) - padding;

        float x = Random.Range(-playableAreaBorderX, playableAreaBorderX);
        float y = Random.Range(-playableAreaBorderY, playableAreaBorderY);
        float z = 50.0f;

        Vector3 spawnPoint = new Vector3(x, y, z);

        return spawnPoint;
    }

    private void SendWinNotification()
    {
        string title = "Space Guy Shooting";
        string text = "Congrats! You have defeated the " + stageData.bossName + "!";
        System.DateTime fireTime = System.DateTime.Now.AddSeconds(3);
        AndroidNotification notif = new AndroidNotification(title, text, fireTime);

        notif.LargeIcon = "spaceguy";

        AndroidNotificationCenter.SendNotification(notif, "default");
    }
}
