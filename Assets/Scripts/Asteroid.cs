using UnityEngine;
using System.Collections;

public class Asteroid : Enemy {

    private const float m_scaleThreshold = 25.0f; // Smallest we're allowed

    static GameController gameController = null;
    protected override void Start() 
    {
        base.Start();

        // TODO: Initialise with a random velocity
        transform.forward = new Vector3(0.3f, 0.4f, 0);
        transform.forward.Normalize();

        transform.localRotation = Random.rotation;

        // Get reference to game controller
        if ( gameController == null )
        {
            GameObject obj = GameObject.Find("GameController");
            gameController = (GameController) obj.GetComponent<GameController>() ;
        }
	}

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();

        // Move
        transform.Translate(transform.forward * movementAmount * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;



        // TODO: Rotate randomly
        float h = 300 * Time.deltaTime;
        foreach (Transform child in transform)
        {
            if (child.name == "Sphere")
            {
                Quaternion deltaRotation = child.localRotation * Quaternion.Euler(new Vector3(h, 0, 0) * Time.deltaTime);
                child.localRotation = deltaRotation;
                break;
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
                    // Dead
                    gameController.DeductLife(toDie.transform.position, toDie.transform.rotation);

                    Destroy(toDie);

                    BreakUp();
                }
            }
            else if (col.gameObject.name.Contains("Alien"))
            {
                Destroy(col.gameObject.transform.parent.gameObject);
                BreakUp();

            }
        }
    }
    
    private void BreakUp()
    {
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
