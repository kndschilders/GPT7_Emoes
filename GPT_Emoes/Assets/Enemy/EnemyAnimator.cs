using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{

    public string IsMovingParam;
    public string IsChasingParam;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIsMoving(bool val)
    {
        animator.SetBool(IsMovingParam, val);
    }

    public void SetIsChasing(bool val)
    {
        animator.SetBool(IsChasingParam, val);
    }

}
