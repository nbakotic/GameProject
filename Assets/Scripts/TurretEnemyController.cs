using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : MonoBehaviour
{    
    private bool CanAttack;

    [Header("Transforms")]
    public Rigidbody2D Rb;
    public Transform Player;
    public Transform Gun;

    [Header("Values")]
    public float PlayerDistance; // Distance from Player
    public float AttackRange = 10.0f; // Distance in which turret will start attacking
    public float Recoil = 1.0f; // Time between shoots

    [Header("Bullet attributes")]
    public float TurretBulletVelocity = 10;
    public GameObject TurretBullet;
    
    void Start()
    {
        Rb = this.GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        CanAttack = true;
    }

    void Update()
    {
        PlayerDistance = Vector2.Distance(transform.position, Player.transform.position);
        if(PlayerDistance < AttackRange && CanAttack){
            StartCoroutine(Attack());
        }
    }

    /* Function that shoots Turret Bullets at Player */
    IEnumerator Attack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(Recoil);
        GameObject NewBullet = Instantiate(TurretBullet, Gun.transform.position, Gun.transform.rotation);
        Vector3 bulletDirection = Player.transform.position - NewBullet.transform.position;
        NewBullet.GetComponent<Rigidbody2D>().velocity = bulletDirection.normalized * TurretBulletVelocity;
        CanAttack = true;
    }
}
