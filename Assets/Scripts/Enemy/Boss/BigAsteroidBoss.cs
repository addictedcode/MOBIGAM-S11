using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroidBoss : Enemy
{
    private int numSplits = 3;
    private float scale = 50;

    private void Start()
    {
        damage = 4;
        hp = 50;
        enemyType = EnemyType.Asteroid;
        speed = 0.5f;
    }

    private void Update()
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
        float newScale = scale - (scale * (transform.position.z / 100));
        transform.localScale = new Vector3(newScale, newScale);
        if (transform.position.z <= 0)
        {
            HitPlayer();
        }
    }

    public void Initialize(int split, int dmg, float scl, int newhp)
    {
        numSplits = split;
        damage = dmg;
        scale = scl;
        hp = newhp;
    }

    protected override void TakeDamage(int damage)
    {
        hp -= damage;
        StopCoroutine(AnimateTakeDamage());
        StartCoroutine(AnimateTakeDamage());
        if (hp <= 0)
        {
            if (numSplits > 0)
            {
                for (int i = 0; i < 3; ++i)
                {
                    float x = Random.Range(-2f, 2f);
                    float y = Random.Range(-2f, 2f);

                    Quaternion rotation = Random.rotation;
                    rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);

                    GetComponent<SpriteRenderer>().color = Color.white;

                    BigAsteroidBoss smallAsteroid = Instantiate(this, new Vector3(x, y, transform.position.z), rotation, transform.parent);
                    smallAsteroid.Initialize(numSplits - 1, damage - 1, scale / 2, hp / 3);
                }
            }
            GameObject explosion = Instantiate(pbExplosion, transform.position, transform.rotation);
            explosion.transform.localScale = transform.localScale;
            Destroy(explosion, 1.5f);

            Destroy(gameObject);
            Player.instance.AddScore(5);
            Player.instance.AddMoney(50);
            if (numSplits <= 0)
            {
                //Check if killed all asteroids
                StageManager.instance.CheckIfBossIsDead();
            }
        }
    }

    protected override void HitPlayer()
    {
        base.HitPlayer();

        if (numSplits <= 0)
        {
            //Check if killed all asteroids
            StageManager.instance.CheckIfBossIsDead();
        }
    }
}
