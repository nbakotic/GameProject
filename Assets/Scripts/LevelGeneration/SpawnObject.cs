using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        /* Takes an object from the object array and instantiates it as a child of the parent object.
         * This ensures that when we're spawning random objectsk they're still considered as a whole.
         */
        int rand = Random.Range(0, objects.Length);
        GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
