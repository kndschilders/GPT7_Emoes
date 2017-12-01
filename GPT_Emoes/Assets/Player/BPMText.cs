using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPMText : MonoBehaviour {

    public FloatVariable BPMLevel;

    public Animator AC;
    public Text BPMUIText;
	
	void Update () {
        BPMUIText.text = ((int)BPMLevel.Value).ToString();
        AC.SetFloat("multiplier", BPMLevel.Value/120f);
	}
}
