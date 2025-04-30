using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDoor : MonoBehaviour
{
    public float openSpeed = 2f;
    public Vector3 openToTheRight = new Vector3(3f, 0f, 0f);
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool hasPlayed = false;
    public AudioSource groundOpening;
    // Start is called before the first frame update
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openToTheRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);

        }
    }

    public void OpenCave()
    {
        isOpening = true;
        if (!hasPlayed)
        {
            hasPlayed = true;
            if (groundOpening != null && !groundOpening.isPlaying)
            {
                groundOpening.Play();
            }
        }
    }
    public void ResetCaveDoor()
    {
        transform.position = closedPosition;
        isOpening = false;
        hasPlayed = false;

        if (groundOpening != null && groundOpening.isPlaying)
        {
            groundOpening.Stop();
        }
    }
}
