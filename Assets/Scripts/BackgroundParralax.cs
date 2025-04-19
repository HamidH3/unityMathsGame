using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParralax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam1;
    public float parallaxEffect;
    private float initialY;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        initialY = transform.position.y; // Save original Y position



    }

//    void Update()
//    {
//        float temp = (cam1.transform.position.x * (1 - parallaxEffect));
//        float distance = (cam1.transform.position.x * parallaxEffect);
//        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

//        if (temp > startPos + length)
//        {
//            startPos += length;

//        }
//        else if (temp < startPos - length)
//        {
//            startPos -= length;
//        }
//    }
//}
void Update()
{
    float temp = (cam1.transform.position.x * (1 - parallaxEffect));
    float distance = (cam1.transform.position.x * parallaxEffect);

    // Only change X, keep Y fixed at initialY
    transform.position = new Vector3(startPos + distance, initialY, transform.position.z);

    if (temp > startPos + length)
    {
        startPos += length;
    }
    else if (temp < startPos - length)
    {
        startPos -= length;
    }
}
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BackgroundParralax : MonoBehaviour
//{
//    private float length, startPos;
//    public GameObject cam1;
//    public float parallaxEffect = 0.5f;

//    private float initialY;

//    // Smooth camera follow
//    public Transform player; // Assign your player object here
//    public Vector3 camOffset = new Vector3(0, 0, -10);
//    public float smoothTime = 0.3f;
//    private Vector3 velocity = Vector3.zero;

//    void Start()
//    {
//        startPos = transform.position.x;
//        length = GetComponent<SpriteRenderer>().bounds.size.x;
//        initialY = transform.position.y; // Save original Y position
//    }

//    void LateUpdate()
//    {
//        // Smoothly move the camera (cam1) toward the player with offset
//        if (cam1 != null && player != null)
//        {
//            Vector3 targetPosition = player.position + camOffset;
//            cam1.transform.position = Vector3.SmoothDamp(cam1.transform.position, targetPosition, ref velocity, smoothTime);
//        }

//        // Parallax movement (only in X axis)
//        float temp = (cam1.transform.position.x * (1 - parallaxEffect));
//        float distance = (cam1.transform.position.x * parallaxEffect);
//        transform.position = new Vector3(startPos + distance, initialY, transform.position.z);

//        // Wraparound logic if needed
//        if (temp > startPos + length)
//        {
//            startPos += length;
//        }
//        else if (temp < startPos - length)
//        {
//            startPos -= length;
//        }
//    }
//}
