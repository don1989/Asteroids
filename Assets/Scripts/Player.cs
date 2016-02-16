using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float m_rotationAmount;
    public float m_thrustAmount;
    public float m_shootCooldownTimer;
    private const float m_shootCooldown = 0.3f;

    private float m_vulnerableTime = 0.0f;
    private float m_vulnerableTimeSet = 5.0f;
    private float m_blinkTime = 0.0f;

    private bool m_isVulnerable = true;
    private bool m_powerupEnabled = false;
    private bool m_isPlaying = false;

    private Rigidbody m_rigidBody;
    private ParticleSystem.EmissionModule m_thrustEmissions;

	void Start () 
    {
        // Constrain to x-y axis
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        m_shootCooldownTimer = 0;

        ParticleSystem thrustParticleSystem = GetComponentInChildren<ParticleSystem>();
        if ( thrustParticleSystem )
        {
            m_thrustEmissions = thrustParticleSystem.emission;
            m_thrustEmissions.enabled = false;
        }
	}
	
	void FixedUpdate () 
    {
        Debug.Log(m_isPlaying.ToString());
        if (m_isPlaying)
        {
            HandleMovement();
            HandleShooting();
            HandleVulnerability();
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

    private void HandleShooting()
    {
        // Fire bullets
        if (m_shootCooldownTimer <= 0.0f && (Input.GetAxis("Fire1") == 1.0f || Input.GetKeyDown("space")))
        {
            GameController.Instance.PlayShootingAudio();
            m_shootCooldownTimer = m_shootCooldown;

            Vector3 middlePos = m_rigidBody.transform.position + (m_rigidBody.transform.forward.normalized * 1.2f);
            GameObject obj = Instantiate(SpawnerScript.Instance.PlayerBulletPrefab, middlePos, Quaternion.identity) as GameObject;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.Direction = m_rigidBody.transform.forward.normalized;

            // TODO: make it based on player's scale!
            if (m_powerupEnabled)
            {
                Vector3 leftPos = m_rigidBody.transform.position + (m_rigidBody.transform.up.normalized * 0.9f) + (m_rigidBody.transform.forward.normalized * 1.1f);
                Vector3 rightPos = m_rigidBody.transform.position - (m_rigidBody.transform.up.normalized * 0.9f) + (m_rigidBody.transform.forward.normalized * 1.1f);

                GameObject obj2 = Instantiate(SpawnerScript.Instance.PlayerBulletPrefab, leftPos, Quaternion.identity) as GameObject;
                GameObject obj3 = Instantiate(SpawnerScript.Instance.PlayerBulletPrefab, rightPos, Quaternion.identity) as GameObject;

                Bullet bullet2 = obj2.GetComponent<Bullet>();
                bullet2.Direction = m_rigidBody.transform.forward.normalized;

                Bullet bullet3 = obj3.GetComponent<Bullet>();
                bullet3.Direction = m_rigidBody.transform.forward.normalized;
            }
        }

        if (m_shootCooldownTimer > 0.0f)
        {
            m_shootCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleVulnerability()
    {
        if (!m_isVulnerable)
        {
            if (m_blinkTime <= 0.0f)
            {
                MeshRenderer myRenderer = GetComponentInChildren<MeshRenderer>();
                myRenderer.enabled = !myRenderer.enabled;
                m_blinkTime = 0.3f;
            }

            m_blinkTime -= Time.deltaTime;


            if (m_vulnerableTime <= 0.0f)
            {
                MeshRenderer myRenderer = GetComponentInChildren<MeshRenderer>();
                myRenderer.enabled = true;

                m_isVulnerable = true;
            }

            m_vulnerableTime -= Time.deltaTime;
        }
    }

    public bool Vulnerable
    {
        get { return m_isVulnerable; }
        set 
        {
            m_isVulnerable = value;
            if ( !m_isVulnerable )
            {
                m_vulnerableTime = m_vulnerableTimeSet;
            }
        }
    }

    public bool Playing
    {
        get { return m_isPlaying;  }
        set { m_isPlaying = value; }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Bullet"))
        {
            if ( m_isVulnerable )
            {
                // Shot by the alien
                if (col.gameObject.tag.Contains("Alien"))
                {
                    GameObject particles = Instantiate(SpawnerScript.Instance.ExplosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
                    Destroy(particles, 5.0f);

                    Destroy(col.gameObject); // Destroy bullet

                    GameController.Instance.DeductLife(transform.position, transform.rotation);
                    GameController.Instance.PlayExplosionAudio();

                    Destroy(gameObject); // Destroy self
                }
            }
        }
        else if (col.gameObject.CompareTag("Powerup"))
        {
            GameController.Instance.PlayPowerupAudio();

            Destroy(col.gameObject.transform.parent.gameObject);

            m_powerupEnabled = true;
        }
    }
}
