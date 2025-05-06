using UnityEngine;

public class MovingFloatingPlatform : MonoBehaviour
{
    public enum MovementType { horizontal, vertical }
    public MovementType movementType;

    private float moveSpeed = 1f;
    private float moveDistance = 2f;
    [SerializeField] private float timeOffset = 0f;

    public float moveDirection = 1f;

    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 lastPos;
    public Vector3 deltaMovement;




    void Start()
    {
        startPos = transform.position;

        SetTargetPos();

        // ensure that the platform starts moving immediately
        lastPos = transform.position;

        if (movementType == MovementType.horizontal)
        {
            transform.position = startPos + new Vector3(0.1f, 0f, 0f);
        }
        else if (movementType == MovementType.vertical)
        {
            transform.position = startPos + new Vector3(0f, 0.1f, 0f);
        }
    }

    void Update()
    {
        // call the appropriate movement function
        switch (movementType)
        {
            case MovementType.horizontal:
                MoveHorizontally();
                break;
            case MovementType.vertical:
                MoveVertically();
                break;
        }

        // calculate delta movement
        deltaMovement = transform.position - lastPos;
        lastPos = transform.position;
    }

    
    private void MoveHorizontally()
    {
        float offset = Mathf.Sin((Time.time + timeOffset) * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }

    private void MoveVertically()
    {
        float offset = Mathf.Sin((Time.time + timeOffset) * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
    }

    private void SetTargetPos()
    {
        if (movementType == MovementType.horizontal)
        {
            endPos = startPos + new Vector3(moveDistance, 0f, 0f);  
        }
        else if (movementType == MovementType.vertical)
        {
            endPos = startPos + new Vector3(0f, moveDistance, 0f); 
        }
    }
}
