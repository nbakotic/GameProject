using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject projectileOrigin;

    public GameObject projectile;
    public float projectileVelocity;
    public bool despawnProjectileOnImpact = true;

    void Start()
    {
        projectileOrigin = transform.Find("ProjectileOrigin").gameObject;
    }

    void Update()
    {
        // Get the position of the mouse in the world coordinates
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(screenMousePos);

        Vector3 aimVector = Vector3.Normalize(worldMousePos - transform.parent.transform.position);

        // Rotate the weapon object towards the aim position
        transform.localRotation = Quaternion.FromToRotation(Vector3.right, aimVector);

        // If left mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create new projectile object and give it force
            GameObject projectileInst = Instantiate(projectile, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
            projectileInst.GetComponent<Rigidbody2D>().AddForce(aimVector * projectileVelocity);

            // If flag "despawnProjectileOnImpact" is enabled add a script to the projectile that despawns it on impact
            if (despawnProjectileOnImpact)
            {
                projectileInst.AddComponent<Projectile>();
            }
        }
    }
}
