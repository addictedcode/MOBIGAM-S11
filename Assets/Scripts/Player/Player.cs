using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

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

    public PlayerStats stats;
    public EnemyType currentWeaponType = EnemyType.Asteroid;
    private int hp = 1;
    public int score = 0;
    public int bombs = 1;

    public Transform enemyHolder;
    [SerializeField] private GameObject pbExplosion;

    [SerializeField] private GameObject redPanel;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.instance.OnSwipe += OnSwipe;
        GestureManager.instance.OnPinchSpread += OnPinch;
        hp = stats.maxHp;
    }

    private void OnDisable()
    {
        GestureManager.instance.OnSwipe -= OnSwipe;
        GestureManager.instance.OnPinchSpread -= OnPinch;
    }

    public void OnSwipe(object sender, OnSwipeEventArg args)
    {
        switch (args.SwipeDirection)
        {
            case Directions.LEFT:
                currentWeaponType--;
                if (currentWeaponType < EnemyType.Asteroid)
                {
                    currentWeaponType = EnemyType.Projectile;
                }
                HUDValues.instance.UpdateWeapon((int)currentWeaponType);
                break;
            case Directions.RIGHT:
                currentWeaponType++;
                if (currentWeaponType > EnemyType.Projectile)
                {
                    currentWeaponType = EnemyType.Asteroid;
                }
                HUDValues.instance.UpdateWeapon((int)currentWeaponType);
                break;
        }
    }

    public void OnPinch(object sender, OnPinchSpreadEventArg args)
    {
        if (bombs > 0)
        {
            if (args.DistanceDifference < 0)
            {
                bombs--;
                HUDValues.instance.UpdateBombs(bombs);
                GameObject explosion = Instantiate(pbExplosion, transform.GetComponentInChildren<Transform>().position, transform.GetComponentInChildren<Transform>().rotation);
                explosion.transform.localScale = new Vector3(25, 25, 25);
                Destroy(explosion, 1.5f);
                AudioManager.instance.PlayExplosionSound();
                foreach (Enemy enemy in enemyHolder.GetComponentsInChildren<Enemy>()) {
                    if (!enemy.isBoss)
                    {
                        Destroy(enemy.gameObject);
                    }
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        AudioManager.instance.PlayExplosionSound();
        AudioManager.instance.PlayWarningSound();
        HUDValues.instance.UpdateHP(hp);
        StopAllCoroutines();
        StartCoroutine(AnimateTakeDamage());
        if (hp <= 0)
        {
            StageManager.instance.DisplayEndPanel(true);
        }
    }

    private IEnumerator AnimateTakeDamage()
    {
        Image sprite = redPanel.GetComponent<Image>();
        sprite.color = new Color(1, 0.4f, 0.4f, 0.4f);

        WaitForSeconds delay = new WaitForSeconds(0.1f);

        redPanel.SetActive(true);

        for (int i = 0; i < 5; ++i)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 0.4f, 0.4f, 0.05f), 0.5f);
            yield return delay;
        }

        redPanel.SetActive(false);
    }

    public void AddScore(int value)
    {
        score += value;
        HUDValues.instance.UpdateScore(score);
    }

    public void AddMoney(int value)
    {
        stats.money += value;
        HUDValues.instance.UpdateMoney(stats.money);
    }
}
