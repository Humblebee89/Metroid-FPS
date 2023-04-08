using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPasser : MonoBehaviour
{
    //This class is used to send animation events to the PlayerWeaponController
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private ArmCannonEffectsController armCannonEffectsController;

    private void CanFire(int value)
    {
        playerWeaponController.CanFire(value);
    }

    private void BeamChange()
    {
        armCannonEffectsController.BeamChange();
    }
}
