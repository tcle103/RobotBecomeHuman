/* 
 * Last modified by: Niko Otsuki
 * Last modified on: 6/9/25
 * 
 * Earthquake.cs contains code for causing the screen to shake randomly 
 * as well as smoothly using animation curves, code provided from this video
 * by Thomas Friday:
 * https://www.youtube.com/watch?v=BQGTdRhGmE4
 * 
 * Created by: Niko Otsuki
 * Created on: 6/9/25
 * Contributors: Thomas Friday, Niko 
 */

using System.Collections;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    public bool start = false;
    public float duration = 1.5f;
    public AnimationCurve curve;

    public void Begin()
    {
        start = true;
    }
    void Update () {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
