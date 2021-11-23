using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitPreview : MonoBehaviour 
{   
    public int steps = 10;
    public bool usePhysicsTime = false;

    private float timeStep;

    void Update () {
        if(!Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    void OnValidate()
    {
        if (usePhysicsTime) timeStep = Universe.physicsTimeStep;
        else timeStep = Universe.previewTimeStep;
    }
    
    void DrawOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        VirtualBody[] virtualBodies = new VirtualBody[bodies.Length];

        Vector3[,] points = new Vector3[virtualBodies.Length, steps];

        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
        }

        for (int i = 0; i < steps; i++)
        {
            // Have every VirtualBody loop over every other VirtualBody
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                points[j, i] = virtualBodies[j].position;
                foreach (VirtualBody otherBody in virtualBodies)
                {
                    if (virtualBodies[j] != otherBody)
                    {
                        var sqrDistance = (otherBody.position - virtualBodies[j].position).sqrMagnitude;
                        var forceDir = (otherBody.position - virtualBodies[j].position).normalized;

                        var magnitude = Universe.G * otherBody.mass / sqrDistance;
                        var velocityChange = forceDir * magnitude * timeStep;
                        virtualBodies[j].velocity += velocityChange;
                    }
                }

                virtualBodies[j].position += virtualBodies[j].velocity * timeStep;
            }
        }

        for (int i = 0; i < virtualBodies.Length; i++)
        {
            for (int j = 1; j < steps; j++)
            {
                LineRenderer lr = bodies[i].gameObject.GetComponent<LineRenderer>();
                lr.positionCount = steps;
                lr.SetPosition(j, points[i, j]);
            }
        }
    }

    private class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(CelestialBody body) {
            position = body.transform.position;
            velocity = body.velocity;
            mass = body.mass;
        }
    }
}