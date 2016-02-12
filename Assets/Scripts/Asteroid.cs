using UnityEngine;
using System.Collections;

public class Asteroid : Enemy {

    protected override void Start() 
    {
        base.Start();

        // TODO: Initialise with a random velocity
        transform.forward = new Vector3(0.3f, 0.4f, 0);
        transform.forward.Normalize();

        transform.localRotation = Random.rotation;
	}

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();

        // TODO: Rotate randomly
        float h = 300 * Time.deltaTime;
        foreach (Transform child in transform)
        {
            if (child.name == "Sphere")
            {
                Quaternion deltaRotation = child.localRotation * Quaternion.Euler(new Vector3(h, 0, 0) * Time.deltaTime);
                child.localRotation = deltaRotation;
            }
        }

        // If collision with bullet
        {
            // if scale is big enough
            {
                // Generate 2 more asteroids
            }
            // Destroy self
        }


	}
}
