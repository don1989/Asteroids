using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

    public float m_movementAmount = 3.0f; // Should be set in editor
    private Vector3 m_direction;

	void Start () 
    {

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        m_direction = new Vector3(x, y, 0);
        m_direction.Normalize();
	}
	
	void FixedUpdate () 
    {
        transform.Translate(m_direction * m_movementAmount * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
	}
}
