
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(BoxCollider))]
public class Enemy : MonoBehaviour
{
    public enum BehaviorState
    {
        Roaming,
        Investigating,
        Chasing
    }

    #region Public variables

    public FloatReference PlayerStressLevel;
    public Vector3Reference LastPlayerPos;
    public BehaviorState Behavior;
    public float ChaseUpdateTickTime = .25f;

    [Tooltip("The maximum distance the enemy will move from the LastPlayerPos when investigating")]
    public float InvestigationRadius = 5f;

    #region Public variables - Movement
    public float MoveSpeedRoaming = 1f;
    public float MoveSpeedInvestigating = 1.25f;
    public float MoveSpeedChasing = 2f;
    #endregion

    #region Public variables - Alertness
    [Tooltip("The amount by which the alertness level will be increased when the player sends out a presence update.")]
    public float AlertnessIncrement = 1f;
    [Tooltip("The level of alertness this enemy has to reach before switching to investigation mode.")]
    public float InvestigationAlertnessThreshold = 3;
    [Tooltip("The amount by which the alertness level will be reduced per second")]
    public float AlertnessReductionPerSecond = 0.25f;
    public float MaxAlertness = 5f;
    #endregion

    #region Public variables - Distance maintenance
    [Tooltip("When the distance between the player and this enemy exceeds this, this enemy will be teleported closer toward the player")]
    public float MaxDistanceToPlayer = 100f;
    [Tooltip("Time between distance maintenance checks.")]
    public float DistanceCheckTickTime = 2f;
    [Tooltip("The weight of the distance between potential destinations and this enemy when deciding the optimal teleport location (when out of range)")]
    public float DistanceToSelfWeight = 1f;
    [Tooltip("The weight of the distance between potential destinations and the player when deciding the optimal teleport location (when out of range)")]
    public float DistanceToPlayerWeight = 1f;
    #endregion
    #endregion

    #region Private variables
    private GameObject[] RoamDestinations;
    private Mover mover;
    private float alertness = 0f;
    private GameObject playerObject;
    #endregion

    #region Initializaiton
    private void Awake()
    {
        mover = GetComponent<Mover>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        RoamDestinations = GameObject.FindGameObjectsWithTag("Destination");

        if (playerObject != null)
            Debug.Log("Found player: " + playerObject.name);
        else
            Debug.Log("No player found! Please mark something with the Player tag.");
    }

    private void Start()
    {
        // Init behavior
        UpdateBehaviorState(Behavior);

        // Start the distance check cycle
        InvokeRepeating("DistanceCheckTick", DistanceCheckTickTime, DistanceCheckTickTime);

        // Start the alertness reduction cycle
        InvokeRepeating("AlertnessReductionTick", 1, 1);
    }

    #endregion

    #region Player detection
    private void OnTriggerEnter(Collider other)
    {
        // test
        Debug.Log("Something entered " + name + "'s vision");

        // Only check for player
        if (!other.gameObject == playerObject)
        {
            Debug.Log("It's not the player...");
            return;
        }

        // Don't do anything when player not in LOS
        if (!PlayerInLOS())
            return;

        // When in LOS, start chasing.
        UpdateBehaviorState(BehaviorState.Chasing);
    }

    private void OnTriggerExit(Collider other)
    {
        // Only check for player
        if (other != playerObject)
            return;

        // When chasing and player disappears from sight, go into investigation mode
        if (Behavior == BehaviorState.Chasing)
            UpdateBehaviorState(BehaviorState.Investigating);
    }

    private bool PlayerInLOS()
    {
        // Check if player in LOS.
        RaycastHit hit;
        Ray ray = new Ray(transform.position, (playerObject.transform.position - transform.position).normalized);
        Physics.Raycast(ray, out hit);
        return hit.collider != null && (hit.collider.gameObject == playerObject);
    }
    #endregion

    #region Distance maintenance
    /// <summary>
    /// Checks if the path distance to the player exceeds the limit and if so, teleports to a more suitable location.
    /// </summary>
    private void DistanceCheckTick()
    {
        if (playerObject == null)
            return;

        if (mover.GetPathDistanceToTarget(playerObject.transform) > MaxDistanceToPlayer)
            TeleportToSuitableLocation();
    }

    /// <summary>
    /// Teleports to a roam destination closer to the player
    /// </summary>
    private void TeleportToSuitableLocation()
    {
        transform.position = GetOptimalTeleportLocation(DistanceToPlayerWeight, DistanceToSelfWeight);
        UpdateBehaviorState(Behavior);
    }

