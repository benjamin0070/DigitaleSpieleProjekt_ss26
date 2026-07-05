using UnityEngine;
using UnityEngine.AI;

public enum HunterState
{
    Wander,
    Hunt,
    Seek
}

[RequireComponent(typeof(NavMeshAgent))]
public class FlockHunter : MonoBehaviour
{

    private NavMeshAgent agent;

    public Vector3 Velocity { get; private set; } = Vector3.forward;
    public HunterState CurrentState { get; private set; } = HunterState.Wander;

    [Header("View")]
    public float viewDistance = 15f;
    public float viewAngle = 90f;

    [Header("Speed")]
    public float wanderMinSpeed = 1f;
    public float wanderMaxSpeed = 2f;
    public float huntMinSpeed = 3f;
    public float huntMaxSpeed = 4.5f;
    public float seekMinSpeed = 2.5f;
    public float seekMaxSpeed = 3.5f;

    [Header("Weights")]
    public float huntWeight = 0.15f;
    public float seekWeight = 0.1f;

    [Header("Wander")]
    public float wanderJitterPower = 0.4f;
    public float wanderChangeInterval = 2f;

    [Header("Seek")]
    public float searchTime = 10f;

    [Header("Turn")]
    public float turnSpeed = 8f;

    private float searchTimer;
    private float wanderTimer;
    private Vector3 wanderTargetDirection;
    private Vector3 lastSeenPosition;
    private FlockAgent targetAgent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        wanderTargetDirection = transform.forward;
    }

    void OnEnable() {
        FlockManager.Register(this);
    }
    void OnDisable() {
        FlockManager.Unregister(this);
    }

    void Update()
    {
        bool canSeeTarget = CanSeeNearestFlockAgent();
        Vector3 velocity = Velocity;
        float minSpeed;
        float maxSpeed;

        switch (CurrentState)
        {
            case HunterState.Wander:
                if (canSeeTarget)
                {
                    ChangeState(HunterState.Hunt);
                }
                velocity += Wander();
                minSpeed = wanderMinSpeed;
                maxSpeed = wanderMaxSpeed;
                break;

            case HunterState.Hunt:
                if (canSeeTarget && targetAgent != null)
                {
                    lastSeenPosition = targetAgent.transform.position;
                    velocity += Seek(targetAgent.transform.position, huntWeight);
                    minSpeed = huntMinSpeed;
                    maxSpeed = huntMaxSpeed;
                }
                else
                {
                    ChangeState(HunterState.Seek);
                    minSpeed = seekMinSpeed;
                    maxSpeed = seekMaxSpeed;
                }
                break;

            case HunterState.Seek:
                if (canSeeTarget)
                {
                    ChangeState(HunterState.Hunt);
                    velocity += Seek(targetAgent != null ? targetAgent.transform.position : lastSeenPosition, huntWeight);
                    minSpeed = huntMinSpeed;
                    maxSpeed = huntMaxSpeed;
                }
                else
                {
                    searchTimer -= Time.deltaTime;
                    if (searchTimer <= 0f)
                    {
                        ChangeState(HunterState.Wander);
                        velocity += Wander();
                        minSpeed = wanderMinSpeed;
                        maxSpeed = wanderMaxSpeed;
                    }
                    else
                    {
                        velocity += Seek(lastSeenPosition, seekWeight);
                        minSpeed = seekMinSpeed;
                        maxSpeed = seekMaxSpeed;
                    }
                }
                break;

            default:
                minSpeed = wanderMinSpeed;
                maxSpeed = wanderMaxSpeed;
                break;
        }

        Velocity = FlockManager.ClampSpeed(velocity, minSpeed, maxSpeed, transform.forward);
        agent.Move(Velocity * Time.deltaTime);
        FaceVelocity();
    }

    void ChangeState(HunterState newState)
    {
        CurrentState = newState;
        if (newState == HunterState.Seek)
            searchTimer = searchTime;
    }

    // Seek
    Vector3 Seek(Vector3 targetPosition, float power)
    {
        Vector3 delta = targetPosition - transform.position;
        return delta * power;
    }

    // Wander
    Vector3 Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            wanderTimer = wanderChangeInterval;
            Vector3 baseDir = Velocity.sqrMagnitude > 0.0001f ? Velocity.normalized : transform.forward;
            float angle = Random.Range(-70f, 70f);
            wanderTargetDirection = Quaternion.Euler(0f, angle, 0f) * baseDir;
        }

        return wanderTargetDirection * wanderJitterPower;
    }

    bool CanSeeNearestFlockAgent()
    {
        FlockAgent candidate = FlockManager.GetNearestFlockAgent(transform.position, out float distance);
        if (candidate == null || distance > viewDistance)
        {
            targetAgent = null;
            return false;
        }

        Vector3 direction = candidate.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > viewAngle * 0.5f)
        {
            targetAgent = null;
            return false;
        }

        Vector3 eye = transform.position + Vector3.up * 1.5f;
        if (Physics.Raycast(eye, direction.normalized, out RaycastHit hit, viewDistance))
        {
            if (hit.transform == candidate.transform)
            {
                targetAgent = candidate;
                lastSeenPosition = candidate.transform.position;
                return true;
            }
        }

        targetAgent = null;
        return false;
    }


    // Roatate Agent (Cube) to face the moving direction. Part of RenderBoid()
    void FaceVelocity()
    {
        if (Velocity.sqrMagnitude < 0.0001f)
            return;
        Quaternion targetRotation = Quaternion.LookRotation(Velocity.normalized, Vector3.up);
        transform.rotation = Quaternion.LookRotation(Velocity.normalized, Vector3.up);
    }
}
