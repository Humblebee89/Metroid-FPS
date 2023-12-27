using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScreenEffectController : MonoBehaviour
{
    private Animator dashEffectAnimator;

    private void Awake()
    {
        dashEffectAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Actions.OnDash += Dash;
    }

    private void OnDisable()
    {
        Actions.OnDash -= Dash;
    }

    private void Dash()
    {
        dashEffectAnimator.SetTrigger("Dash");
    }

}
