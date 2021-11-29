using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Modeled on Sebastian Lague's N-Body Gravity Simulation
* https://github.com/SebLague/Solar-System
*/

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class CelestialBody : GravityBody
{
    public float radius;
    public float gravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed Body";
    public BodyType bodyType;
    public Vector3 velocity { get; private set; }
    public float mass { get; private set; }
    private Rigidbody rb;
    private NBodySimulation sim;

    // Initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        sim = FindObjectOfType<NBodySimulation>();
    }

    // Update velocity by providing a force directly
    public void UpdateVelocity(Vector3 force, float timeStep) {
        velocity += force * timeStep;
    }

    // Update actual position according to velocity
    public void UpdatePosition(float timeStep) {
        rb.MovePosition(rb.position + velocity * timeStep);
    }

    // Recalculate object after changing things in editor
    void LateUpdate() {
        if (!sim.simulationRunning)
        {
            mass = gravity * radius * radius / Universe.G;
            gameObject.name = bodyName;
            transform.localScale = Vector3.one * radius;
            velocity = initialVelocity;
        }
    }

    public void PrepSim() {
        rb.mass = mass;
        velocity = initialVelocity;
    }

    // Getters
    public Rigidbody Rigidbody {
        get { return rb; }
    }

    public Vector3 Position {
        get { return rb.position; }
    }
}
