using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private float timer = 0.0f;
    public float bobbingSpeed = 0.15f;
    public float bobbingAmount = 0.13f;
    public float midpoint;

    public FloatVariable stressLevel;

	public PlayerStateReference PlayerStateRef;

	// Use this for initialization
	void Start () {
        midpoint = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerStateRef.Value == PlayerState.Stand)
        {
            bobbingSpeed = Mathf.Clamp(stressLevel.Value * 0.25f, 0.15f, 0.25f);
        }
        else
        {
            bobbingSpeed = 0.15f;
        }

        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if(timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            transform.localPosition = new Vector3(transform.localPosition.x, midpoint + translateChange, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, midpoint, transform.localPosition.z);
        }
	}
}
