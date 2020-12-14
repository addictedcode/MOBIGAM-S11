using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDValues : MonoBehaviour
{
    #region Singleton
    public static HUDValues instance;

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

    public Text score;
    public Text hp;
    public Text bombs;
    public Text money;
    public Transform weaponPanel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        UpdateHP(Player.instance.stats.maxHp);
        UpdateBombs(Player.instance.bombs);
        UpdateMoney(Player.instance.stats.money);
        UpdateWeapon((int)Player.instance.currentWeaponType);
    }

    public void UpdateScore(int value)
    {
        score.text = "Score: " + value.ToString();
    }

    public void UpdateHP(int value)
    {
        hp.text = "HP: " + value.ToString();
    }

    public void UpdateBombs(int value)
    {
        bombs.text = "Bombs: " + value.ToString();
    }

    public void UpdateMoney(int value)
    {
        money.text = "Money: " + value.ToString();
    }

    public void UpdateWeapon(int index)
    {
        Player.instance.currentWeaponType = (EnemyType)index;
        for (int i = 0; i < weaponPanel.childCount; i++)
        {
            if (i == index)
            {
                weaponPanel.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                weaponPanel.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
    }
}

