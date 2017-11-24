using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAfterTime : MonoBehaviour
{

    public float Time = 1f;
    public UnityEvent ToDo;

    private void OnEnable()
    {
        Invoke("Do", Time);
    }

    // Do something
    private void Do()
    {
        ToDo.Invoke();
    }
}
