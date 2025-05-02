using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class NPCMovementComponent : MonoBehaviour
{
    [SerializeField] private int StartPoint;
    [SerializeField] private Spline moveSpline; 
    private int totalPoints;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = moveSpline.GetPosition(StartPoint);
        totalPoints = moveSpline.GetPointCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.5f, 4));
        }
    }
}
