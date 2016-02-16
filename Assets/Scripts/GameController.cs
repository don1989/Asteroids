using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    // Assigned values
    public Text m_scoreText;
    public Text m_livesText;
    public Text m_startText;
    public Text m_gameOverText;

    private Vector3 m_lastPlayerTransform;
    private Quaternion m_lastPlayerRotation;

    public float m_deathCooldownSet = 2.0f;
    private float m_deathCooldown;
    private bool m_died;

    private GameState m_gameState;

    public AudioClip deathAudioClip;
    public AudioClip powerupAudioClip;
    public AudioClip shootAudioClip;
    public AudioClip explosionAudioClip;
    private AudioSource audioSource;

    // Singleton
    private static GameController m_instance = null;
    public static GameController Instance
    {
       get 
       {
           if (m_instance == null)
           {
               m_instance = (GameController)FindObjectOfType(typeof(GameController));
           }
           return m_instance;
       }
    }

	void Start () 
    {
        m_score = 0;
        m_lives = 3;

        m_died = false;
        m_lastPlayerTransform = new Vector3(0, 0, 0);
        m_lastPlayerRotation = new Quaternion();
        m_deathCooldown = 0.0f;

        m_gameState = GameState.MainMenu;

        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () 
    {
        switch (m_gameState)
        {
            case GameState.Playing:
                {
                    Playing();

                    break;
                }
            case GameState.MainMenu:
                {
                    MainMenu();

                    break;
                }
            case GameState.GameOver:
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        SceneManager.LoadScene(0);
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

    public void PlayDeathAudio()
    {
        audioSource.PlayOneShot(deathAudioClip);
    }
    public void PlayExplosionAudio()
    {
        audioSource.PlayOneShot(explosionAudioClip);
    }
    public void PlayShootingAudio()
    {
        audioSource.PlayOneShot(shootAudioClip);
    }
    public void PlayPowerupAudio()
    {
        audioSource.PlayOneShot(powerupAudioClip);
    }

    public void IncrementScore( int amount )
    {
        // Intentional truncation to see if we need to award a life after achieving a multiple of 1000
        int preVal = m_score / 1000;

        m_score += amount;

        int postVal = m_score / 1000;

        if (postVal > preVal)
            m_lives++;
        
    }

    public void DeductLife( Vector3 inTransform, Quaternion inRotation )
    {
        --m_lives;
        m_deathCooldown = m_deathCooldownSet;
        m_died = true;
        
        m_lastPlayerTransform.x = inTransform.x;
        m_lastPlayerTransform.y = inTransform.y;
        m_lastPlayerTransform.z = inTransform.z;

        m_lastPlayerRotation.x = inRotation.x;
        m_lastPlayerRotation.y = inRotation.y;
        m_lastPlayerRotation.z = inRotation.z;
        m_lastPlayerRotation.w = inRotation.w;
    }

    private void Playing()
    {
        m_scoreText.text = "Score: " + m_score;
        m_livesText.text = "Lives: " + m_lives;

        if (m_died)
        {
            if (m_lives > 0)
            {
                m_deathCooldown -= Time.deltaTime;

                if (m_deathCooldown <= 0)
                {
                    // Respawn
                    SpawnerScript.Instance.RespawnPlayer(m_lastPlayerTransform, m_lastPlayerRotation);
                    m_died = false;
                }
            }
            else
            {
                PlayDeathAudio();

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
            if (SpawnerScript.Instance.AsteroidsToSpawnThisWave() <= 0)
            {
                SpawnerScript.Instance.NextWave();
            }
        }
    }


    private void MainMenu()
    {
        m_scoreText.enabled = false;
        m_livesText.enabled = false;
        m_startText.enabled = true;
        m_gameOverText.enabled = false;

        if (Input.GetKey(KeyCode.Return))
        {
            SpawnerScript.Instance.Playing = true;
            m_gameState = GameState.Playing;

            m_scoreText.enabled = true;
            m_livesText.enabled = true;

            m_startText.enabled = false;
            m_gameOverText.enabled = false;            
        }
    }

    public bool IsPlaying()
    {
        return m_gameState == GameState.Playing;
    }
}
