using UnityEngine;
using System.Collections;

public class WrappableObject : MonoBehaviour {

    private float m_extent = 70.0f;

    void Start()
    {

    }

    void FixedUpdate() 
    {

        // (0, 0) is the bottom left corner in pixel coordinates, and the top-right corner is (Screen.width, Screen.height).
        // Check bounds and screen wrap
        Vector3 tmpPos = Camera.main.WorldToScreenPoint (transform.position);

        if (tmpPos.x > (Screen.width + m_extent) ) 
        {
            transform.position = new Vector3(-transform.position.x + 1, transform.position.y, 0);
        }
        else if ( tmpPos.x < -m_extent )
        {
            transform.position = new Vector3(-transform.position.x - 1, transform.position.y, 0);
        }

        else if (tmpPos.y > (Screen.height + m_extent) ) 
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y + 1, 0);
        }
        else if ( tmpPos.y < -m_extent )
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y - 1, 0);
        }
	}

}
