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
    [SerializeField] private Transform weaponDisplayPanel;

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
        StopAllCoroutines();
        for (int i = 0; i < weaponPanel.childCount; i++)
        {
            if (i == index)
            {
                weaponPanel.GetChild(i).GetComponent<Button>().interactable = false;
                StartCoroutine(FadeGameObject(weaponDisplayPanel.GetChild(i).gameObject));
            }
            else
            {
                weaponPanel.GetChild(i).GetComponent<Button>().interactable = true;
                weaponDisplayPanel.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator FadeGameObject(GameObject obj)
    {
        obj.SetActive(true);
        Image img = obj.GetComponent<Image>();

        img.color = new Color(1, 1, 1, 0.66f);

        yield return new WaitForSeconds(0.3f);

        Color transparent = new Color(1, 1, 1, 0);

        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 5; ++i)
        {
            img.color = Color.Lerp(img.color, transparent, 0.5f);
            yield return delay;
        }

        obj.SetActive(false);
    }
}

