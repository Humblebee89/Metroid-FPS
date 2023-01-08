using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPasser : MonoBehaviour
{
    //This class is used to send animation events to the PlayerWeaponController
    [SerializeField] private PlayerWeaponController playerWeaponController;

    private void CanFire(int value)
    {
        playerWeaponController.CanFire(value);
    }
}
