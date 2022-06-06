using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /* This script controls the health bar display. */


    public Slider _healthSlider; //slider component of the health bar

    private void Start()
    {
        //get the slider component
        _healthSlider = GetComponent<Slider>(); 
    }

    public void SetMaxHealth(int maxHealth)
    {
        //set slider max value to max player health and current health to max health
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
    }

  public void SetHealth(int health)
    {
        //set slider value to specified health value
        _healthSlider.value = health; 
    }
}
