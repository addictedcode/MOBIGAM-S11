using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantNukeBoss : Enemy
{
    private void Start()
    {
        damage = 999;
        hp = 100;
        enemyType = EnemyType.Projectile;
        speed = 1f;
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
        float newScale = 50 - (50 * (transform.position.z / 100));
        transform.localScale = new Vector3(newScale, newScale);
        if (transform.position.z <= 0)
        {
            HitPlayer();
        }
    }

    protected override void TakeDamage(int damage)
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
            Player.instance.AddScore(15);
            Player.instance.AddMoney(150);
            StageManager.instance.DisplayEndPanel(false);
        }
    }
}
