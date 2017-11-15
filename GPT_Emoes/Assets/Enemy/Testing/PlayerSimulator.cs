using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates player behavior to test the AI enemy
/// </summary>
public class PlayerSimulator : MonoBehaviour
{

    public FloatVariable StressLevel;
    public Vector3Variable LastPos;
    public GameEvent PresenceUpdate;
    public Transform[] MovePoints;
    public float SimulationTickTime = 1f;
    [Range(0f, 1f)]
    public float MinStressLevel, MaxStressLevel;

    private void Start()
    {
        // Start the simulation tick
        InvokeRepeating("SimulationTick", SimulationTickTime, SimulationTickTime);
    }

    private void SimulationTick()
    {
        // Move
        if (Random.Range(0f, 1f) < 0.5f)
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
        if (MovePoints.Length == 0)
        {
            Debug.LogWarning("No move points assigned!");
            return;
        }
        LastPos.SetValue(RandomUtil.RandomElement(MovePoints).position);
        Debug.Log("Player moved to " + LastPos.Value);
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
        PresenceUpdate.Raise();
        Debug.Log("Presence update triggered");
    }
}
