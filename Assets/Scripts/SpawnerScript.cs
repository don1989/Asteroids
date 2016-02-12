using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    private int m_asteroidsToSpawn = 5;
    private int m_aliensToSpawn = 1;

    private float m_asteroidSpawnTime = 0.0f;
    private float m_alienSpawnTime = 10.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_asteroidsToSpawn > 0 && m_asteroidSpawnTime <= 0 )
        {
            GameObject obj = Instantiate(Resources.Load("AsteroidPrefab"), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            obj.transform.localScale = new Vector3(80, 90, 100);

            m_asteroidSpawnTime = 6;
            --m_asteroidsToSpawn;
        }
/*
        if (m_aliensToSpawn > 0 && m_alienSpawnTime <= 0)
        {
            GameObject obj = Instantiate(Resources.Load("AsteroidPrefab"), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            obj.transform.localScale = new Vector3(80, 90, 100);

            m_asteroidSpawnTime = 6;
            --m_asteroidsToSpawn;
        }
*/
        m_asteroidSpawnTime -= Time.deltaTime;
        m_alienSpawnTime -= Time.deltaTime;
	}
}
