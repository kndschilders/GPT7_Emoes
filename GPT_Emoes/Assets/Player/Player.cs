
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameEvent OnPresenceUpdate;
    public Vector3Variable LastKnownPos;

    private void Start()
    {
        InvokeRepeating("Test", 1f, 1f);
    }

    private void Test()
    {
        transform.Translate(Vector3.one);
        PresenceUpdate();
    }

    /// <summary>
    /// Update last known position and raise the presence update event.
    /// </summary>
    private void PresenceUpdate()
    {
        UpdateLastKnownPos();
        OnPresenceUpdate.Raise();
    }

    private void UpdateLastKnownPos()
    {
        LastKnownPos.SetValue(transform.position);
    }
}
