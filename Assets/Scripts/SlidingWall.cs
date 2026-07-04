using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 0, 8f);
    public float moveSpeed = 8f;


    public float openTime = 3f;
    public float closedTime = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;

    private Vector3 targetPos;
    private float timer;
    private bool isOpen = false;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + openOffset;

        targetPos = closedPos;
        timer = closedTime;
    }

    void Update()
    {
        // Move smoothly toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Timer logic
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                targetPos = openPos;
                timer = openTime;
            }
            else
            {
                targetPos = closedPos;
                timer = closedTime;
            }
        }
    }
}
