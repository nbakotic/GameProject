using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    
    [Header("Movement variables")]
    public float enemyVelocity = 2.0f;


    


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        rb.velocity = new Vector2(enemyVelocity, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.gameObject.tag != "Player"){
                enemyVelocity *= -1;
        }
    }

    

}