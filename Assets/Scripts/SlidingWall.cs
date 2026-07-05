using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    public Vector3 destinationOffset = new Vector3(0, 0, 8f);
    public float moveSpeed = 8f;

    public float initialPositionWaitTime = 3f;
    public float destinationPositionWaitTime = 3f;

    private Vector3 initialPos;
    private Vector3 destinationPos;
    private Vector3 targetPos;

    private float maxTimer;
    private bool isAtTarget = true;

    void Start()
    {
        initialPos = transform.position;
        destinationPos = initialPos + destinationOffset;
        targetPos = destinationPos;

        maxTimer = 0f;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        maxTimer -= Time.deltaTime;

        if (maxTimer <= 0f)
        {
            isAtTarget = !isAtTarget;

            if (isAtTarget)
            {
                targetPos = initialPos;
                maxTimer = destinationPositionWaitTime;
            }
            else
            {
                targetPos = destinationPos;
                maxTimer = initialPositionWaitTime;
            }
        }
    }
}
