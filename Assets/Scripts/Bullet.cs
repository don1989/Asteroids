using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float m_speed;
    public float m_lifetime;

    private Vector3 m_direction; // Direction to be set by shooter

	void Start ()
    {
	}

    void FixedUpdate() 
    {
	    // We want this to move along its direction vector
        transform.Translate(m_direction * (m_speed * Time.deltaTime));

        Vector3 tmpPos = transform.position;
        tmpPos.z = 0;
        transform.position = tmpPos;
        
        m_lifetime -= Time.deltaTime;
        if ( m_lifetime < 0 )
        {
            Destroy(gameObject);
        }
	}

    public Vector3 Direction
    {
        get { return m_direction; }
        set { m_direction = value; }
    }
}
