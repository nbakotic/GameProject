using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemyController : MonoBehaviour
{    
    [Header("Components")]
    public Transform Player;
    private Rigidbody2D _rb;
    [Header("Movement variables")]
    public float enemyVelocity = 5.0f;
    
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 Dir = Player.position - transform.position;
        Dir.Normalize();
        Vector2 simpleDir = Dir;
        _rb.MovePosition((Vector2)transform.position + simpleDir * enemyVelocity * Time.deltaTime);
    }
}
