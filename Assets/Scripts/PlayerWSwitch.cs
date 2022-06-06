using System.Collections.Generic;
using UnityEngine;

public class PlayerWSwitch : MonoBehaviour
{
    [SerializeField] float throwPower;
    [SerializeField] float throwTorque;

    List<GameObject> nearbyWeapons = new List<GameObject>();


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WorldWeapon")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "WorldWeapon")
        {
            if (!nearbyWeapons.Contains(collider.gameObject))
            {
                nearbyWeapons.Add(collider.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "WorldWeapon")
        {
            if (nearbyWeapons.Contains(collider.gameObject))
            {
                nearbyWeapons.Remove(collider.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (nearbyWeapons.Count > 0)
            {
                SwitchWeapon();
            }
        }
    }

    void SwitchWeapon() {
        GameObject weaponToPickup = nearbyWeapons[0];
        string weaponToPickupName = weaponToPickup.name.Replace("World", "");

        GameObject newPickup = (GameObject)Instantiate(Resources.Load("Weapons/" + weaponToPickupName), transform);
        newPickup.name = weaponToPickupName;

        nearbyWeapons.RemoveAt(0);
        Destroy(weaponToPickup);


        GameObject weaponToDrop = transform.GetChild(0).gameObject;
        string weaponToDropName = weaponToDrop.name + "World";

        GameObject newDrop = (GameObject)Instantiate(Resources.Load("Weapons/" + weaponToDropName), transform.position, Quaternion.identity);
        newDrop.name = weaponToDropName;
        newDrop.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 1, 0) * throwPower);
        newDrop.GetComponent<Rigidbody2D>().AddTorque(throwTorque);
        Physics2D.IgnoreCollision(newDrop.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Destroy(weaponToDrop);
    }
}
