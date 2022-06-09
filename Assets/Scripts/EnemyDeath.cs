using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [Header("Health System Variables")]
    private HealthSystem _enemyHealth = new HealthSystem(30, 30); //set enemy max health and current health

    /* Damage enemy on collision. 15 is hardcoded for testing purposes */
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            EnemyTakeDamage(15);
        }
    }

    /* Use generic take damage function to remove enemy health. When health is below zero, destroy game object */
    private void EnemyTakeDamage(int damageAmount)
    {
        _enemyHealth.TakeDamage(damageAmount);

        if (_enemyHealth.Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
