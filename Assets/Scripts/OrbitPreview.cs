using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitPreview : MonoBehaviour 
{   
    [Header("Physics data")]
    public int steps = 10;
    public bool usePhysicsTime = false;

    [Header("Relative tracking")]
    public bool relativeTo;
    public CelestialBody relativeBody;

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

        DrawOrbits();
    }
    
    void DrawOrbits()
    {
        // Find all tracked planets and create the array of simulation points
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        VirtualBody[] virtualBodies = new VirtualBody[bodies.Length];

        // Create path points array
        Vector3[,] points = new Vector3[virtualBodies.Length, steps];

        // Relative tracking prep
        int referenceFrameIndex = 0;
        Vector3 referenceFrame = Vector3.zero;
        
        // Load the simulation points and set up relative tracking if enabled
        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);

            if (bodies[i] == relativeBody && relativeTo)
            {
                referenceFrameIndex = i;
                referenceFrame = virtualBodies[i].position;
            }
        }

        // Loop through all the preview points
        for (int i = 0; i < steps; i++)
        {
            Vector3 referenceBodyPosition = (relativeTo) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;

            // Have every VirtualBody loop over every other VirtualBody
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                foreach (VirtualBody otherBody in virtualBodies)
                {
                    // Check that we're not calcuating the effect of a body and itself
                    if (virtualBodies[j] != otherBody)
                    {
                        // Distance and direction
                        var sqrDistance = (otherBody.position - virtualBodies[j].position).sqrMagnitude;
                        var forceDir = (otherBody.position - virtualBodies[j].position).normalized;

                        // Magnitude and velocity changes
                        var magnitude = Universe.G * otherBody.mass / sqrDistance;
                        var velocityChange = forceDir * magnitude * timeStep;
                        virtualBodies[j].velocity += velocityChange;
                    }
                }
                
                // Calculate position change and apply it
                Vector3 newPos = virtualBodies[j].position + virtualBodies[j].velocity * timeStep;
                virtualBodies[j].position = newPos;

                // Actually calculate relative stuff
                if (relativeTo)
                {
                    var referenceFrameOffset  = referenceBodyPosition - referenceFrame;
                    newPos -= referenceFrameOffset;

                    if(j == referenceFrameIndex) {
                        newPos = referenceFrame;
                    }
                }

                // Append point
                points[j, i] = newPos;
            }
        }

        for (int i = 0; i < virtualBodies.Length; i++)
        {
            for (int j = 0; j < steps; j++)
            {
                LineRenderer lr = bodies[i].gameObject.GetComponent<LineRenderer>();
                lr.startColor = bodies[i].gameObject.GetComponent<Renderer>().sharedMaterial.color;
                lr.endColor = bodies[i].gameObject.GetComponent<Renderer>().sharedMaterial.color;
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