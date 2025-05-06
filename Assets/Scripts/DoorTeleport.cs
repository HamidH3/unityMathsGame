using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public DoorTeleport goToDoor;
    public Transform exitPoint;
    
    //transforms position of player when they enter door
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (goToDoor != null)
            {
                Vector3 targetPos = goToDoor.exitPoint != null ? goToDoor.exitPoint.position : goToDoor.transform.position;
                other.transform.position = targetPos;

                Rigidbody2D body = other.GetComponent<Rigidbody2D>();
                if (body != null) {
                    body.velocity = Vector2.zero;
                }
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Flip();
                }
            }
            

        }
    }
}
