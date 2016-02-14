using UnityEngine;
using System.Collections;

public class Player : WrappableObject {

    //public Vector3 m_initialDirection;
    public float m_rotationAmount;
    public float m_thrustAmount;
    public const float m_shootCooldown = 0.3f;
    public float m_shootCooldownTimer;

    private Rigidbody m_rigidBody;
    private bool m_isVulnerable = true;
    private float m_vulnerableTime = 0.0f;
    private float m_blinkTime = 0.0f;

	protected override void Start () 
    {
        base.Start();

        // Constrain to x-y axis
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        //transform.rotation = Quaternion.LookRotation(m_initialDirection);
        //m_rigidBody.transform.forward = new Vector3(1, 0, 0);

        m_shootCooldownTimer = 0;
        /*m_vulnerableTime = 0.0f;
        m_blinkTime = 0.0f;
        m_isVulnerable = true;*/
	}
	
	protected override void FixedUpdate () 
    {
        base.FixedUpdate();

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
        if ( Input.GetAxis("Fire1") == 1.0f && m_shootCooldownTimer <= 0.0f )
        {
            m_shootCooldownTimer = m_shootCooldown;
             
            GameObject obj = Instantiate(Resources.Load("Prefabs/Bullet"),
                m_rigidBody.transform.position + (m_rigidBody.transform.forward.normalized * 1.2f), 
                Quaternion.identity) as GameObject;
            obj.tag = "Player";

             if (obj != null)
             {
                Bullet bullet = obj.GetComponent<Bullet>();
                 if ( bullet )
                 {
                     
                     bullet.SetDirection(m_rigidBody.transform.forward.normalized);
                     //Debug.Log("Dir " + m_rigidBody.transform.forward.ToString());
                 }
             }
        }

        if ( m_shootCooldownTimer > 0.0f )
        {
            m_shootCooldownTimer -= Time.deltaTime;
        }

        if ( !m_isVulnerable )
        {
            if ( m_blinkTime <= 0.0f )
            {
                MeshRenderer myRenderer = GetComponentInChildren<MeshRenderer>();
                myRenderer.enabled = !myRenderer.enabled;
                m_blinkTime = 0.3f;
            }

            m_blinkTime -= Time.deltaTime;
            

            if ( m_vulnerableTime <= 0.0f )
            {
                MeshRenderer myRenderer = GetComponentInChildren<MeshRenderer>();
                myRenderer.enabled = true;

                m_isVulnerable = true;
            }
            
            m_vulnerableTime -= Time.deltaTime;
        }
	}

    public void SetVulnerable( bool val )
    {
        m_isVulnerable = false;
        m_vulnerableTime = 5.0f;
    }

    public bool IsVulnerable()
    {
        return m_isVulnerable;
    }

}
