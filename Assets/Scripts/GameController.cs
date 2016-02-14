using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    private int m_score;
    private int m_lives;

    public Text m_scoreText;
    public Text m_livesText;

    public Vector3 m_lastPlayerTransform;
    public Quaternion m_lastPlayerRotation;

    private float m_deathCooldown;
    private bool m_died;


	void Start () 
    {
        m_score = 0;
        m_lives = 3;

        m_died = false;
        m_lastPlayerTransform = new Vector3(0, 0, 0);
        m_lastPlayerRotation = new Quaternion();
        m_deathCooldown = 0.0f;
	}
	
	void Update () 
    {
        m_scoreText.text = "Score: " + m_score;
        m_livesText.text = "Lives: " + m_lives;
   
        if (m_died && m_lives > 0)
        {
            m_deathCooldown -= Time.deltaTime;

            if (m_deathCooldown <=0)
            {
                // Respawn
                GameObject playerObj = Instantiate(Resources.Load("Prefabs/Player"), m_lastPlayerTransform, m_lastPlayerRotation) as GameObject;
                Player player = playerObj.GetComponent<Player>();
                player.SetVulnerable( false );

                m_died = false;
            }
        }

        int asteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;
        if (asteroidCount == 0)
        {
            GameObject spawner = GameObject.Find("Spawner");
            SpawnerScript spawnerScript = spawner.GetComponent<SpawnerScript>();
            
            if ( spawnerScript && spawnerScript.AsteroidsToSpawnThisWave() <= 0 )
            {
                spawnerScript.NextWave();
            }
        }

	}

    public void IncrementScore( int amount )
    {
        m_score += amount;
    }

    public void DeductLife( Vector3 inTransform, Quaternion inRotation )
    {
        --m_lives;
        m_deathCooldown = 2.0f;
        m_died = true;
        

        m_lastPlayerTransform.x = inTransform.x;
        m_lastPlayerTransform.y = inTransform.y;
        m_lastPlayerTransform.z = inTransform.z;

        m_lastPlayerRotation.x = inRotation.x;
        m_lastPlayerRotation.y = inRotation.y;
        m_lastPlayerRotation.z = inRotation.z;
        m_lastPlayerRotation.w = inRotation.w;
    }
}
