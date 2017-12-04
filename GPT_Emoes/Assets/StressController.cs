using UnityEngine;

public class StressController : MonoBehaviour {

    public FloatVariable stress;
    public FloatVariable bpmLevel;

    private float stressValue;

	void Update () {
		if(Input.GetKey(KeyCode.KeypadMinus) || Input.GetMouseButton(1))
        {
            stressValue = (stress.Value - .3f * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.KeypadPlus) || Input.GetMouseButton(0))
        {
            stressValue = (stress.Value + .3f * Time.deltaTime);
        }

        stressValue = Mathf.Clamp(stressValue, 0, 1);

        stress.SetValue(stressValue);
        bpmLevel.SetValue(60 + 60 * stressValue);
	}
}
