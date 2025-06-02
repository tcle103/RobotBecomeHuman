/*
* Created by: Dale Spence
* Created on: 5 / 25 / 25
* Last Edit: 5 / 27 / 25
* Contributors: Dale Spence
* 
* 
* Calculates and displays FPS through a text object. Updates text every half second.
* The FPS display is only visible when showFPS is true and game is not paused.
* 
*/
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private int fps;

    [Header("References")]
    public FPSController fpsController;
    public TextMeshProUGUI fpsText;

    [Header("Display Settings")]
    public bool showFPS = true;
    public float updateInterval = 0.5f; // Change this to 1.0f for once-per-second updates

    private float updateTimer = 0.0f;

    void Update()
    {
        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(showFPS && Time.timeScale > 0f);
        }

        if (!showFPS || Time.timeScale == 0f || fpsText == null) return;

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        updateTimer += Time.unscaledDeltaTime;

        if (updateTimer >= updateInterval)
        {
            fps = (int)(1.0f / deltaTime);
            fpsText.text = $"FPS: {fps}";
            updateTimer = 0f;
        }
    }

}