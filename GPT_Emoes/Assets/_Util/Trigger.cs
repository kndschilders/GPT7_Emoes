using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{

    public string[] TagsToCheck;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private BoxCollider trigger;
    private List<string> tags;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
        UpdateTrigger();
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateTrigger();
    }
#endif

    private void UpdateTrigger()
    {
        trigger.isTrigger = true;
        // Only update internal tags when external tags are updated.
        if (TagsToCheck.Length != 0 && TagsToCheck.Length != tags.Intersect(TagsToCheck).ToArray().Count())
        {
            tags = new List<string>(TagsToCheck);
            Debug.Log(name + " trigger tags updated!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tags.Contains(other.tag))
            OnEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (tags.Contains(other.tag))
            OnExit.Invoke();
    }
}
