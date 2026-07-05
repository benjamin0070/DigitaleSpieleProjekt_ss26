using UnityEngine;

public class SlidingTurningWall : MonoBehaviour
{
    public Vector3 destinationOffset = new Vector3(0, 0, 8f);
    public float moveSpeed = 8f;

    public float initialPositionWaitTime = 3f;
    public float destinationPositionWaitTime = 3f;

    private Vector3 initialPos;
    private Vector3 destinationPos;
    private Vector3 targetPos;

    private float maxTimer;
    private bool isAtDestinationPos = false;

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
            isAtDestinationPos = !isAtDestinationPos;

            if (isAtDestinationPos)
            {
                targetPos = initialPos;
                maxTimer = destinationPositionWaitTime;
                transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                targetPos = destinationPos;
                maxTimer = initialPositionWaitTime;
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }
}
