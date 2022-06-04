using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager { get; private set; } //define getter and setter for game manager

    public HealthSystem _playerHealth = new HealthSystem(100,100); //set global values for player max health and start health

    /* 
     * Checks whether another game manager object exists and destroys it if it does
     * Else it assigns the game manager in the scene to this object
     */
    void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        } else
        {
            gameManager = this;
        }
    }

    /* Restarts level */
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
