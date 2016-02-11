using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	// Use this for initialization
    private float m_speed = 20.0f;

	void Start () 
    {
	}
	

	void FixedUpdate () 
    {
	    // We want this to move along its direction vector
        transform.Translate(transform.forward * (m_speed * Time.deltaTime));
	}
}
