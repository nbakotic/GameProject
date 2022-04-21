using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public GameObject FallDetector;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update(){
        //moves position of fall detector according to player position
        FallDetector.transform.position = new Vector2(transform.position.x, FallDetector.transform.position.y);
    }

   private void OnCollisionEnter2D(Collision2D collision){

       if (collision.gameObject.CompareTag("Trap")){

           Die();
       }
       if (collision.gameObject.CompareTag("FallDetector")){

           Die();
       }
   }
   private void Die(){
       rb.bodyType = RigidbodyType2D.Static;
       anim.SetTrigger("Death");
   }
   private void RestartLevel(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
}
