using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerStats : ScriptableObject
{
    public int maxHp = 1;
    public int money = 0;

    public int[] weaponDamage;

    public float cameraSpeed = 0.5f;

    public int[] numTimesUpgrade = {0};

}
