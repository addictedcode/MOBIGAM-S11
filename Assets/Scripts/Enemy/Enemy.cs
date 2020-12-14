using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITapped
{
    protected int hp = 0;
    protected EnemyType enemyType = 0;
    protected float speed = 0;

    protected int damage = 1;
    public bool isBoss = false;

    [SerializeField] protected GameObject pbExplosion;

    public void Initialize(EnemyData data)
    {
        GetComponent<SpriteRenderer>().sprite = data.sprites[Random.Range(0, data.sprites.Length)];
        hp = data.hp;
        enemyType = data.enemyType;
        speed = data.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplicationManager.instance.isApplicationPause)
        {
            return;
        }
        Move();
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (speed * Time.deltaTime));
        float newScale = 10 - (10 * (transform.position.z / 50));
        transform.localScale = new Vector3(newScale, newScale);
        if (transform.position.z <= 0)
        {
            HitPlayer();
        }
    }

    public void OnTap()
    {
        if (Player.instance.currentWeaponType == enemyType)
        {
            AudioManager.instance.PlayLaserSound();
            TakeDamage(Player.instance.stats.weaponDamage[(int)enemyType]);
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        hp -= damage;
        StopAllCoroutines();
        StartCoroutine(AnimateTakeDamage());
        if (hp <= 0)
        {
            GameObject explosion = Instantiate(pbExplosion, transform.position, transform.rotation);
            explosion.transform.localScale = transform.localScale;
            Destroy(explosion, 1.5f);

            AudioManager.instance.PlayExplosionSound();

            Destroy(gameObject);
            Player.instance.AddScore(1);
            Player.instance.AddMoney(20);
        }
    }

    protected IEnumerator AnimateTakeDamage()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 0.35f, 0.35f, 1);

        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 5; ++i)
        {
            sprite.color = Color.Lerp(sprite.color, Color.white, 0.5f);
            yield return delay;
        }

        sprite.color = Color.white;
    }

    protected virtual void HitPlayer()
    {
        Player.instance.TakeDamage(damage);
        Destroy(gameObject);
    }
}