    /// <summary>
    /// Returns a location that has an optimal distance to player and distance to self.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetOptimalTeleportLocation(float distanceToPlayerWeight, float distanceToSelfWeight)
    {
        // Get distances
        float farthestDistanceToPlayer, closestDistanceToSelf;
        Dictionary<Vector3, float> distancesToPlayer, distancesToSelf;
        GetDistancesPerRoamDestination(out farthestDistanceToPlayer, out closestDistanceToSelf, out distancesToPlayer, out distancesToSelf);

        // Determine score per location (also filter locations that are too far away)
        Dictionary<Vector3, float> scoresPerLocation = new Dictionary<Vector3, float>();
        foreach (KeyValuePair<Vector3, float> entry in distancesToPlayer)
        {
            // Skip scoring locations that are out of bounds
            if (Vector3.Distance(entry.Key, playerObject.transform.position) > MaxDistanceToPlayer)
                continue;

            // Get distances
            float distancePlayer = entry.Value;
            float distanceSelf;
            distancesToSelf.TryGetValue(entry.Key, out distanceSelf);

            // Determine scores
            float distancePlayerScore = distancePlayer / farthestDistanceToPlayer * distanceToPlayerWeight;
            float distanceSelfScore = (closestDistanceToSelf / distanceSelf) * distanceToSelfWeight; // distance to self has 3 times the 'weight' of distance to player
            scoresPerLocation.Add(entry.Key, distancePlayerScore + distanceSelfScore);
        }

        // Return best match
        return GetLocationWithHighestScore(scoresPerLocation);
    }

    /// <summary>
    /// Returns a location that either has the highest score or is within the top x percent highest scoring.
    /// </summary>
    /// <param name="scoresPerLocation"></param>
    /// <param name="randomHighscorer"></param>
    /// <param name="topPercentRange"></param>
    /// <returns></returns>
    private Vector3 GetLocationWithHighestScore(Dictionary<Vector3, float> scoresPerLocation, bool randomHighscorer = false, float topPercentRange = .25f)
    {
        List<Vector3> candidates = new List<Vector3>();

        float highestScore = 0f;
        Vector3 highestScoringLocation = Vector3.zero;
        // Determine highest score
        foreach (KeyValuePair<Vector3, float> entry in scoresPerLocation)
        {
            if (entry.Value > highestScore)
            {
                highestScore = entry.Value;
                highestScoringLocation = entry.Key;
            }
        }

        // Determine candidates
        foreach (KeyValuePair<Vector3, float> entry in scoresPerLocation)
        {
            if (entry.Value > highestScore * (1 - topPercentRange))
                candidates.Add(entry.Key);
        }

        // Return either a random entry in the top x percent or the entry with the highest score.
        return randomHighscorer ? RandomUtil.RandomElement(candidates.ToArray()) : highestScoringLocation;
    }

    /// <summary>
    /// Calculates distances to player and self for every entry in the Roam Destinations list.
    /// </summary>
    /// <param name="farthestDistanceToPlayer"></param>
    /// <param name="closestDistanceToSelf"></param>
    /// <param name="distancesToPlayer"></param>
    /// <param name="distancesToSelf"></param>
    private void GetDistancesPerRoamDestination(out float farthestDistanceToPlayer, out float closestDistanceToSelf, out Dictionary<Vector3, float> distancesToPlayer, out Dictionary<Vector3, float> distancesToSelf)
    {
        distancesToPlayer = new Dictionary<Vector3, float>();
        farthestDistanceToPlayer = 0f;
        distancesToSelf = new Dictionary<Vector3, float>();
        closestDistanceToSelf = 9999999999999999999999999f;

        // Calculate distances per destination
        foreach (GameObject destination in RoamDestinations)
        {
            // Calc distance to player
            float distanceToPlayer = Vector3.Distance(destination.transform.position, playerObject.transform.position);
            distancesToPlayer.Add(destination.transform.position, distanceToPlayer);

            // Update farthest distance to player
            if (distanceToPlayer > farthestDistanceToPlayer)
                farthestDistanceToPlayer = distanceToPlayer;

            // Calc distance to self
            float distanceToSelf = Vector3.Distance(destination.transform.position, transform.position);
            distancesToSelf.Add(destination.transform.position, distanceToSelf);

            // Update closest distance to self
            if (distanceToSelf < closestDistanceToSelf)
                closestDistanceToSelf = distanceToSelf;
        }
    }
    #endregion

