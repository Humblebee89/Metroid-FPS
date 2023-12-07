using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSelectorUIController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private Animator powerBeamAnimator;
    [SerializeField] private Animator waveBeamAnimator;
    [SerializeField] private Animator iceBeamAnimator;
    [SerializeField] private Animator plasmaBeamAnimator;

    private PlayerWeaponController.ActiveBeam previousBeam;

    private void OnEnable()
    {
        Actions.OnBeamChange += BeamChange;
        previousBeam = playerWeaponController.activeBeam;
        BeamChange();
    }

    private void OnDisable()
    {
        Actions.OnBeamChange -= BeamChange;
    }

    private void BeamChange()
    {
        StartCoroutine("BeamAnimator");
    }

    private IEnumerator BeamAnimator()
    {
        switch (previousBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                powerBeamAnimator.SetBool("BeamSelected", false);
                previousBeam = playerWeaponController.activeBeam;
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                waveBeamAnimator.SetBool("BeamSelected", false);
                previousBeam = playerWeaponController.activeBeam;
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                iceBeamAnimator.SetBool("BeamSelected", false);
                previousBeam = playerWeaponController.activeBeam;
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                plasmaBeamAnimator.SetBool("BeamSelected", false);
                previousBeam = playerWeaponController.activeBeam;
                break;
        }

        yield return new WaitForSeconds(0.2f);

        switch (playerWeaponController.activeBeam)
        {
            case PlayerWeaponController.ActiveBeam.Power:
                powerBeamAnimator.SetBool("BeamSelected", true);
                break;
            case PlayerWeaponController.ActiveBeam.Wave:
                waveBeamAnimator.SetBool("BeamSelected", true);
                break;
            case PlayerWeaponController.ActiveBeam.Ice:
                iceBeamAnimator.SetBool("BeamSelected", true);
                break;
            case PlayerWeaponController.ActiveBeam.Plasma:
                plasmaBeamAnimator.SetBool("BeamSelected", true);
                break;
        }
    }
}
