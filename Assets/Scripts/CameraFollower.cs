using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Vector3 positionOffset;

    public GameObject target;
    public NBodySimulation sim;

    void FixedUpdate()
    {
        if(sim.simulationRunning)
            gameObject.transform.position = target.transform.position + positionOffset;       
    }
}
