using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    //public Vector3 m_initialDirection;
    public float m_rotationAmount;
    public float m_thrustAmount;

    private Rigidbody m_rigidBody;
    
	void Start () 
    {
        // Constrain to x-y axis
        m_rigidBody = GetComponent<Rigidbody>();
        //m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        //transform.rotation = Quaternion.LookRotation(m_initialDirection);
        //m_rigidBody.transform.forward = new Vector3(1, 0, 0);
	}
	
	void FixedUpdate () 
    {
        // Rotation
        float h = Input.GetAxis("Horizontal") * m_rotationAmount * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(h, 0, 0) * Time.deltaTime);
        m_rigidBody.MoveRotation(m_rigidBody.rotation * deltaRotation);

        // Using torque instead makes it super hard
        // m_rigidBody.AddTorque(transform.up * h); 



        // Thrust
        float t = Input.GetAxis("Vertical") * m_thrustAmount * Time.deltaTime;
        //m_rigidBody.AddForce(transform.forward * t);
        m_rigidBody.AddForce(m_rigidBody.transform.forward * t);


        // Fire bullets
        if ( Input.GetAxis("Fire1") == 1.0f )
        {
            //GameObject obj = (GameObject)Instantiate(Resources.Load("BulletPrefab"), m_rigidBody.transform.position, Quaternion.identity);
            // TODO: Recycle bullets
            //obj.transform.forward = m_rigidBody.transform.forward;
        }
	}
}
