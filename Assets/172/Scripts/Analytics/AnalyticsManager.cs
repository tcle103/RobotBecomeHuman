using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }
    
    public void SendPuzzleSolvedEvent(float timeTaken, int fails)
    {
        if (!_isInitialized) return;

        CustomEvent puzzleEvent = new CustomEvent("puzzle_solved")
        {
            { "time_taken", timeTaken },
            { "fails", fails }
        };
        AnalyticsService.Instance.RecordEvent(puzzleEvent);
        AnalyticsService.Instance.Flush();
    }

    public void Restart()
    {
        AnalyticsService.Instance.RecordEvent("restart_game");
        AnalyticsService.Instance.Flush();
    }
    
}
