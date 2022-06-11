using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar _healthbar; //reference to healthbar object in the scene

    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
     
    }

    /* Damages the player. Generic script is used so it's accessed through game manager*/
    private void PlayerTakeDamage(int damageAmount)
    {
        GameManager.gameManager._playerHealth.TakeDamage(damageAmount);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);

        if (GameManager.gameManager._playerHealth.Health <= 0)
        {
           Die();
        }
    }

    /* Heals the player. Generic script is used so it's accessed through game manager*/
    private void PlayerHeal(int healAmount)
    {
        GameManager.gameManager._playerHealth.Heal(healAmount);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    /* Damage player on collision with traps and enemies */
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Trap"))
        {
            PlayerTakeDamage(100);
        }

        if (collision.gameObject.CompareTag("hostile"))
        {
            PlayerTakeDamage(10);
        }
        if(collision.gameObject.CompareTag("turretbullet"))
        {
            PlayerTakeDamage(20);
        }
    }

    /* Trigger death animation and restart the level */
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("Death");
        FindObjectOfType<GameManager>().RestartLevel();
    }


}
