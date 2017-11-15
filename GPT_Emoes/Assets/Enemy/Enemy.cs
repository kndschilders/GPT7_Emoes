
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{

    public FloatReference PlayerStressLevel;
    public Vector3Reference LastPlayerPos;

    private Mover mover;


    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    /// <summary>
    /// Respond to a player presence update
    /// </summary>
    public void OnPlayerPresenceUpdate()
    {
        // Move to last player pos
        mover.MoveToLocation(LastPlayerPos.Value);
    }
}
