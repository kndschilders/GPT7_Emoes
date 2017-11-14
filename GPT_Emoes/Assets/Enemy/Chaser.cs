using RoboRyanTron.Unite2017.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{

    public Vector3Reference LastPlayerPos;

    public void MoveToLastPlayerPos()
    {
        Debug.Log("Chaser is moving to last player pos : " + LastPlayerPos.Value);
    }
}
