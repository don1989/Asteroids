using UnityEngine;
using System.Collections;

public class Alien : Enemy {

	// Use this for initialization
	protected override void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
    protected override void FixedUpdate()
    {
        // Go left/right
        

        // Shoot at player
        base.FixedUpdate();
	}
}
