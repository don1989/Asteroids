using UnityEngine;
using System.Collections;

public abstract class WrappableObject : MonoBehaviour {

    private float m_extent = 70.0f;

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate() 
    {

        // (0, 0) is the bottom left corner in pixel coordinates, and the top-right corner is (Screen.width, Screen.height).
        // Check bounds and screen wrap
        Vector3 tmpPos = Camera.main.WorldToScreenPoint (transform.position);

        if (tmpPos.x > (Screen.width + m_extent) || tmpPos.x < -m_extent) 
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }

        if (tmpPos.y > (Screen.height + m_extent) || tmpPos.y < -m_extent)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
        }
	}

}
