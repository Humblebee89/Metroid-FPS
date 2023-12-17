using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissileCounterUIController : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private int startingMissileCount = 20;
    [SerializeField] TMP_Text missileCounterText;
    [SerializeField] RectTransform missileIcon;
    [SerializeField] private Vector2 startAndEndYPosition;
    
    private SpriteMask spriteMask;
    private int currentMissileCount;
    private float missilePercent;

    private void OnEnable()
    {
        Actions.OnFireMissile += UpdateCounter;
    }

    private void OnDisable()
    {
        Actions.OnFireMissile -= UpdateCounter;
    }

    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        currentMissileCount = startingMissileCount;
        MoveMissileIcon();
    }

    private void UpdateCounter()
    {
        currentMissileCount--;
        missilePercent = 1 - (float)currentMissileCount / startingMissileCount;
        spriteMask.alphaCutoff = missilePercent;
        missileCounterText.text = currentMissileCount.ToString();
        MoveMissileIcon();
    }

    private void MoveMissileIcon()
    {
        var newYPosition =  Mathf.Lerp(startAndEndYPosition.x, startAndEndYPosition.y, missilePercent);
        missileIcon.anchoredPosition = new Vector2(missileIcon.anchoredPosition.x, newYPosition);
    }
    
}
