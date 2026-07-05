using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Wander,
    Hunt,
    Seek
}

[RequireComponent(typeof(NavMeshAgent))]
public class HunterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public Transform player;

    public float wanderSpeed = 2f;
    public float seekSpeed = 3f;
    public float huntSpeed = 3.5f;

    public float viewDistance = 15f;
    public float viewAngle = 90f;

    public float wanderRadius = 12f;

    public float searchRadius = 6f;
    public float searchTime = 10f;

    private float searchTimer;

    private State currentState = State.Wander;
    private Vector3 lastSeenPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Debug.Log("Player: " + player);
    }

    void Update()
    {
        bool canSeePlayer = CanSeePlayer();
        animator.SetBool("SeesPlayer", canSeePlayer);

        switch(currentState)
        {
            case State.Wander:
                if (canSeePlayer)
                {
                    ChangeState(State.Hunt);
                }
                else
                {
                    Wander();
                }
                break;

            case State.Hunt:
                if (canSeePlayer)
                {
                    lastSeenPosition = player.position;
                    agent.SetDestination(player.position);
                }
                else
                {
                    ChangeState(State.Seek);
                }
                break;

            case State.Seek:
                if (canSeePlayer)
                {
                    ChangeState(State.Hunt);
                }
                else
                {
                    Seek();
                }
                break;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;

        if(direction.magnitude > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, direction);

        if(angle > viewAngle * 0.5f)
            return false;

        RaycastHit hit;

        Vector3 eye = transform.position + Vector3.up * 1.5f;

        if(Physics.Raycast(eye, direction.normalized, out hit, viewDistance))
        {
            if(hit.transform == player)
            {
                lastSeenPosition = player.position;
                return true;
            }
        }
        return false;
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        switch(newState)
        {
            case State.Wander:
                agent.speed = wanderSpeed;
                animator.SetBool("RecentlySeenPlayer", false);
                break;

            case State.Hunt:
                agent.speed = huntSpeed;
                animator.SetBool("RecentlySeenPlayer", true);
                break;

            case State.Seek:
                searchTimer = searchTime;
                agent.SetDestination(lastSeenPosition);
                agent.speed = seekSpeed;
                animator.SetBool("RecentlySeenPlayer", true);
                break;
        }
    }

    void Seek()
    {
        searchTimer -= Time.deltaTime;

        if(searchTimer <= 0)
        {
            ChangeState(State.Wander);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            Vector3 target = ConeWanderPoint(Random.Range(-90f, 90f), searchRadius);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(target, out hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void Wander()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            Vector3 target = ConeWanderPoint(Random.Range(-45f, 45f), wanderRadius);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(target, out hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    Vector3 ConeWanderPoint(float angle, float radius)
    {
        Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
        Vector3 randomDistance = direction * Random.Range(3f, radius);
        return transform.position + randomDistance;
    }
}
