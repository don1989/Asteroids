using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    // Number of objects to spawn
    private int m_asteroidsStart = 4;
    private int m_aliensStart = 1;

    private int m_asteroidsToSpawn;
    private int m_aliensToSpawn;

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

    private float m_powerupSpawnTime;
    private bool m_powerupHasBeenSpawned;

    private bool m_isPlaying;

    public GameObject AsteroidPrefab;
    public GameObject AlienPrefab;
    public GameObject PowerupPrefab;
    public GameObject PlayerPrefab;
    public GameObject PlayerBulletPrefab;
    public GameObject AlienBulletPrefab;
    public GameObject ExplosionParticlesPrefab;
    public GameObject AsteroidParticlesPrefab;

    private static SpawnerScript m_instance = null;
    public static SpawnerScript Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (SpawnerScript)FindObjectOfType(typeof(SpawnerScript));
            }
            return m_instance;
        }
    }

	void Start () 
    {
        m_asteroidSpawnPosition = new Vector3[m_asteroidRandomPositions];
        m_asteroidSpawnPosition[0] = new Vector3(0, 11, 0);
        m_asteroidSpawnPosition[1] = new Vector3(0, -11, 0);
        m_asteroidSpawnPosition[2] = new Vector3(22, 0, 0);
        m_asteroidSpawnPosition[3] = new Vector3(-22, 0, 0);

        m_alienSpawnPosition = new Vector3[m_alienRandomPositions];
        m_alienSpawnPosition[0] = new Vector3(-18, 4, 0);
        m_alienSpawnPosition[1] = new Vector3(18, -4, 0);

        m_asteroidsToSpawn = m_asteroidsStart;
        m_aliensToSpawn = m_aliensStart;

        ResetPowerup();

        m_isPlaying = false;
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
	
	void Update () 
    {
        if (m_isPlaying)
        {
            if (m_asteroidsToSpawn > 0 && m_asteroidSpawnTime <= 0)
            {
                int randomInt = Random.Range(0, m_asteroidRandomPositions);

                GameObject obj = Instantiate(AsteroidPrefab, m_asteroidSpawnPosition[randomInt], Quaternion.identity) as GameObject;
                obj.transform.localScale = new Vector3(Random.Range(90, 120), Random.Range(90, 120), Random.Range(90, 120));

                m_asteroidSpawnTime = m_asteroidSpawnCooldown;
                --m_asteroidsToSpawn;
            }

            if (m_aliensToSpawn > 0 && m_alienSpawnTime <= 0)
            {
                int randomInt = Random.Range(0, m_alienRandomPositions);

                Instantiate(AlienPrefab, m_alienSpawnPosition[randomInt], Quaternion.identity);

                m_alienSpawnTime = m_alienSpawnCooldown;
                --m_aliensToSpawn;
            }

            if (! m_powerupHasBeenSpawned )
            {
                m_powerupSpawnTime -= Time.deltaTime;
                if ( m_powerupSpawnTime <= 0)
                {
                    int randomInt = Random.Range(0, m_asteroidRandomPositions);

                    Instantiate(PowerupPrefab, m_asteroidSpawnPosition[randomInt], Quaternion.identity);

                    m_powerupHasBeenSpawned = true;
                }
            }

            m_asteroidSpawnTime -= Time.deltaTime;
            m_alienSpawnTime -= Time.deltaTime;
        }
	}

    public int AsteroidsToSpawnThisWave()
    {
        return m_asteroidsToSpawn;
    }

    public void ResetPowerup()
    {
        m_powerupSpawnTime = Random.Range(30.0f, 60.0f);
        m_powerupHasBeenSpawned = false;
    }

    public void RespawnPlayer( Vector3 lastPlayerPosition, Quaternion lastPlayerRotation )
    {
        // Allow the player to be able to collect the power up again
        int powerupCount = GameObject.FindGameObjectsWithTag("Powerup").Length;
        if (powerupCount == 0)
        {
            ResetPowerup();
        }

        GameObject playerObj = Instantiate(PlayerPrefab, lastPlayerPosition, lastPlayerRotation) as GameObject;
        Player player = playerObj.GetComponent<Player>();
        player.SetVulnerable(false);
    }

    public bool Playing
    {
        get { return this.m_isPlaying; }
        set { this.m_isPlaying = value; }
    }
}
