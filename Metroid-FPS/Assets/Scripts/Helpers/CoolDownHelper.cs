using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class CoolDownHelper 
{
    //public static bool timerFinished;

    public static IEnumerator CoolDown(float coolDownTimer, System.Action<bool> timerFinished)
    {   
        timerFinished(false);
        yield return new WaitForSeconds(coolDownTimer);
        timerFinished(true);
        //yield break;
    }
}
