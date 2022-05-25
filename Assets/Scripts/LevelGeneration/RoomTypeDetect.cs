using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypeDetect : MonoBehaviour
{
    [Header("Room Type Variables")]
    public int type;

    /* Destroys a room game object*/
    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
