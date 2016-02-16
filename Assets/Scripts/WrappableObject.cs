using UnityEngine;
using System.Collections;

public class WrappableObject : MonoBehaviour {

    public int m_extent = 200; // breathing room

    private int m_minExtentX;
    private int m_maxExtentX;

    private int m_minExtentY;
    private int m_maxExtentY;

    void Start()
    {
        m_minExtentX = -m_extent;
        m_minExtentY = -m_extent;

        m_maxExtentX = Screen.width + m_extent;
        m_maxExtentY = Screen.height + m_extent;
    }

    void FixedUpdate() 
    {
        // (0, 0) is the bottom left corner in pixel coordinates, and the top-right corner is (Screen.width, Screen.height).
        // Check bounds and flip the value where we go out of range - push it in slightly so we don't trigger it on the other side
        Vector3 tmpPos = Camera.main.WorldToScreenPoint (transform.position);

        if (tmpPos.x > m_maxExtentX) 
        {
            transform.position = new Vector3(-transform.position.x + 1, transform.position.y, 0);
        }
        else if (tmpPos.x < m_minExtentX)
        {
            transform.position = new Vector3(-transform.position.x - 1, transform.position.y, 0);
        }

        else if (tmpPos.y > m_maxExtentY) 
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y + 1, 0);
        }
        else if (tmpPos.y < m_minExtentY)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y - 1, 0);
        }
	}

}
