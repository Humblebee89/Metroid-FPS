using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PitchIndicatorUIController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private RectTransform tickParent;
    [SerializeField] private float maxAngle = 90f;
    [SerializeField] private float maxScroll = 500f;

    private float rotationCorrected;

    private void Update()
    {
        if (cameraTransform.localEulerAngles.x > 90f)
            rotationCorrected = cameraTransform.localEulerAngles.x - 360f;
        else
            rotationCorrected = cameraTransform.localEulerAngles.x;

       float rotationPercent =  math.remap(maxAngle, -maxAngle, 0, 1, rotationCorrected);
       tickParent.anchoredPosition = new Vector2(tickParent.anchoredPosition.x, Mathf.Lerp(maxScroll, -maxScroll, rotationPercent));
    }
}
