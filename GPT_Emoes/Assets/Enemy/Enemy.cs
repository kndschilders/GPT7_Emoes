
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
    public float ChaseUpdateTickTime = .25f;

    private Mover mover;
    private float alertness = 0f;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    private void Start()
    {
        UpdateBehaviorState(Behavior);
    }

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
                InvokeRepeating("Chase", 0, 0.25f);
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
        Move(LastPlayerPos);
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

    private void Chase()
    {
        Debug.Log(name + " is chasing!");
        Move(LastPlayerPos);
    }

    /// <summary>
    /// TODO: determine what to do based on behavior state
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
                Chase();
                break;
        }
    }

    /// <summary>
    /// Move toward the last known player pos.
    /// Also start checking if the destination has been reached.
    /// </summary>
    private void Move(Vector3 destination)
    {
        mover.MoveToLocation(destination);
    }
}
