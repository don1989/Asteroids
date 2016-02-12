using UnityEngine;
using System.Collections;

public class Bullet : WrappableObject {

	// Use this for initialization
    private float m_speed = 20.0f;

	protected override void Start () 
    {
        base.Start();
	}


    protected override void FixedUpdate() 
    {
        base.FixedUpdate();

	    // We want this to move along its direction vector
        transform.Translate(transform.forward * (m_speed * Time.deltaTime));
	}
}
