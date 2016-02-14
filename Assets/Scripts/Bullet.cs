using UnityEngine;
using System.Collections;

public class Bullet : WrappableObject {

	// Use this for initialization
    private float m_speed = 20.0f;
    private float m_lifetime = 2.0f;

    Vector3 m_direction;

	protected override void Start () 
    {
        base.Start();
	}


    protected override void FixedUpdate() 
    {
        base.FixedUpdate();

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

    public void SetDirection( Vector3 dir )
    {
        m_direction = dir;
    }
}
