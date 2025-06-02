/*
* Created by: Dale Spence
* Created on: 5 / 25 / 25
* Last Edit: 5 / 27 / 25
* Contributors: Dale Spence
* 
* This script manages the game's framerate cap using an enum with different speed options.
* The SetFPSLimit method updates Unity's Application.targetFrameRate, and
* OnFPSLimitChanged lets the FPS setting be changed
* 
* 
*/


using UnityEngine;

public class FPSController : MonoBehaviour
{
    // Enum to represent all supported FPS limits
    public enum FPSLimit
    {
        Sixty,
        OneTwenty,
        TwoForty,
        Off  // No limit
    }

    [Header("Current FPS Setting")]
    public FPSLimit currentFPSLimit = FPSLimit.TwoForty;

    void Start()
    {
        SetFPSLimit(currentFPSLimit);
    }

    // Sets the FPS based on the provided enum
    public void SetFPSLimit(FPSLimit limit)
    {
        currentFPSLimit = limit;

        switch (limit)
        {
            case FPSLimit.Sixty:
                Application.targetFrameRate = 60;
                break;
            case FPSLimit.OneTwenty:
                Application.targetFrameRate = 120;
                break;
            case FPSLimit.TwoForty:
                Application.targetFrameRate = 240;
                break;
            case FPSLimit.Off:
                Application.targetFrameRate = -1; // No limit
                break;
        }

        Debug.Log("FPS Limit set to: " + limit);
    }

    // Maps integer input (from menu selection) to enum values
    public void OnFPSLimitChanged(int fpsOption)
    {
        // Make sure the input index is valid
        if (fpsOption >= 0 && fpsOption < System.Enum.GetValues(typeof(FPSLimit)).Length)
        {
            SetFPSLimit((FPSLimit)fpsOption);
        }
        else
        {
            Debug.LogWarning("Invalid FPS option index: " + fpsOption);
        }
    }
}