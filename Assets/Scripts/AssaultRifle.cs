using UnityEngine;

public class AssaultRifle : Weapon
{
    [SerializeField] int burstRoundCount = 3;
    int roundsLeft;

    new void Start()
    {
        // Call parent Update method because its overriden
        base.Start();

        roundsLeft = burstRoundCount;
    }

    protected override void Shoot()
    {
        // Create new projectile object and give it force
        GameObject projectileInst = Instantiate(projectile, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
        projectileInst.GetComponent<Rigidbody2D>().AddForce(aimVector * projectileLaunchVelocity);

        if (despawnProjectileOnImpact)
        {
            // Add a script to the projectile that despawns it on impact
            projectileInst.AddComponent<Projectile>();
        }

        roundsLeft--;
        if (roundsLeft > 0)
        {
            Invoke("Shoot", 0.05f);
        }
        else
        {
            roundsLeft = burstRoundCount;
        }
    }
}
