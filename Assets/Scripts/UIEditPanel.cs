using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEditPanel : MonoBehaviour
{
    private NBodySimulation sim;

    [Header("Header Elements")]
    public RawImage image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    [Header("Data Entry")]
    public TMP_InputField attraction;
    public TMP_InputField radius;
    
    //Position
    public TMP_InputField positionX;
    public TMP_InputField positionY;
    public TMP_InputField positionZ;
    
    //Starting Velocity
    public TMP_InputField velocityX;
    public TMP_InputField velocityY;
    public TMP_InputField velocityZ;
    private SelectPlanet planetSelector;

    [Header("Data")]
    public TextMeshProUGUI bodyText;

    void Start()
    {
        planetSelector = GetComponent<SelectPlanet>();
        sim = FindObjectOfType<NBodySimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (planetSelector.selected != null && !sim.simulationRunning)
        {
            image.texture = planetSelector.selected.bodyType.icon;
            nameText.text = planetSelector.selected.bodyName.ToUpper();
            typeText.text = planetSelector.selected.bodyType.className.ToUpper();
            
            if (attraction.text != "")
                planetSelector.selected.gravity = float.Parse(attraction.text);
            if (radius.text != "")
                planetSelector.selected.radius = float.Parse(radius.text, CultureInfo.InvariantCulture.NumberFormat);
            
            if ((positionX.text != "") && (positionY.text != "") && (positionZ.text != ""))
                planetSelector.selected.transform.position = new Vector3(float.Parse(positionX.text), float.Parse(positionY.text), float.Parse(positionZ.text));
            if ((velocityX.text != "") && (velocityY.text != "") && (velocityZ.text != ""))
                planetSelector.selected.initialVelocity = new Vector3(float.Parse(velocityX.text), float.Parse(velocityY.text), float.Parse(velocityZ.text));

            bodyText.text = planetSelector.selected.bodyType.description;
            // What the f** is this mess Unity? Why do I have to do this to work with your UI system? Why?
        }
        else
        {
            nameText.text = "None";
            typeText.text = "Selected";
        }
    }

    public void PopulateFields()
    {
        if (planetSelector.selected != null && !sim.simulationRunning)
        {
            attraction.text = planetSelector.selected.gravity.ToString();
            radius.text = planetSelector.selected.radius.ToString();

            // Position
            positionX.text = planetSelector.selected.transform.position.x.ToString();
            positionY.text = planetSelector.selected.transform.position.y.ToString();
            positionZ.text = planetSelector.selected.transform.position.z.ToString();

            // Velocity
            velocityX.text = planetSelector.selected.initialVelocity.x.ToString();
            velocityY.text = planetSelector.selected.initialVelocity.y.ToString();
            velocityZ.text = planetSelector.selected.initialVelocity.z.ToString();
        }
    }
}
