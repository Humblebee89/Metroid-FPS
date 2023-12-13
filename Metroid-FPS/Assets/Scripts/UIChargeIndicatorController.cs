using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChargeIndicatorController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController playerWeaponController;

    private SpriteMask spriteMask;

    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
    }

    private void Update()
    {
        spriteMask.alphaCutoff = 1 - playerWeaponController.chargeValue;
    }
}
