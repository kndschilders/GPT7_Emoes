using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Simulates player behavior to test the AI enemy
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(PlayerHidingScript))]
public class PlayerSimulator : MonoBehaviour
{
    public FloatVariable StressLevel;
    public Vector3Variable LastPos;
    public GameEvent PresenceUpdate;
    public float SimulationTickTime = 1f;
    [Range(0f, 1f)]
    public float MinStressLevel, MaxStressLevel;
    public float HideExitTime = 5f;
    public float MinDistanceToHideSpot = 3f;

    #region Private variables
    private GameObject[] movePoints;
    private NavMeshAgent agent;
    private PlayerHidingScript playerHidingScript;
    private bool isFindingHideSpot = false;
    private bool isInsideHidingSpot = false;
    private List<HideSpotScript> hideSpots;
    private HideSpotScript currentHidingSpot;
    private bool isBeingChased = false;
    private List<Enemy> enemiesInLevel;
    #endregion

    #region Initialization
    private void Awake()
    {
        // Get components
        agent = GetComponent<NavMeshAgent>();
        playerHidingScript = GetComponent<PlayerHidingScript>();

        // Get non-components
        movePoints = GameObject.FindGameObjectsWithTag("Destination");
        enemiesInLevel = new List<Enemy>(FindObjectsOfType<Enemy>());
        hideSpots = new List<HideSpotScript>(FindObjectsOfType<HideSpotScript>());
    }

    private void Start()
    {
        // Start the simulation tick
        InvokeRepeating("SimulationTick", 0f, SimulationTickTime);
    }
    #endregion

    /// <summary>
    /// Simulates player behavior
    /// </summary>
    private void SimulationTick()
    {
        // Check if being chased
        CheckIsBeingChased();

        // Find hiding spot
        if (isBeingChased)
            FindHideSpot();

        // Move randomly (or not) if not being chased or already moving
        else if (Random.Range(0f, 1f) < 0.5f)
        {
            if (agent.velocity != Vector3.zero)
                MoveRandomly();
        }

        // Randomize stress level
        RandomizeStressLevel();

        // React to stress level
        ReactToStressLevel();
    }

    #region Chase behavior
    /// <summary>
    /// Checks if any enemies are currently chasing the player and if so, starts the chasing process
    /// </summary>
    private void CheckIsBeingChased()
    {
        foreach (Enemy enemy in enemiesInLevel)
        {
            if (enemy.Behavior == Enemy.BehaviorState.Chasing)
            {
                StartChaseProcess();
                return;
            }
        }

        // Stop chase process when not being chased.
        StopChaseProcess();
    }

    /// <summary>
    /// When not already in chasing process, finds a hiding spot
    /// </summary>
    public void StartChaseProcess()
    {
        if (isBeingChased)
            return;

        Debug.Log("Player is being chased!");
        isBeingChased = true;
        FindHideSpot();
    }

    /// <summary>
    /// Stops the chasing process
    /// </summary>
    public void StopChaseProcess()
    {
        if (!isBeingChased)
            return;

        Debug.Log("Player is no longer being chased.");
        isBeingChased = false;
    }
    #endregion

    #region Moving
    /// <summary>
    /// Moves to the destination.
    /// </summary>
    /// <param name="destination"></param>
    private void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    /// <summary>
    /// Updates LastPos to a random entry from MovePoints
    /// </summary>
    private void MoveRandomly()
    {
        if (movePoints.Length == 0)
        {
            Debug.LogWarning("No move points assigned!");
            return;
        }
        agent.SetDestination(RandomUtil.RandomElement(movePoints).transform.position);
        Debug.Log("Player moving to " + agent.destination);
    }
    #endregion

    #region Hiding
    /// <summary>
    /// Finds the nearest hiding spot, moves toward it and enters it when close enough.
    /// </summary>
    private void FindHideSpot()
    {
        if (isFindingHideSpot || hideSpots.Count == 0 || isInsideHidingSpot)
            return;

        isFindingHideSpot = true;
        Debug.Log(name + " is finding a hiding spot!");

        // Find nearest hiding spot
        float nearestDistance = 99999999999999999999999999999999f;
        currentHidingSpot = null;
        foreach (HideSpotScript hideSpot in hideSpots)
        {
            float distance = Vector3.Distance(hideSpot.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                currentHidingSpot = hideSpot;
            }
        }

        Move(currentHidingSpot.transform.position);
        StartCoroutine(Hide(currentHidingSpot));
    }

    /// <summary>
    /// Exits the current hiding spot
    /// </summary>
    private void ExitHideSpot()
    {
        if (currentHidingSpot == null)
            return;

        // Exit hiding spot
        Debug.Log("Player is exiting hiding spot.");
        isFindingHideSpot = false;
        playerHidingScript.ExitHidingSpot();
        isInsideHidingSpot = false;

        // Activate movement
        GetComponent<Collider>().enabled = true;
        agent.isStopped = false;
    }

    /// <summary>
    /// Enters the hiding spot
    /// </summary>
    /// <param name="hideSpot"></param>
    private void EnterHideSpot(HideSpotScript hideSpot)
    {
        Debug.Log("Player is entering hiding spot!");

        // Disable movement
        agent.isStopped = true;

        // Hide
        GetComponent<Collider>().enabled = false;
        playerHidingScript.EnterHidingSpot(hideSpot.PlayerLocationTransform);
        isFindingHideSpot = false;
        isInsideHidingSpot = true;
    }

    /// <summary>
    /// Waits until close enough to the hiding spot, then enters it and starts waiting until no enemies are chasing anymore.
    /// </summary>
    /// <param name="hideSpot"></param>
    /// <returns></returns>
    private IEnumerator Hide(HideSpotScript hideSpot)
    {
        // Wait 'til close enough to hiding spot
        while (Vector3.Distance(transform.position, hideSpot.transform.position) > MinDistanceToHideSpot)
        {
            yield return null;
        }

        // Enter hiding spot
        EnterHideSpot(hideSpot);

        // Wait inside hiding spot until no enemies are chasing the player anymore.
        StartCoroutine(WaitOutTheChase());
    }

    /// <summary>
    /// Waits until all enemies are no longer chasing, then exits the hiding spot.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitOutTheChase()
    {
        // Wait until no longer being chased
        while (isBeingChased)
            yield return new WaitForSeconds(1);

        // Wait some time (to simulate 'making sure')
        yield return new WaitForSeconds(HideExitTime);

        // Exit hiding spot
        ExitHideSpot();
    }
    #endregion

    #region Stress behavior
    /// <summary>
    /// Randomize stress level, picking a value between MinStressLevel and MaxStressLevel
    /// </summary>
    private void RandomizeStressLevel()
    {
        StressLevel.SetValue(Random.Range(MinStressLevel, MaxStressLevel));
        //Debug.Log("Player stress level is now " + StressLevel.Value);
    }

    /// <summary>
    /// Sends out a presence update on a chance basis. Chances are linked to player stress level.
    /// </summary>
    private void ReactToStressLevel()
    {
        float stress = StressLevel.Value;

        if (stress < 0.3f)
        {
            //Debug.Log("Player is calm.");
        }
        else if (stress < 0.5f)
        {
            //Debug.Log("Player is slightly stressed");
            if (Random.Range(0f, 1f) < 0.2f)
                UpdatePresence();
        }
        else if (stress < 0.8f)
        {
            //Debug.Log("Player is stressed");
            if (Random.Range(0f, 1f) < 0.4f)
                UpdatePresence();
        }
        else
        {
            //Debug.Log("Player is very stressed");
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
        //Debug.Log("Presence update triggered");
    }
    #endregion
}
