using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Asteroid,
    Ship,
    Projectile
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Enemies")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;
    public int hp = 1;
    public float speed = 1.0f;
    public Sprite[] sprites;
}
