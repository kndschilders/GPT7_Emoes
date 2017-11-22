using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Simulates player behavior to test the AI enemy
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerSimulator : MonoBehaviour
{
    public static bool IsBeingChased = false;
    public FloatVariable StressLevel;
    public Vector3Variable LastPos;
    public GameEvent PresenceUpdate;
    public float SimulationTickTime = 1f;
    [Range(0f, 1f)]
    public float MinStressLevel, MaxStressLevel;

    private GameObject[] movePoints;
    private NavMeshAgent agent;
    private bool isFindingHideSpot = false;
    private HideSpotScript hideSpot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        movePoints = GameObject.FindGameObjectsWithTag("Destination");
    }

    private void Start()
    {
        // Start the simulation tick
        InvokeRepeating("SimulationTick", 0f, SimulationTickTime);
    }

    private void SimulationTick()
    {
        // Find hiding spot
        if (IsBeingChased)
            FindHideSpot();
        // Move
        else if (Random.Range(0f, 1f) < 0.5f)
            Move();

        // Randomize stress level
        RandomizeStressLevel();

        // React to stress level
        ReactToStressLevel();

    }

    /// <summary>
    /// Updates LastPos to a random entry from MovePoints
    /// </summary>
    private void Move()
    {
        if (movePoints.Length == 0)
        {
            Debug.LogWarning("No move points assigned!");
            return;
        }
        agent.SetDestination(RandomUtil.RandomElement(movePoints).transform.position);
        Debug.Log("Player moving to " + agent.destination);
    }

    private void FindHideSpot()
    {
        if (isFindingHideSpot)
            return;

    }

    private void ExitHideSpot()
    {

    }

    /// <summary>
    /// Randomize stress level, picking a value between MinStressLevel and MaxStressLevel
    /// </summary>
    private void RandomizeStressLevel()
    {
        StressLevel.SetValue(Random.Range(MinStressLevel, MaxStressLevel));
        Debug.Log("Player stress level is now " + StressLevel.Value);
    }

    /// <summary>
    /// Sends out a presence update on a chance basis. Chances are linked to player stress level.
    /// </summary>
    private void ReactToStressLevel()
    {
        float stress = StressLevel.Value;

        if (stress < 0.3f)
        {
            Debug.Log("Player is calm.");
        }
        else if (stress < 0.5f)
        {
            Debug.Log("Player is slightly stressed");
            if (Random.Range(0f, 1f) < 0.2f)
                UpdatePresence();
        }
        else if (stress < 0.8f)
        {
            Debug.Log("Player is stressed");
            if (Random.Range(0f, 1f) < 0.4f)
                UpdatePresence();
        }
        else
        {
            Debug.Log("Player is very stressed");
            if (Random.Range(0f, 1f) < 0.8f)
                UpdatePresence();
        }
    }

    /// <summary>
    /// Raises the PresenceUpdate event.
    /// </summary>
    private void UpdatePresence()
    {
        LastPos.Value = transform.position;
        PresenceUpdate.Raise();
        Debug.Log("Presence update triggered");
    }
}
