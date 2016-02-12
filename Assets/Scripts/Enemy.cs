using UnityEngine;
using System.Collections;

public class Enemy : WrappableObject {

    protected int m_scoreToAdd;
    private float movementAmount = 20.0f;

	// Use this for initialization
    protected override void Start() 
    {
        base.Start();

        
	}
	
    protected override void FixedUpdate() 
    {
        base.FixedUpdate();

        // TODO: Move random amounts
        transform.Translate(transform.forward * movementAmount * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
	}

    protected void OnDestroyed()
    {
        //   m_scoreToAdd;
    }

}
