/* 
 * Last modified by: Tien Le
 * Last modified on: 4/21/25
 * 
 * BeatSync.cs contains code for syncing music up to a track
 * w/ a provided BPM using the method discussed in this tutorial
 * by b3agz:
 * https://youtu.be/gIjajeyjRfE?feature=shared
 * Expanded to support offbeats + measure gaps
 * 
 * Created by: Tien Le
 * Created on: 4/21/25
 * Contributors: Tien Le, b3agz
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatSync : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audio;
    [SerializeField] private int measureLength;
    [SerializeField] private Intervals[] intervals;

    // Update is called once per frame
    private void Update()
    {
        foreach (var interval in intervals) 
        {
            //Debug.Log(audio.timeSamples);
            float sampledTime = (audio.timeSamples / (audio.clip.frequency * interval.GetIntervalLength(bpm)));
            float measureTime = (audio.timeSamples / (audio.clip.frequency * interval.GetMeasure(bpm, measureLength)));
            interval.CheckForNewInterval(sampledTime, measureTime);
        }
    }
}

// [4/21/25 Tien]
// This class generates an interval of time
// that a note in a specified BPM takes up
// Also checks if that interval has passed (is a whole number)
// meaning that a beat has passed

// Basically divides a minute (in seconds)
// by the BPM, then divides it by a specific value
// to get the amount of time a note of that length takes up
[System.Serializable]
public class Intervals
{
    // [4/21/25 Tien]
    // noteLength literally refers to how long a note is
    // in music we usually call these quarter, half, etc.
    // for this value, the lower the value, the longer it is.
    // e.g. a noteLength value of 0.25 is equivalent to a whole note
    // (recurs once every 4 beats) in 4/4 time
    // the inverse is thus true, so a noteLength value of 2 should be equivalent
    // to an 8th note (recurs twice per beat) in 4/4 time
    [SerializeField] private float noteLength;
    // [4/21/25 Tien]
    // everyOther makes it so that the event triggers every other interval
    // effectively makes the note length half as long
    // ex. a notelength of 1 with everyOther is quarter notes on the twos and fours
    // only, or half notes offset by 1 interval (in this case, 1 beat)
    // this would be how you do offbeats
    [SerializeField] private bool everyOther;
    // [4/21/25 Tien]
    // measureGap represents a gap of measures between each beat pattern
    // ex. if you wanted quarter notes on the offbeats every other measure
    // noteLength 1, everyOther true, measureGap 1
    [SerializeField] private int measureGap;
    // [4/21/25 Tien]
    // event that triggers when the specified interval has passed
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;
    private int lastMeasureInt;

    public float GetIntervalLength(float bpm)
    {
        return (60f / (bpm * noteLength));
    }

    public float GetMeasure(float bpm, int measureLength)
    {
        return (60f / (bpm)) * measureLength;
    }

    // [4/21/25 Tien]
    // this checks if a new interval has passed based on if
    // the time passed has exceeded the interval 
    // because of its usage in the Beat Sync class it actually measures in time
    // based on packets/samples, not in beats
    public void CheckForNewInterval(float interval, float measureInterval)
    {
        
        if (Mathf.FloorToInt(measureInterval) != lastMeasureInt)
        {
            lastMeasureInt = Mathf.FloorToInt(measureInterval);
        }
        if ((Mathf.FloorToInt(interval) != lastInterval))
        {
            lastInterval = Mathf.FloorToInt(interval);
            if ((lastMeasureInt % (measureGap + 1) == 1) || measureGap == 0)
            {
                if (everyOther)
                {
                    if (lastInterval % 2 == 1)
                    {
                        trigger.Invoke();
                    }
                }
                else
                {
                    trigger.Invoke();
                }
            }
        }
    }
}


