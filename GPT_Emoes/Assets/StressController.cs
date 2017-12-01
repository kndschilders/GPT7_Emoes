using UnityEngine;

public class StressController : MonoBehaviour {

    public FloatVariable stress;
    public FloatVariable bpmLevel;

    private float stressValue;

	void Update () {
		if(Input.GetKey(KeyCode.KeypadMinus))
        {
            stressValue = (stress.Value - .3f * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.KeypadPlus))
        {
            stressValue = (stress.Value + .3f * Time.deltaTime);
        }

        stressValue = Mathf.Clamp(stressValue, 0, 1);

        stress.SetValue(stressValue);
        bpmLevel.SetValue(60 + 60 * stressValue);
	}
}
