using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;

public class HealthSystemTests 
{
    [Test]
    public void HealthSystem_NegativeDamageParam_Exception()
    {
        HealthSystem healthSystem = new HealthSystem(30, 30);

        Assert.Throws<ArgumentOutOfRangeException>(() => healthSystem.TakeDamage(-10));
    }

    [Test]
    public void HealthSystem_NegativeHealParam_Exception()
    {
        HealthSystem healthSystem = new HealthSystem(30, 30);

        Assert.Throws<ArgumentOutOfRangeException>(() => healthSystem.Heal(-10));
    }

    [Test]
    public void HealthSystem_TakeDamage()
    {
        HealthSystem healthSystem = new HealthSystem(30, 30);
        healthSystem.TakeDamage(10);

        Assert.AreEqual(20, healthSystem.Health);
    }

    [Test]
    public void HealthSystem_Heal()
    {
        HealthSystem healthSystem = new HealthSystem(10, 30);
        healthSystem.Heal(10);

        Assert.AreEqual(20, healthSystem.Health);
    }

    [Test]
    public void HealthSystem_HealthAboveZero()
    {
        HealthSystem healthSystem = new HealthSystem(30, 30);
        healthSystem.TakeDamage(40);

        Assert.AreEqual(0, healthSystem.Health);
    }

    [Test]
    public void HealthSystem_HealthBelowMax()
    {
        HealthSystem healthSystem = new HealthSystem(20, 30);
        healthSystem.Heal(40);

        Assert.AreEqual(healthSystem.MaxHealth, healthSystem.Health);
    }
}
