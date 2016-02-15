using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    //public Vector3 m_initialDirection;
    public float m_rotationAmount;
    public float m_thrustAmount;
    public const float m_shootCooldown = 0.3f;
    public float m_shootCooldownTimer;

    private Rigidbody m_rigidBody;
    private bool m_isVulnerable = true;
    private float m_vulnerableTime = 0.0f;
    private float m_blinkTime = 0.0f;

    static GameController gameController = null;
    ParticleSystem.EmissionModule m_thrustEmissions;

	void Start () 
    {
        // Constrain to x-y axis
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        m_shootCooldownTimer = 0;

        // Get reference to game controller
        if (gameController == null)
        {
            GameObject obj = GameObject.Find("GameController");
            gameController = (GameController)obj.GetComponent<GameController>();
        }

        ParticleSystem thrustParticleSystem = GetComponentInChildren<ParticleSystem>();
        if ( thrustParticleSystem )
        {
            m_thrustEmissions = thrustParticleSystem.emission;
        }
	}
	
	void FixedUpdate () 
    {

        HandleMovement();
        


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

    private void HandleMovement()
    {
        // Rotation
        float h = Input.GetAxis("Horizontal") * m_rotationAmount * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(h, 0, 0) * Time.deltaTime);
        m_rigidBody.MoveRotation(m_rigidBody.rotation * deltaRotation);


        // Thrust
        float t = Input.GetAxis("Vertical") * m_thrustAmount * Time.deltaTime;
        if (t > 0)
        {
            m_rigidBody.AddForce(m_rigidBody.transform.forward * t);
            m_thrustEmissions.enabled = true;
        }
        else
        {
            m_thrustEmissions.enabled = false;
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

    private void OnTriggerEnter(Collider col)
    {
        if ( m_isVulnerable )
        {
            if (col.gameObject.name.Contains("Bullet"))
            {
                // Shot by the alien
                if (col.gameObject.tag.Contains("Alien"))
                {
                    GameObject particles = Instantiate(Resources.Load("Prefabs/ExplosionParticles"), transform.position, Quaternion.identity) as GameObject;
                    Destroy(particles, 5.0f);

                    Destroy(col.gameObject); // Destroy bullet

                    gameController.DeductLife(transform.position, transform.rotation);

                    Destroy(gameObject); // Destroy self
                }
            }
        }
    }

}
