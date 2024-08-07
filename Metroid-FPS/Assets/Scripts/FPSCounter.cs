using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float refreshRate = 1f;
    [SerializeField] private int vSyncValue;
 
    private float timer;

    void Start()
    {
        QualitySettings.vSyncCount = vSyncValue;
    }
 
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