using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipBoss : Enemy
{
    private WaitForSeconds miniShipSpawnInterval = new WaitForSeconds(15.0f);
    private WaitForSeconds projectileSpawnInterval = new WaitForSeconds(20.0f);

    private void Start()
    {
        damage = 0;
        hp = 100;
        enemyType = EnemyType.Ship;
        speed = 0f;
        StartCoroutine(SpawnShip());
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnShip()
    {
        while (true)
        {
            StageManager.instance.SpawnEnemy(EnemyType.Ship);
            yield return miniShipSpawnInterval;
        }

    }

    private IEnumerator SpawnProjectile()
    {
        while (true)
        {
            StageManager.instance.SpawnEnemy(EnemyType.Projectile);
            yield return projectileSpawnInterval;
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
            Player.instance.AddScore(10);
            Player.instance.AddMoney(100);
            StageManager.instance.DisplayEndPanel(false);
        }
    }
}
