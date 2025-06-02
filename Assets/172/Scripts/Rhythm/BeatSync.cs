/* 
 * Last modified by: Niko Otsuki
 * Last modified on: 5/28/25
 * 
 * BeatSync.cs contains code for syncing music up to a track
 * w/ a provided BPM using the method discussed in this tutorial
 * by b3agz:
 * https://youtu.be/gIjajeyjRfE?feature=shared
 * Expanded to support offbeats + measure gaps
 * 
 * Created by: Tien Le
 * Created on: 4/21/25
 * Contributors: Tien Le, b3agz, Niko 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatSync : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource audio;
    [SerializeField] public int measureLength;
    [SerializeField] private Intervals[] intervals;

    // Update is called once per frame
    private void Update()
    {
        foreach (var interval in intervals)
        {
            //Debug.Log(audio.timeSamples);
            float sampledTime = (audio.timeSamples / (audio.clip.frequency * interval.GetIntervalLength(bpm)));
            float measureTime = (audio.timeSamples / (audio.clip.frequency * interval.GetMeasure(bpm, measureLength)));
            //if you have custom beats enabled, only custom beats will work
            if (interval.customBeats == true)
            {
                interval.CheckForCustomInterval(sampledTime, measureTime, measureLength);
            }
            else
            {
                interval.CheckForNewInterval(sampledTime, measureTime, measureLength);
            }
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
    //[5/28/25 Niko]
    //can be a value between 0 and 3 and determines which beat in a measure is single out ot be played
    //so if you want only the 3rd beat to play, you would enter 2 as the soloBeat
    [SerializeField] private int soloBeat;
    //[5/28/25 Niko]
    //just a check if you want to single out a beat.
    [SerializeField] private bool singleBeat;
    //[5/28/25 Niko]
    //a list of 4 (has to be 4) numbers that are either 0 or 1 that represent which beat will be played during the first measure
    [SerializeField] private List<int> measure1;
    //[5/28/25 Niko]
    //same as above bu for the second measure. [PLEASE follow the rules of 4 elements of 1 or 0 I'm sorry it's like this]
    [SerializeField] private List<int> measure2;
    //[5/28/25 Niko]
    //a check whether or not you want to use custom beats
    [SerializeField] public bool customBeats;
    //[5/28/25 Niko]
    //just to switch between first and second measure
    private bool m1 = true;
    //a simple incrementer
    private int i = 0;
    // [4/21/25 Tien]
    // event that triggers when the specified interval has passed
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;
    private int lastMeasureInt;
    private int beat;

    public float GetIntervalLength(float bpm)
    {
        return (60f / (bpm * noteLength));
    }

    public float GetMeasure(float bpm, int measureLength)
    {
        return (60f / (bpm)) * measureLength;
    }

    //[5/28/25 Niko]
    //Similar to the normal function below, its just that custom beats interferes with solo beats and normal processes so it needs its own function.
    public void CheckForCustomInterval(float interval, float measureInterval, int measureLength)
    {
        //if (Mathf.FloorToInt(measureInterval) != lastMeasureInt)
        //{
        //    lastMeasureInt = Mathf.FloorToInt(measureInterval);
        //}
        if ((Mathf.FloorToInt(interval) != lastInterval))
        {
            lastInterval = Mathf.FloorToInt(interval);
            if (m1)
            {
                beat = measure1[i];
            }
            else
            {
                beat = measure2[i];
            }
            if (beat == 1)
            {
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
            i++;
            if (i > measureLength - 1)
            {
                i = 0;
                if (m1)
                {
                    m1 = false;
                }
                else
                {
                    m1 = true;
                }
            }

        }
    }

    // [4/21/25 Tien]
    // this checks if a new interval has passed based on if
    // the time passed has exceeded the interval 
    // because of its usage in the Beat Sync class it actually measures in time
    // based on packets/samples, not in beats
    // [5/28/25] Niko
    // added soloBeat capability so you can now single out a beat (0-3) to play each meassure
    public void CheckForNewInterval(float interval, float measureInterval, int measureLength)
    {
        if (Mathf.FloorToInt(measureInterval) != lastMeasureInt)
        {
            lastMeasureInt = Mathf.FloorToInt(measureInterval);
        }
        if ((Mathf.FloorToInt(interval) != lastInterval))
        {
            lastInterval = Mathf.FloorToInt(interval);
            if (soloBeat == 0)
            {
                if ((lastMeasureInt % (measureGap + 1) == 1) || measureGap == 0)
                {
                    if (everyOther)
                    {
                        if (lastInterval % 2 == 1)
                        {
                            trigger.Invoke();
                            if (singleBeat)
                            {
                                soloBeat = measureLength - 1;
                            }
                        }
                    }
                    else
                    {
                        trigger.Invoke();
                        if (singleBeat)
                        {
                            soloBeat = measureLength - 1;
                        }
                    }
                }
            }
            else
            {
                soloBeat--;
            }
        }
    }
}


