
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    public enum BehaviorState
    {
        Roaming,
        Investigating,
        Chasing
    }

    public FloatReference PlayerStressLevel;
    public Vector3Reference LastPlayerPos;
    public BehaviorState Behavior;
    public int InvestigationAlertnessThreshold = 3;
    public Transform[] RoamDestinations;
    public float AlertnessReductionPerRoam = 0.25f;
    public float MaxDistanceToPlayer = 100f;
    public float DistanceCheckTickTime = 2f;
    public float DistanceToSelfWeight = 1f;
    public float DistanceToPlayerWeight = 1f;

    private Mover mover;
    private float alertness = 0f;
    private GameObject playerObject;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        // Init behavior
        UpdateBehaviorState(Behavior);

        // Start the distance check cycle
        InvokeRepeating("MaintainDistanceToPlayer", DistanceCheckTickTime, DistanceCheckTickTime);
    }

    #region Distance maintenance
    /// <summary>
    /// Checks if the path distance to the player exceeds the limit and if so, teleports to a more suitable location.
    /// </summary>
    private void MaintainDistanceToPlayer()
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
        foreach (Transform destination in RoamDestinations)
        {
            // Calc distance to player
            float distanceToPlayer = Vector3.Distance(destination.position, playerObject.transform.position);
            distancesToPlayer.Add(destination.position, distanceToPlayer);

            // Update farthest distance to player
            if (distanceToPlayer > farthestDistanceToPlayer)
                farthestDistanceToPlayer = distanceToPlayer;

            // Calc distance to self
            float distanceToSelf = Vector3.Distance(destination.position, transform.position);
            distancesToSelf.Add(destination.position, distanceToSelf);

            // Update closest distance to self
            if (distanceToSelf < closestDistanceToSelf)
                closestDistanceToSelf = distanceToSelf;
        }
    }
    #endregion

    /// <summary>
    /// Respond to a player presence update based on current behavior state
    /// </summary>
    public void OnPlayerPresenceUpdate()
    {
        switch (Behavior)
        {
            case BehaviorState.Roaming:
                RaiseAlertnessLevel();
                break;
            case BehaviorState.Investigating:
                Investigate();
                break;
            case BehaviorState.Chasing:
                // TODO: actual chasing behavior
                break;
        }
    }

    /// <summary>
    /// Increases alertness level. When threshold reached, switches behavior to Investigating.
    /// </summary>
    private void RaiseAlertnessLevel(float amount = 1f)
    {
        alertness += amount;
        Debug.Log(name + " alertness has been raised to " + alertness);

        if (alertness >= InvestigationAlertnessThreshold)
            UpdateBehaviorState(BehaviorState.Investigating);
    }

    /// <summary>
    /// Lowers alertness level to a minimum of 0.
    /// When threshold passed, switches behavior to roaming.
    /// </summary>
    private void LowerAlertnessLevel(float amount = 1f)
    {
        if (alertness > 0)
            alertness -= amount;
        else
            return;

        Debug.Log(name + " alertness has been lowered to " + alertness);

        if (alertness < InvestigationAlertnessThreshold && Behavior != BehaviorState.Roaming)
            UpdateBehaviorState(BehaviorState.Roaming);
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
                Roam();
                break;
            case BehaviorState.Investigating:
                Investigate();
                break;
            case BehaviorState.Chasing:
                // TODO: Actual chasing behavior
                break;
        }
    }

    /// <summary>
    /// Move to last known player pos.
    /// TODO: When already close to that pos, check the area.
    /// </summary>
    private void Investigate()
    {
        Debug.Log(name + " is investigating.");
        Move(LastPlayerPos.Value);
    }

    /// <summary>
    /// Reduce alertness and move to a random roam destination.
    /// </summary>
    private void Roam()
    {
        Debug.Log(name + " is roaming.");
        Move(RandomUtil.RandomElement(RoamDestinations).position);
        LowerAlertnessLevel(AlertnessReductionPerRoam);
    }

    /// <summary>
    /// When in roaming mode, roam to a new destination.
    /// When in investigation mode, switch to roaming mode.
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
                // TODO: Actual chasing behavior
                break;
        }
    }

    /// <summary>
    /// Move toward the last known player pos.
    /// </summary>
    private void Move(Vector3 destination)
    {
        mover.MoveToLocation(destination);
    }
}
