using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected GameObject projectileOrigin;
    protected Vector3 aimVector;
    protected bool canShoot = true;

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float projectileLaunchVelocity;
    [SerializeField] protected int shotsPerSecond;
    [SerializeField] protected bool despawnProjectileOnImpact = true;

    protected void Start()
    {
        projectileOrigin = transform.Find("ProjectileOrigin").gameObject;
    }

    protected void Update()
    {
        // Get the position of the mouse in the world coordinates
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(screenMousePos);

        aimVector = Vector3.Normalize(worldMousePos - transform.parent.transform.position);

        // Rotate the weapon object towards the aim position
        transform.localRotation = Quaternion.FromToRotation(Vector3.right, aimVector);

        // If left mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            PullTrigger();
        }

    }

    void PullTrigger()
    {
        if (!canShoot) { return; }

        // Call method in subclass
        Shoot();

        canShoot = false;
        Invoke("EnableFire", 1f / shotsPerSecond);
    }

    void EnableFire()
    {
        canShoot = true;
    }

    // Method that is implemented inside sublasses
    protected virtual void Shoot() { }
}