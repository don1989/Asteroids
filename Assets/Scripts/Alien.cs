using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour
{

    Vector3 m_velocity;
    public const float m_shootCooldown = 2.0f;
    public float m_shootCooldownTimer;
    private float movementAmount = 5.0f;

    private Rigidbody m_rigidBody;

    void Start () 
    {
        int RandomInt = Random.Range(0, 2);

        m_velocity = new Vector3( RandomInt > 0 ? - 1 : 1, 0, 0);

        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;


        m_shootCooldownTimer = 0;
	}
	
    void FixedUpdate()
    {
        // Go left/right
        HandleMovement();

        Shoot();

        m_shootCooldownTimer -= Time.deltaTime;
	}

    private void HandleMovement()
    {
        transform.Translate(m_velocity * Time.deltaTime * movementAmount);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Bullet"))
        {
            // Shot by the player
            if ( col.gameObject.tag.Contains("Player") )
            {
                GameObject particles = Instantiate(SpawnerScript.Instance.ExplosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
                Destroy(particles, 5.0f);

                GameController.Instance.IncrementScore(20);

                Destroy(col.gameObject); // Destroy bullet
                Destroy(gameObject); // Destroy self
            }
        }
        else if (col.gameObject.name.Contains("Player"))
        {
            GameObject toDie = col.gameObject.transform.parent.gameObject;
            if (toDie.GetComponent<Player>().IsVulnerable())
            {
                GameObject particles = Instantiate(SpawnerScript.Instance.ExplosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
                Destroy(particles, 5.0f);

                // Dead
                GameController.Instance.DeductLife(toDie.transform.position, toDie.transform.rotation);

                Destroy(toDie);
                Destroy(gameObject); // Destroy myself
            }
        }
    }

    void Shoot()
    {
        if (m_shootCooldownTimer <= 0.0f)
        {
            m_shootCooldownTimer = m_shootCooldown;

            GameObject obj = Instantiate(SpawnerScript.Instance.AlienBulletPrefab, m_rigidBody.transform.position, Quaternion.identity) as GameObject;
            Bullet bullet = obj.GetComponent<Bullet>();
            
            // Random direction
            float randomX = Random.Range(-1.0f, 1.0f);
            float randomY = Random.Range(-1.0f, 1.0f);

            Vector3 randomDirection = new Vector3(randomX, randomY, 0);
            bullet.Direction = randomDirection.normalized;
        }
    }
}
