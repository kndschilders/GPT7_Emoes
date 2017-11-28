using UnityEngine;
using UnityEngine.UI;

public class HeartRateVisuals : MonoBehaviour {

    public int HeartRate = 0;

    public Animator AC;
    public Text text;

    private void Update()
    {
        text.text = HeartRate.ToString();
        AC.SetFloat("multiplier", (HeartRate / 110f));
    }
}
