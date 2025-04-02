using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParralax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam1;
    public float parallaxEffect;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;


    }

    void Update()
    {
        float temp = (cam1.transform.position.x * (1 - parallaxEffect));
        float distance = (cam1.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

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
