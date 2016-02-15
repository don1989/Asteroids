using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    private const float m_scaleThreshold = 20.0f; // Smallest we're allowed
    public float m_movementAmount;
    Vector3 m_direction;
    static GameController gameController = null;

    void Start() 
    {
        m_movementAmount = 3.0f;

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        m_direction = new Vector3(x, y, 0);
        m_direction.Normalize();

        // Get reference to game controller
        if ( gameController == null )
        {
            GameObject obj = GameObject.Find("GameController");
            gameController = (GameController) obj.GetComponent<GameController>() ;
        }
	}

    void FixedUpdate() 
    {
        HandleMovement();
	}

    private void HandleMovement()
    {
        transform.Translate(m_direction * m_movementAmount * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.enabled)
        {
            if (col.gameObject.name.Contains("Bullet"))
            {
                if (col.gameObject.tag.Contains("Player"))
                {
                    Destroy(col.gameObject); // Destroy bullet
                    BreakUp();
                }

                gameController.IncrementScore(10);
            }
            else if (col.gameObject.name.Contains("Player"))
            {
                GameObject toDie = col.gameObject.transform.parent.gameObject;
                if ( toDie.GetComponent<Player>().IsVulnerable() )
                {
                    GameObject particles = Instantiate(Resources.Load("Prefabs/ExplosionParticles"), transform.position, Quaternion.identity) as GameObject;
                    Destroy(particles, 5.0f);

                    // Dead
                    gameController.DeductLife(toDie.transform.position, toDie.transform.rotation);

                    Destroy(toDie);

                    BreakUp();
                }
            }
            else if (col.gameObject.name.Contains("Alien"))
            {
                GameObject particles = Instantiate(Resources.Load("Prefabs/ExplosionParticles"), transform.position, Quaternion.identity) as GameObject;
                Destroy(particles, 5.0f);

                Destroy(col.gameObject.transform.parent.gameObject);
                BreakUp();

            }
        }
    }
    
    private void BreakUp()
    {
        GameObject particles = Instantiate(Resources.Load("Prefabs/AsteroidParticles"), transform.position, Quaternion.identity) as GameObject;
        Destroy(particles, 5.0f);

        // Get half the max scale of this game object
        float halfScale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z) * 0.5f;

        // If we are big enough, break up into two (i.e. create 2 more with half of our scale
        if (halfScale > m_scaleThreshold)
        {
            GameObject obj1 = Instantiate(Resources.Load("Prefabs/Asteroid"), transform.position, Quaternion.identity) as GameObject;
            GameObject obj2 = Instantiate(Resources.Load("Prefabs/Asteroid"), transform.position, Quaternion.identity) as GameObject;

            obj1.transform.localScale = new Vector3(halfScale, halfScale, halfScale);
            obj2.transform.localScale = new Vector3(halfScale, halfScale, halfScale);
        }

        // Then destroy this object
        Destroy(gameObject);
    }

}
