using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour {

    public float extent = 8.0f;
	void Start () 
    {
	    // Initialise with a random velocity
        transform.forward = new Vector3(0.3f, 0.4f, 0);
        transform.forward.Normalize();
	}
	
	void FixedUpdate () 
    {
        const float amount = 2.0f;
        transform.Translate(transform.forward * amount * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        CheckBounds();
	}

    void CheckBounds ()
    {
        if (transform.position.y > extent || transform.position.y < -extent)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
            Debug.Log("Reached Y extent");
        }

        if (transform.position.x > extent || transform.position.x < -extent)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
            Debug.Log("Reached X extent");
        }
    }
}
