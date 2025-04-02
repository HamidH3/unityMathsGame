using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public Transform player;  // Drag the player GameObject here
    public Vector3 offset = new Vector3(0, 2, -10); // Adjust camera position

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
