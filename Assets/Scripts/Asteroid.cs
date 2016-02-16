using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float m_scaleThreshold = 20.0f; // Smallest we're allowed

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Bullet"))
        {
            if (col.gameObject.CompareTag("Player")) // Player's bullet
            {
                GameController.Instance.IncrementScore(10);

                Destroy(col.gameObject); // Destroy bullet
                BreakUp();
            }
            // Don't do anything for Alien's bullet
        }
        else if (col.gameObject.name.Contains("Player"))
        {
            GameObject toDie = col.gameObject.transform.parent.gameObject;
            if ( toDie.GetComponent<Player>().Vulnerable )
            {
                GameObject particles = Instantiate(SpawnerScript.Instance.ExplosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
                Destroy(particles, 5.0f);

                // Dead
                GameController.Instance.DeductLife(toDie.transform.position, toDie.transform.rotation);
                GameController.Instance.PlayExplosionAudio();
                Destroy(toDie);

                BreakUp();
            }
        }
        // Alien currently indestructible
        /*
        else if (col.gameObject.name.Contains("Alien"))
        {
            GameObject particles = Instantiate(SpawnerScript.Instance.ExplosionParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 5.0f);

            Destroy(col.gameObject.transform.parent.gameObject);
            BreakUp();
        }*/
    }
    
    private void BreakUp()
    {
        GameObject particles = Instantiate(SpawnerScript.Instance.AsteroidParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(particles, 5.0f);

        // Get half the max scale of this game object
        float halfScale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z) * 0.5f;

        // If we are big enough, break up into two (i.e. create 2 more with half of our scale
        if (halfScale > m_scaleThreshold)
        {
            GameObject obj1 = Instantiate(SpawnerScript.Instance.AsteroidPrefab, transform.position, Quaternion.identity) as GameObject;
            GameObject obj2 = Instantiate(SpawnerScript.Instance.AsteroidPrefab, transform.position, Quaternion.identity) as GameObject;

            obj1.transform.localScale = new Vector3(halfScale, halfScale, halfScale);
            obj2.transform.localScale = new Vector3(halfScale, halfScale, halfScale);
        }

        // Then destroy this object
        Destroy(gameObject);
    }

}
