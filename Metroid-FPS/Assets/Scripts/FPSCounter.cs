using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float refreshRate = 1f;
 
    private float timer;
 
    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
            timer = Time.unscaledTime + refreshRate;
        }
    }
}