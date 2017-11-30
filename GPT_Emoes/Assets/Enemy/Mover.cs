using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    public float DestinationAccuracy = 1f;
    public float DestinationCheckTickTime = 0.25f;
    public UnityEvent OnDestinationReached;
    public UnityEvent OnStartMoving;

    private bool destinationReached = false;
    private NavMeshAgent agent;
    private Vector3 moveDestination = Vector3.zero;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetMoveSpeed(float speed)
    {
        agent.speed = speed;
    }

    public bool IsMoving()
    {
        return agent.velocity != Vector3.zero;
    }

    public Vector3 GetVelocityNormalized()
    {
        return agent.velocity.normalized;
    }

    /// <summary>
    /// Start moving to the new location. Fires DestinationReached event when destination is reached.
    /// </summary>
    /// <param name="location"></param>
    public void MoveToLocation(Vector3 location)
    {
        moveDestination = location;
        agent.SetDestination(moveDestination);
        OnStartMoving.Invoke();

        StopCoroutine("CheckDestinationReached");
        StartCoroutine("CheckDestinationReached");
        //Debug.Log(name + " is moving to " + location);
    }

    /// <summary>
    /// Instantly move to the new location and fire the DestinationReached event.
    /// </summary>
    /// <param name="location"></param>
    public void TeleportToLocation(Vector3 location)
    {
        moveDestination = location;
        //transform.position = location;
        agent.Warp(location);

        StopCoroutine("CheckDestinationReached");
        StartCoroutine("CheckDestinationReached");
        Debug.Log(name + " teleported to " + location);
    }

    /// <summary>
    /// Checks if the current transform position is close enough to the destination and updates the destinationReached bool accordingly.
    /// Fires OnDestinationReached event when destination reached
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckDestinationReached()
    {
        destinationReached = false;

        while (Vector2.Distance(transform.position, moveDestination) > DestinationAccuracy)
        {
            //Debug.Log("Mover: Destination not in range");
            yield return new WaitForSeconds(DestinationCheckTickTime);
        }

        destinationReached = true;
        OnDestinationReached.Invoke();
        //Debug.Log("Mover: Destination reached!");
    }

    /// <summary>
    /// Returns the total distance of the path to the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public float GetPathDistanceToTarget(Transform target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target.position, path);
        float totalDistance = 0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
            totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);

        return totalDistance;
    }
}
