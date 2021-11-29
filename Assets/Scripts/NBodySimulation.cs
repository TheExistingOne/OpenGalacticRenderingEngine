using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Modeled on Sebastian Lague's N-Body Gravity Simulation
* https://github.com/SebLague/Solar-System
*/

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] bodies;
    static NBodySimulation instance;

    [System.NonSerialized]
    public bool simulationRunning = false;

    public void StartSim()
    {
        simulationRunning = true;
        CelestialBody[] objects = FindObjectsOfType<CelestialBody>();
        foreach (CelestialBody body in objects)
        {
            body.PrepSim();
        }
    }

    // Initialize the simulation
    void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        Debug.Log("FixedDeltaTime: " + Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if(simulationRunning)
        {
            // Apply physics to each body
            for (int i = 0; i < bodies.Length; i++)
            {   
                Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
                bodies[i].UpdateVelocity(acceleration, Universe.physicsTimeStep);
            }

            // Update positions of all bodies    
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].UpdatePosition(Universe.physicsTimeStep);
            }
        }
    }

    public static Vector3 CalculateAcceleration(Vector3 position, CelestialBody ignoreBody = null) {
        Vector3 acceleration = Vector3.zero;

        foreach (CelestialBody body in Instance.bodies) {
            // If this is the body we're ignoring, skip it
            if (body != ignoreBody) {
                // Calculate distance and direction to body
                float sqrDistance = (body.Position - position).sqrMagnitude;
                Vector3 forceDir = (body.Position - position).normalized;

                // Net
                acceleration += forceDir * Universe.G * body.mass / sqrDistance;
            }
        }

        return acceleration;
    }
    

    // Getters
    public static CelestialBody[] Bodies
    {
        get
        {
            return instance.bodies;
        }
    }

    public static NBodySimulation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NBodySimulation>();
            }
            return instance;
        }
    }
}
