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
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(screenMousePos);
        transform.localRotation = Quaternion.FromToRotation(Vector3.right, worldMousePos - transform.parent.transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject projectileInst = Instantiate(projectile, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
            projectileInst.GetComponent<Rigidbody2D>().AddForce((worldMousePos - transform.parent.transform.position) * projectileVelocity);
            if (despawnProjectileOnImpact)
            {
                projectileInst.AddComponent<Projectile>();
            }
        }
    }
}
