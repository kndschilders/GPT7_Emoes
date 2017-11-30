using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{

    public FloatVariable StressLevel;
    public Vector3Variable LastPlayerPos;
    public GameEvent PresenceUpdate;
    public float UpdateTickTime = 1f;
    [Range(0.1f,1f)]
    public float PulseChance_SlightlyStressed, PulseChance_Stressed, PulseChance_VeryStressed;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
            return;

        // React to stress level every x seconds
        InvokeRepeating("ReactToStressLevel", 0f, UpdateTickTime);
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
            if (Random.Range(0f, 1f) < PulseChance_SlightlyStressed)
                UpdatePresence();
        }
        else if (stress < 0.8f)
        {
            Debug.Log("Player is stressed");
            if (Random.Range(0f, 1f) < PulseChance_Stressed)
                UpdatePresence();
        }
        else
        {
            Debug.Log("Player is very stressed");
            if (Random.Range(0f, 1f) < PulseChance_VeryStressed)
                UpdatePresence();
        }
    }

    /// <summary>
    /// Raises the PresenceUpdate event.
    /// </summary>
    private void UpdatePresence()
    {
        LastPlayerPos.Value = playerTransform.position;
        PresenceUpdate.Raise();
        Debug.Log("Presence update triggered");
    }
}
