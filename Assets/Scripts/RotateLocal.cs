using UnityEngine;
using System.Collections;

public class RotateLocal : MonoBehaviour {

    private float m_randomRotationSpeedValue;
    
	void Start () 
    {

        m_randomRotationSpeedValue = Random.Range(500.0f, 1000.0f);

        // Initialise each child object with a random location
        foreach (Transform child in transform)
        {
            child.localRotation = Random.rotation;
        }

	}
	

	void FixedUpdate ()
    {
        // Rotate
        foreach (Transform child in transform)
        {
            float val = m_randomRotationSpeedValue * Time.deltaTime;

            Quaternion deltaRotation = child.localRotation * Quaternion.Euler(new Vector3(val, 0, 0) * Time.deltaTime);
            child.localRotation = deltaRotation;
        }
	}
}
