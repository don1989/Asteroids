using UnityEngine;
using System.Collections;

public class Enemy : WrappableObject {

    protected int m_scoreToAdd;
    protected float movementAmount = 5.0f;

	// Use this for initialization
    protected override void Start() 
    {
        base.Start();

        
	}
	
    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
        
        // TODO: Move random amounts
        
	}

    protected void OnDestroyed()
    {
        //   m_scoreToAdd;
    }

}
