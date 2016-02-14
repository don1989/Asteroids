using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    // Number of objects to spawn
    private int m_asteroidsStart = 5;
    private int m_aliensStart = 1;

    private int m_asteroidsToSpawn = 5;
    private int m_aliensToSpawn = 1;

    // The time they will spawn in, in seconds
    private float m_asteroidSpawnTime = 0.0f;
    private float m_alienSpawnTime = 10.0f;

    // Time in seconds to spawn the next one
    private float m_asteroidSpawnCooldown = 6.0f;
    private float m_alienSpawnCooldown = 15.0f;

    private const int m_asteroidRandomPositions = 4;
    Vector3[] m_asteroidSpawnPosition;

    private const int m_alienRandomPositions = 2;
    Vector3[] m_alienSpawnPosition;

	void Start () 
    {
        m_asteroidSpawnPosition = new Vector3[m_asteroidRandomPositions];
        m_asteroidSpawnPosition[0] = new Vector3(0, 10, 0);
        m_asteroidSpawnPosition[1] = new Vector3(0, -10, 0);
        m_asteroidSpawnPosition[2] = new Vector3(18, 0, 0);
        m_asteroidSpawnPosition[3] = new Vector3(-18, 0, 0);

        m_alienSpawnPosition = new Vector3[m_alienRandomPositions];
        m_alienSpawnPosition[0] = new Vector3(-18, 4, 0);
        m_alienSpawnPosition[1] = new Vector3(18, -4, 0);

        m_asteroidsToSpawn = m_asteroidsStart;
        m_aliensToSpawn = m_aliensStart;

	}

    public void NextWave()
    {
        m_asteroidsStart += 2;
        ++m_aliensStart;

        m_asteroidsToSpawn = m_asteroidsStart;
        m_aliensToSpawn = m_aliensStart;

        m_asteroidSpawnTime = 0.0f;
        m_alienSpawnTime = 10.0f;

        Debug.Log("Next Wave");
    }
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_asteroidsToSpawn > 0 && m_asteroidSpawnTime <= 0 )
        {
            int randomInt = Random.Range(0, m_asteroidRandomPositions);

            GameObject obj = Instantiate(Resources.Load("Prefabs/Asteroid"), m_asteroidSpawnPosition[randomInt], Quaternion.identity) as GameObject;
            obj.transform.localScale = new Vector3(80, 90, 100);

            m_asteroidSpawnTime = m_asteroidSpawnCooldown;
            --m_asteroidsToSpawn;
        }

        if (m_aliensToSpawn > 0 && m_alienSpawnTime <= 0)
        {
            int randomInt = Random.Range(0, m_alienRandomPositions);

            Instantiate(Resources.Load("Prefabs/Alien"), m_alienSpawnPosition[randomInt], Quaternion.identity);

            m_alienSpawnTime = m_alienSpawnCooldown;
            --m_aliensToSpawn;
        }

        m_asteroidSpawnTime -= Time.deltaTime;
        m_alienSpawnTime -= Time.deltaTime;
	}

    public int AsteroidsToSpawnThisWave()
    {
        return m_asteroidsToSpawn;
    }
}
