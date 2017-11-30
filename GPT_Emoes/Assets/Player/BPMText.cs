using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPMText : MonoBehaviour {

    public FloatVariable BPMLevel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = ((int)BPMLevel.Value).ToString();
	}
}
