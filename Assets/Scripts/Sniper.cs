using UnityEngine;

public class Sniper : Weapon
{
    Vector3 startAimVector;
    bool first180Done = false;
    bool slowMoActivated = false;

    [Header("Slow motion")]
    [SerializeField] float slowMoDuration = 1f;
    [SerializeField] float slowMoTimeModf = 0.25f;

    new void Update()
    {
        // Call parent Update method because its overriden
        base.Update();

        // If right mouse clicked
        if (Input.GetMouseButtonDown(1))
        {
            startAimVector = aimVector;
        }

        // If right mouse held down
        if (Input.GetMouseButton(1))
        {
            if (Vector3.Angle(startAimVector, aimVector) > 165f)
            {
                if (!first180Done)
                {
                    first180Done = true;
                    startAimVector = aimVector;
                }
                else if (first180Done && !slowMoActivated)
                {
                    StartSlowMo();
                }
            }
        }
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
    }

    void StartSlowMo()
    {
        startAimVector = aimVector;
        first180Done = false;
        slowMoActivated = true;
        Time.timeScale *= slowMoTimeModf;
        // This fixes the laggy bullet time, but affects the physics
        // Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Invoke("EndSlowMo", slowMoDuration * slowMoTimeModf);
    }

    void EndSlowMo()
    {
        startAimVector = aimVector;
        slowMoActivated = false;
        Time.timeScale = 1f;
        // This fixes the laggy bullet time, but affects the physics
        // Time.fixedDeltaTime = 0.02f;
    }
}
