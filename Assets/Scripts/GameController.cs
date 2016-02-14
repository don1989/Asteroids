using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    };


    private int m_score;
    private int m_lives;

    public Text m_scoreText;
    public Text m_livesText;
    public Text m_startText;
    public Text m_gameOverText;

    public Vector3 m_lastPlayerTransform;
    public Quaternion m_lastPlayerRotation;

    private float m_deathCooldown;
    private bool m_died;

    private GameState m_gameState = GameState.MainMenu;
    SpawnerScript m_spawnerScript;

	void Start () 
    {
        m_score = 0;
        m_lives = 3;

        m_died = false;
        m_lastPlayerTransform = new Vector3(0, 0, 0);
        m_lastPlayerRotation = new Quaternion();
        m_deathCooldown = 0.0f;

        m_spawnerScript = GetComponent<SpawnerScript>();
	}
	
	void Update () 
    {
        switch (m_gameState)
        {
            case GameState.Playing:
                {
                    m_scoreText.text = "Score: " + m_score;
                    m_livesText.text = "Lives: " + m_lives;

                    if ( m_died )
                    {
                        if ( m_lives > 0 )
                        {
                            m_deathCooldown -= Time.deltaTime;

                            if (m_deathCooldown <= 0)
                            {
                                // Respawn
                                GameObject playerObj = Instantiate(Resources.Load("Prefabs/Player"), m_lastPlayerTransform, m_lastPlayerRotation) as GameObject;
                                Player player = playerObj.GetComponent<Player>();
                                player.SetVulnerable(false);

                                m_died = false;
                            }
                        }
                        else
                        {
                            // Game over
                            m_gameState = GameState.GameOver;

                            m_scoreText.enabled = true;
                            m_livesText.enabled = false;
                            m_startText.enabled = false;
                            m_gameOverText.enabled = true;
                        }
                    }

                    int asteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;
                    if (asteroidCount == 0)
                    {
                        if (m_spawnerScript.AsteroidsToSpawnThisWave() <= 0)
                        {
                            m_spawnerScript.NextWave();
                        }
                    }

                    break;
                }
            case GameState.MainMenu:
                {
                    m_scoreText.enabled = false;
                    m_livesText.enabled = false;
                    m_startText.enabled = true;
                    m_gameOverText.enabled = false;

                    if ( Input.GetKey(KeyCode.Return) )
                    {
                        m_spawnerScript.Playing = true;
                        m_gameState = GameState.Playing;

                        m_scoreText.enabled = true;
                        m_livesText.enabled = true;

                        m_startText.enabled = false;
                        m_gameOverText.enabled = false;
                    }

                    break;
                }
            case GameState.GameOver:
                {

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Application.LoadLevel(0);
                    }

                    break;
                }
            default: break;
        }
        

        if ( Input.GetKey(KeyCode.Escape) )
        {
            Application.Quit();
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
