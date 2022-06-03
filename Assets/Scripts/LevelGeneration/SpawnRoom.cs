using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] private LayerMask _whatIsRoom;

    [Header("Level Generator Variables")]
    [SerializeField] private LevelGeneration _levelGen;
    void Update()
    {
        //A collider that detects other rooms
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, _whatIsRoom);

        /*If no room is detected at a generation point and generation is still active
         *instantiate a room from the rooms array
         *Then destroy the generator game object so it doesn't spawn rooms infinitely.
         */
        if (roomDetection == null && _levelGen._stopGeneration)
        {
            int rand = Random.Range(0, _levelGen.roomsArray.Length);
            Instantiate(_levelGen.roomsArray[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
