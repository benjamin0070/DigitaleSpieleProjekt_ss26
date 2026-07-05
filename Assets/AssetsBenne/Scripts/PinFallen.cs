using UnityEngine;
using TMPro;

public class Pin : MonoBehaviour
{

    public float fallenAngle = 45;

    // Update is called once per frame
    public bool IsFallen{
        get
        {
            float angle = Vector3.Angle(transform.up, Vector3.up);
            return angle > fallenAngle;
        }
    }
}
