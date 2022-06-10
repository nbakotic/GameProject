using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemyController : MonoBehaviour
{
    [Header("Transforms")]
    public Rigidbody2D Rb;
    public Transform Player;

    [Header("Values")]
    public float ZombieVelocity = 5.0f; // Enemy's base velocity
    public float PlayerDistance; // Distance from Player
    public float AttackRange = 5.0f; // Distance in which Zombie will start following Player

    void Start()
    {
        Rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerDistance = Vector2.Distance(transform.position, Player.transform.position);
        if(PlayerDistance < AttackRange){
            Attack();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != "Player" && col.gameObject.tag != "hostile" && col.gameObject.tag != "Projectile"){
            Rb.AddForce(Vector2.up * 2000f);
        }
    }

    /* Function for following and attacking Player */
    void Attack()
    {
        if(transform.position.x < Player.position.x){
            Rb.velocity = new Vector2(ZombieVelocity, 0f);
        }else if(transform.position.x > Player.position.x ){
            Rb.velocity = new Vector2(-ZombieVelocity, 0f);
        }
    }
}
