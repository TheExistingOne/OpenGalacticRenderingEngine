using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
* Adapted from Sebastian Lague's N-Body Simulation
* https://github.com/SebLague/Solar-System
*/

[CustomEditor(typeof(GravityBody), true)]
[CanEditMultipleObjects]
public class BodyDebug : Editor
{
    GravityBody body;
    bool showDebugInfo;

    // Intercept Inspecter Draw Call
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Now do our stuff
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
        // Hide our stuff unless we want it
        showDebugInfo = EditorGUILayout.Foldout(showDebugInfo, "Show Debug Info");
        if (showDebugInfo)
        {
            // Get debug data
            string[] gravityInfo = GetGravityInfo(body.transform.position, body as CelestialBody);
            // Show it
            foreach (string info in gravityInfo)
            {
                EditorGUILayout.LabelField(info);
            }
        }
    }

    void OnEnable() 
    {
        body = (GravityBody)target;
        showDebugInfo = EditorPrefs.GetBool(body.gameObject.name + nameof(showDebugInfo), false);
    }

    void OnDisable()
    {
        if(body) {
            EditorPrefs.SetBool(body.gameObject.name + nameof(showDebugInfo), showDebugInfo);
        }
    }

    static string[] GetGravityInfo(Vector3 point, CelestialBody ignore = null)
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        Vector3 totalAccel = Vector3.zero;

        var forceAndName = new List<FloatAndString>();
        foreach (CelestialBody body in bodies) 
        {
            // Complecated acceleration math
            if (body != ignore) {
                var offsetToBody = body.Position - point;
                var sqrDistance = offsetToBody.sqrMagnitude;
                float distance = Mathf.Sqrt(sqrDistance);
                var direction = offsetToBody / distance;
                var accel = Universe.G * body.mass / sqrDistance;
                totalAccel += accel * direction;
                forceAndName.Add(new FloatAndString(){floatValue = accel, stringValue = body.gameObject.name});
            }
        }

        // Sort by force
        forceAndName.Sort((a, b) => b.floatValue.CompareTo(a.floatValue));
        string[] info = new string[forceAndName.Count + 1];
        // Add the acceleration and object to the info list
        info[0] = $"acc: {totalAccel} (mag = {totalAccel.magnitude})";
        for (int i = 0; i < forceAndName.Count; i++)
        {
            info[i + 1] = $"acceleration due to {forceAndName[i].stringValue}: {forceAndName[i].floatValue}";
        }
        return info;
    }

    struct FloatAndString {
        public float floatValue;
        public string stringValue;
    }
}
