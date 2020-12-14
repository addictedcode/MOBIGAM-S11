using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerStats playerStats;
    public Text moneyText;

    public int[] baseUpgradeCost;
    public int[] maxNumTimesUpgrade;
    public Transform[] upgradePanel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyText();
        UpdateButtons();
    }

    public void OnUpgrade(int index)
    {
        if ((baseUpgradeCost[index] * (playerStats.numTimesUpgrade[index] + 1)) > playerStats.money)
        {
            return;
        }
        playerStats.money -= (baseUpgradeCost[index] * (playerStats.numTimesUpgrade[index] + 1));

        playerStats.numTimesUpgrade[index]++;

        switch (index)
        {
            case 0:
                playerStats.maxHp += 3;
                break;
            case 1:
                playerStats.cameraSpeed += 0.5f;
                break;
            case 2:
                playerStats.weaponDamage[0]++;
                break;
            case 3:
                playerStats.weaponDamage[1]++;
                break;
            case 4:
                playerStats.weaponDamage[2]++;
                break;
        }

        UpdateMoneyText();
        UpdateButtons();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money  $" + playerStats.money.ToString();
        for (int i = 0; i < upgradePanel.Length; ++i) {
            if (playerStats.numTimesUpgrade[i] < maxNumTimesUpgrade[i])
            {
                upgradePanel[i].GetChild(1).GetComponent<Text>().text = "$" + (baseUpgradeCost[i] * (playerStats.numTimesUpgrade[i] + 1)).ToString();
            }
        }
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < upgradePanel.Length; ++i)
        {
            if (playerStats.numTimesUpgrade[i] >= maxNumTimesUpgrade[i])
            {
                upgradePanel[i].GetChild(0).GetComponent<Button>().interactable = false;
                upgradePanel[i].GetChild(1).GetComponent<Text>().text = "-";
            }
        }
    }
}
