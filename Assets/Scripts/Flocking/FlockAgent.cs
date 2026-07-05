using UnityEngine;
using UnityEngine.AI;

public enum FlockState
{
    Flock,
    Flee
}

[RequireComponent(typeof(NavMeshAgent))]
public class FlockAgent : MonoBehaviour
{

    private NavMeshAgent agent;

    public Vector3 Velocity { get; private set; } = Vector3.forward;
    public FlockState CurrentState { get; private set; } = FlockState.Flock;

    [Header("Rule 1: Cohesion")]
    public float cohesionDistance = 8f;
    public float cohesionWeight = 0.03f;

    [Header("Rule 2: Alignment")]
    public float alignmentDistance = 2.5f;
    public float alignmentWeight = 0.06f;

    [Header("Rule 3: Separation")]
    public float separationDistance = 2.5f;
    public float separationWeight = 0.5f;

    [Header("Rule 4: Speed Limits")]
    public float flockMinSpeed = 1f;
    public float flockMaxSpeed = 3f;
    public float fleeMinSpeed = 3.5f;
    public float fleeMaxSpeed = 6f;

    [Header("Rule 6: Hunter Avoidance")]
    public float fleeTriggerDistance = 4f;
    public float fleeEndDistance = 8f;
    public float hunterAvoidDistance = 6f;
    public float hunterAvoidWeight = 1.2f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    void OnEnable() {
        FlockManager.Register(this);
    }
    void OnDisable() {
        FlockManager.Unregister(this);
    }

    void Update()
    {
        FlockHunter nearestHunter = FlockManager.GetNearestHunter(transform.position, out float hunterDistance);
        UpdateState(nearestHunter, hunterDistance);

        Vector3 velocity = Velocity;
        velocity += Cohesion();
        velocity += Alignment();
        velocity += Separation();

        float minSpeed = flockMinSpeed;
        float maxSpeed = flockMaxSpeed;

        if (CurrentState == FlockState.Flee && nearestHunter != null)
        {
            velocity += AvoidHunter(nearestHunter);
            minSpeed = fleeMinSpeed;
            maxSpeed = fleeMaxSpeed;
        }

        Velocity = FlockManager.ClampSpeed(velocity, minSpeed, maxSpeed, transform.forward);
        agent.Move(Velocity * Time.deltaTime);
        FaceVelocity();
    }

    void UpdateState(FlockHunter nearestHunter, float hunterDistance)
    {
        switch (CurrentState)
        {
            case FlockState.Flock:
                if (nearestHunter != null && hunterDistance <= fleeTriggerDistance)
                    CurrentState = FlockState.Flee;
                break;
            
            case FlockState.Flee:
                if (nearestHunter == null || hunterDistance >= fleeEndDistance)
                    CurrentState = FlockState.Flock;
                break;
        }
    }

    // Rule 1
    Vector3 Cohesion()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var other in FlockManager.FlockAgents)
        {
            if (other == this || other == null)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < cohesionDistance)
            {
                sum += other.transform.position;
                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        Vector3 meanPosition = sum / count;
        Vector3 deltaCenter = meanPosition - transform.position;
        return deltaCenter * cohesionWeight;
    }

    // Rule 2
    Vector3 Alignment()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var other in FlockManager.FlockAgents)
        {
            if (other == this || other == null)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < alignmentDistance)
            {
                sum += other.Velocity;
                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        Vector3 meanVelocity = sum / count;
        Vector3 deltaVelocity = meanVelocity - Velocity;
        return deltaVelocity * alignmentWeight;
    }

    // Rule 3
    Vector3 Separation()
    {
        Vector3 sum = Vector3.zero;

        foreach (var other in FlockManager.FlockAgents)
        {
            if (other == this || other == null)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < separationDistance && distance > 0.0001f)
            {
                float closeness = separationDistance - distance;
                sum += (transform.position - other.transform.position) * closeness;
            }
        }

        return sum * separationWeight;
    }

    // Rule 6
    Vector3 AvoidHunter(FlockHunter hunter)
    {
        float distance = Vector3.Distance(transform.position, hunter.transform.position);
        if (distance >= hunterAvoidDistance)
            return Vector3.zero;

        float closeness = hunterAvoidDistance - distance;
        return (transform.position - hunter.transform.position) * closeness * hunterAvoidWeight;
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
