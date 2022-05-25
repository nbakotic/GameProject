using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun")]
    [SerializeField] float knockbackForce = 1000f;
    [SerializeField] int pelletsPerShot = 5;
    [SerializeField] float bloomAmount = 0.01f;

    protected override void Shoot()
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Randomly offset aimVector using bloomAmount
            Vector3 bloomedAimVector = aimVector;
            bloomedAimVector.x = Random.Range(aimVector.x - bloomAmount, aimVector.x + bloomAmount);
            bloomedAimVector.y = Random.Range(aimVector.y - bloomAmount, aimVector.y + bloomAmount);

            // Create new projectile object and give it force
            GameObject projectileInst = Instantiate(projectile, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
            projectileInst.GetComponent<Rigidbody2D>().AddForce(bloomedAimVector * projectileLaunchVelocity);

            if (despawnProjectileOnImpact)
            {
                // Add a script to the projectile that despawns it on impact
                projectileInst.AddComponent<Projectile>();
            }
        }

        // Apply knockback to player
        GetComponentInParent<Rigidbody2D>().AddForce(knockbackForce * new Vector3(-aimVector.x, -aimVector.y, aimVector.z));
    }
}
