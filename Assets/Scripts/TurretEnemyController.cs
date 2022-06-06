using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : MonoBehaviour
{    
    [Header("Transforms")]
    public Rigidbody2D Rb;
    public Transform Player;
    public Transform Gun;

    [Header("Values")]
    public float PlayerDistance; // Distance from Player
    public float AttackRange = 5.0f; // Distance in which turret will start attacking
    public float Recoil = 3.0f; // Time between shoots (in seconds)

    [Header("Bullet attributes")]
    public float TurretBulletVelocity = 1.0f;
    public GameObject TurretBullet;
    
    void Start()
    {
        Rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerDistance = Vector2.Distance(transform.position, Player.transform.position);
        if(PlayerDistance < AttackRange){
            StartCoroutine(Attack());
        }
    }

    /* Function that shoots Turret Bullets at Player */
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Recoil);
        GameObject NewBullet = Instantiate(TurretBullet, Gun.position, Quaternion.identity);
        NewBullet.GetComponent<Rigidbody2D>().velocity = new Vector2();
    }
}
