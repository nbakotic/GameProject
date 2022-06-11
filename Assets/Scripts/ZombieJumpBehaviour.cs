using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieJumpBehaviour : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D Rb;
    public float JumpSpeed = 3.0f;

    void Start()
    {
        Rb = this.GetComponent<Rigidbody2D>();
        Rb.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
    }
}
