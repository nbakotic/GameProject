using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthSystem
{
    //Fieds
    int _currentHealth; //tracks current health of a game object
    int _maxHealth; //contains max health of a game object

    //Properties
    public HealthBar healthBar; //references a health bar object

    /* Health getter and setter */
    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    /* Max health getter and setter */
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    //Constructor
    public HealthSystem(int health, int maxHealth) {
        _currentHealth = health;
        _maxHealth = maxHealth;
    }

    //Methods
    /* Subtract an amount of health (damage) from current health*/
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
    }

    /* Add an amount of health (healing) to current health*/
    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}