    #region Alertness
    /// <summary>
    /// Changes alertness level by amount.
    /// Updates behavior based on current behavior and alertness level.
    /// </summary>
    /// <param name="amount"></param>
    private void ChangeAlertnessLevel(float amount = 1f)
    {
        // Maintain max alertness when chasing
        if (Behavior == BehaviorState.Chasing)
        {
            alertness = MaxAlertness;
            return;
        }

        // Update alertness
        alertness = Mathf.Clamp(alertness + amount, 0, MaxAlertness);
        //Debug.Log(name + " alertness has been changed to " + alertness);

        // Start investigating when threshold reached and not already investigating 
        if (alertness >= InvestigationAlertnessThreshold && Behavior != BehaviorState.Investigating)
            UpdateBehaviorState(BehaviorState.Investigating);
        // Switch back to roaming when threshold not reached
        else if (alertness < InvestigationAlertnessThreshold && Behavior != BehaviorState.Roaming)
            UpdateBehaviorState(BehaviorState.Roaming);
    }

    /// <summary>
    /// Reduces alertness level by AlertnessReductionPerSecond
    /// </summary>
    private void AlertnessReductionTick()
    {
        ChangeAlertnessLevel(-AlertnessReductionPerSecond);
    }
    #endregion

    #region Behavior
    /// <summary>
    /// Move toward the last known player pos.
    /// </summary>
    private void Move(Vector3 destination)
    {
        mover.MoveToLocation(destination);
    }

    /// <summary>
    /// Reduce alertness and move to a random roam destination.
    /// </summary>
    private void Roam()
    {
        Debug.Log(name + " is roaming.");
        Move(RandomUtil.RandomElement(RoamDestinations).transform.position);
    }

    /// <summary>
    /// When called whilst chase behavior is active, start moving towards player location until switched to another behavior.
    /// </summary>
    private void Chase()
    {
        if (Behavior != BehaviorState.Chasing)
            return;

        // Switch to investigation mode when player no longer in LOS
        if (!PlayerInLOS())
        {
            UpdateBehaviorState(BehaviorState.Investigating);
            return;
        }

        Move(playerObject.transform.position);
    }

    /// <summary>
    /// Move to last known player pos or investigate the area around it when near enough.
    /// </summary>
    private void Investigate()
    {
        // When already close to the site of investigation, scan around the site
        // Currently simply moves around the area (TODO: more detailed scanning behavior)
        if (Vector3.Distance(LastPlayerPos, transform.position) < 2f)
        {
            Debug.Log(name + " is investigating site.");
            Vector2 randomV2 = Random.insideUnitCircle;
            Vector3 offset = new Vector3(randomV2.x, transform.position.y, randomV2.y) * Random.Range(2f, InvestigationRadius);
            Move(LastPlayerPos + offset);
            return;
        }

        Debug.Log(name + " is moving to inversigation site.");
        Move(LastPlayerPos);
    }

    /// <summary>
    /// Updates the behavior state.
    /// </summary>
    /// <param name="newState"></param>
    private void UpdateBehaviorState(BehaviorState newState)
    {
        Behavior = newState;
        Debug.Log(name + " behaviour state updated to: " + Behavior);

        switch (Behavior)
        {
            case BehaviorState.Roaming:
                mover.SetMoveSpeed(MoveSpeedRoaming);
                Roam();
                break;
            case BehaviorState.Investigating:
                mover.SetMoveSpeed(MoveSpeedInvestigating);
                Investigate();
                break;
            case BehaviorState.Chasing:
                mover.SetMoveSpeed(MoveSpeedChasing);
                InvokeRepeating("Chase", 0, ChaseUpdateTickTime);
                Debug.Log(name + " is chasing!");
                break;
        }
    }

    /// <summary>
    /// Respond to a player presence update based on current behavior state
    /// </summary>
    public void OnPlayerPresenceUpdate()
    {
        switch (Behavior)
        {
            case BehaviorState.Roaming:
                ChangeAlertnessLevel();
                break;
            case BehaviorState.Investigating:
                Investigate();
                break;
            case BehaviorState.Chasing:

                break;
        }
    }

    /// <summary>
    /// When in roaming mode, roam to a new destination.
    /// When in investigation mode, switch to roaming mode.
    /// Should be assigned to the mover's OnDestinationReached event.
    /// </summary>
    public void OnDestinationReached()
    {
        Debug.Log(name + " reached its destination.");

        switch (Behavior)
        {
            case BehaviorState.Roaming:
                Roam();
                break;
            case BehaviorState.Investigating:
                UpdateBehaviorState(BehaviorState.Roaming);
                break;
            case BehaviorState.Chasing:

                break;
        }
    }
    #endregion
}
