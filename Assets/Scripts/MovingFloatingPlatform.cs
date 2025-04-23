

//using UnityEngine;

//public class MovingFloatingPlatform : MonoBehaviour
//{
//    public enum MovementType { horizontal, vertical }
//    public MovementType movementType;

//    [SerializeField] private float moveSpeed = 2f;
//    [SerializeField] private float moveDistance = 3f;
//    public float moveDirection = 1f;

//    private Vector3 startPos;
//    private Vector3 endPos;
//    //private bool movingRight = true;

//    private Vector3 lastPos;
//    public Vector3 deltaMovement;

//    void Start()
//    {
//        startPos = transform.position;
//        //endPos = startPos + new Vector3(moveDistance, 0f, 0f);
//        lastPos = transform.position;

//        SetTargetPos();
//    }

//    void Update()
//    {
//        switch (movementType)
//        {
//            case MovementType.horizontal:
//                MoveHorizontally();
//                break;
//            case MovementType.vertical:
//                MoveVertically();
//                break;
//        }

//        // Move platform back and forth
//        //float step = moveSpeed * Time.deltaTime;
//        //if (movingRight)
//        //{
//        //    transform.position = Vector3.MoveTowards(transform.position, endPos, step);
//        //    if (Vector3.Distance(transform.position, endPos) < 0.01f)
//        //        movingRight = false;
//        //}
//        //else
//        //{
//        //    transform.position = Vector3.MoveTowards(transform.position, startPos, step);
//        //    if (Vector3.Distance(transform.position, startPos) < 0.01f)
//        //        movingRight = true;
//        //}

//        // Calculate delta movement
//        deltaMovement = transform.position - lastPos;
//        lastPos = transform.position;

//        // Debug log to confirm movement
//        //Debug.Log("Platform Delta Movement: " + deltaMovement);
//    }

//    private void MoveHorizontally()
//    {
//        float step = moveSpeed * Time.deltaTime;
//        if (transform.position.x >= lastPos.x || transform.position.x <= startPos.x)
//        {
//            moveDirection = -moveDirection;
//        }
//        transform.position += new Vector3(moveDirection * step, 0f, 0f);
//    }

//    private void MoveVertically()
//    {
//        float step = moveSpeed * Time.deltaTime;
//        if (transform.position.y >= lastPos.y || transform.position.y <= startPos.y)
//        {
//            moveDirection = -moveDirection;
//        }
//        transform.position += new Vector3(0f, moveDirection * step, 0f);
//    }

//    private void SetTargetPos()
//    {
//        if (movementType == MovementType.horizontal)
//        {
//            endPos = startPos + new Vector3(moveDistance, 0f, 0f);
//        }
//        else if (movementType == MovementType.vertical)
//        {
//            endPos = startPos + new Vector3(0f, moveDistance, 0f);
//        }

//    }
//}

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

        // Set the target position based on movement type
        SetTargetPos();

        // Ensure that the platform starts moving immediately
        lastPos = transform.position;

        // Start moving the platform right away
        if (movementType == MovementType.horizontal)
        {
            // Adjust for an immediate move, even if it starts near the end
            transform.position = startPos + new Vector3(0.1f, 0f, 0f);
        }
        else if (movementType == MovementType.vertical)
        {
            // Adjust for an immediate move, even if it starts near the end
            transform.position = startPos + new Vector3(0f, 0.1f, 0f);
        }
    }

    void Update()
    {
        // Call the appropriate movement function
        switch (movementType)
        {
            case MovementType.horizontal:
                MoveHorizontally();
                break;
            case MovementType.vertical:
                MoveVertically();
                break;
        }

        // Calculate delta movement
        deltaMovement = transform.position - lastPos;
        lastPos = transform.position;
    }

    //private void MoveHorizontally()
    //{
    //    float step = moveSpeed * Time.deltaTime;

    //    // Reverse direction when the platform reaches the target
    //    if (transform.position.x >= endPos.x || transform.position.x <= startPos.x)
    //    {
    //        moveDirection = -moveDirection;  // Reverse direction
    //    }
    //    transform.position += new Vector3(moveDirection * step, 0f, 0f);
    //}

    //private void MoveVertically()
    //{
    //    float step = moveSpeed * Time.deltaTime;

    //    // Reverse direction when the platform reaches the target
    //    if (transform.position.y >= endPos.y || transform.position.y <= startPos.y)
    //    {
    //        moveDirection = -moveDirection;  // Reverse direction
    //    }
    //    transform.position += new Vector3(0f, moveDirection * step, 0f);
    //}
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
            endPos = startPos + new Vector3(moveDistance, 0f, 0f);  // Move horizontally
        }
        else if (movementType == MovementType.vertical)
        {
            endPos = startPos + new Vector3(0f, moveDistance, 0f);  // Move vertically
        }
    }
}
