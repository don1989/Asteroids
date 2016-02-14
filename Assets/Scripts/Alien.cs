using UnityEngine;
using System.Collections;

public class Alien : Enemy {

    Vector3 m_velocity;
    public const float m_shootCooldown = 2.0f;
    public float m_shootCooldownTimer;
    //private float movementAmount = 15.0f;

    private Rigidbody m_rigidBody;
    static GameController gameController = null;

	protected override void Start () 
    {
        base.Start();

        m_velocity = new Vector3( -1, 0, 0 );

        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;


        m_shootCooldownTimer = 0;

        // Get reference to game controller
        if (gameController == null)
        {
            GameObject obj = GameObject.Find("GameController");
            gameController = (GameController)obj.GetComponent<GameController>();
        }
	}
	
    protected override void FixedUpdate()
    {

        base.FixedUpdate();

        // Go left/right
        transform.Translate(m_velocity * Time.deltaTime * movementAmount);

        Shoot();

        m_shootCooldownTimer -= Time.deltaTime;
	}

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Collision " + col.gameObject.name);
        if (col.gameObject.name.Contains("Bullet"))
        {
            // Shot by the player
            if ( col.gameObject.tag.Contains("Player") )
            {
                gameController.IncrementScore(20);

                Destroy(col.gameObject); // Destroy bullet
                Destroy(gameObject); // Destroy self

            }
        }
        else if (col.gameObject.name.Contains("Player"))
        {
            GameObject toDie = col.gameObject.transform.parent.gameObject;
            // Dead
            gameController.DeductLife(toDie.transform.position, toDie.transform.rotation);

            Destroy(toDie);
            Destroy(gameObject); // Destroy myself

            
        }
        else if (col.gameObject.name.Contains("Alien"))
        {

        }
    }

    void Shoot()
    {
        if (m_shootCooldownTimer <= 0.0f)
        {
            m_shootCooldownTimer = m_shootCooldown;

            GameObject obj = Instantiate(Resources.Load("Prefabs/Bullet"), m_rigidBody.transform.position, Quaternion.identity) as GameObject;
            obj.tag = "Alien";

            if (obj != null)
            {
                Bullet bullet = obj.GetComponent<Bullet>();
                if (bullet)
                {
                    // Random direction
                    float randomX = Random.Range(-1.0f, 1.0f);
                    float randomY = Random.Range(-1.0f, 1.0f);

                    Vector3 randomDirection = new Vector3(randomX, randomY, 0);
                    bullet.SetDirection(randomDirection.normalized);
                }
            }
        }
    }
}
