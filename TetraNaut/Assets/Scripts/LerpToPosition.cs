using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    public Transform origin;
    public Transform destination;

    public float totalTime;
    private float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        transform.position = Vector3.Slerp(origin.position, destination.position, time / totalTime);

        if(time > totalTime)
        {
            Destroy(this);
        }
    }
}
