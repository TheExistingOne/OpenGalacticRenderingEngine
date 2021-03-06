using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlanet : MonoBehaviour
{
    [System.NonSerialized]
    public CelestialBody selected;
    private Camera source;

    private NBodySimulation sim;

    void Start()
    {
        source = Camera.main;
        sim = FindObjectOfType<NBodySimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !sim.simulationRunning)
        {
            // Raycast from the camera to the mouse
            Ray ray = source.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // If we hit something labelled as a celestial body and it has the script, save it
                if (hit.transform.tag == "Body" && hit.transform.GetComponent<CelestialBody>() != null)
                {
                    selected = hit.transform.GetComponent<CelestialBody>();
                    FindObjectOfType<UIEditPanel>().PopulateFields();
                }
            }
        }
    }

    void LateUpdate()
    {
        if (sim.simulationRunning) {
            selected = null;
        }
    }
}
