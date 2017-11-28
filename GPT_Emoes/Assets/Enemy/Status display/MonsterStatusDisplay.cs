using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatusDisplay : MonoBehaviour
{

    public float UpdateTickRate = 1f;
    public Enemy Monster;
    public Image RoamingStatusImage;
    public Image InvestigatingStatusImage;
    public Image ChasingStatusImage;
    public Text AlertnessText;
    public Text MonsterNameText;


    private void Awake()
    {
        if (Monster == null)
            return;

        InvokeRepeating("UpdateDisplay", 0, UpdateTickRate);
        if (MonsterNameText != null)
            MonsterNameText.text = Monster.name;
    }

    private void UpdateDisplay()
    {
        SetStatusImage(Monster.Behavior);
        SetAlertness(Monster.GetAlertnessPercentage());
    }

    private void SetAlertness(float alertnessPercentage)
    {
        if (AlertnessText == null)
            return;

        AlertnessText.text = alertnessPercentage + "% Alertness";
    }

    private void SetStatusImage(Enemy.BehaviorState behaviorState)
    {
        if (ChasingStatusImage == null || InvestigatingStatusImage == null || RoamingStatusImage == null)
            return;

        switch (behaviorState)
        {
            case Enemy.BehaviorState.Roaming:
                RoamingStatusImage.enabled = true;
                InvestigatingStatusImage.enabled = false;
                ChasingStatusImage.enabled = false;
                break;
            case Enemy.BehaviorState.Investigating:
                RoamingStatusImage.enabled = false;
                InvestigatingStatusImage.enabled = true;
                ChasingStatusImage.enabled = false;
                break;
            case Enemy.BehaviorState.Chasing:
                RoamingStatusImage.enabled = false;
                InvestigatingStatusImage.enabled = false;
                ChasingStatusImage.enabled = true;
                break;
        }
    }
}
