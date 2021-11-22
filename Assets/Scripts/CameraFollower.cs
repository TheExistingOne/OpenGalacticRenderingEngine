using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Vector3 positionOffset;

    public GameObject target;

    void FixedUpdate()
    {
        gameObject.transform.position = target.transform.position + positionOffset;       
    }
}
