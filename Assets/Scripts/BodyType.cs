using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewBodyType", menuName = "ScriptableObjects/Celestial Body Type", order = 1)]
public class BodyType : ScriptableObject
{
    public string className;
    public Color defaultColor;
    public Texture2D icon;
}