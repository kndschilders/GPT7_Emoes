using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToLocation(Vector3 location)
    {
        agent.SetDestination(location);
        Debug.Log(name + " is moving to " + location);
    }

    public void TeleportToLocation(Vector3 location)
    {
        transform.position = location;
        Debug.Log(name + " teleported to " + location);

    }
}
