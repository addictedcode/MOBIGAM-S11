using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Enemy/EnemyStageData")]
public class StageData : ScriptableObject
{
    public int stageIndex;
    public int bossMusicIndex;
    public string stageName;
    public EnemyType[] enemyType;
    public float[] enemySpawnTime;

    public GameObject pbBoss;
    public string bossAnnouncement;
}
