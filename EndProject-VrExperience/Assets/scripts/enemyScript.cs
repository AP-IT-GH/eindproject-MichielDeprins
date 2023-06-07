using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab;
    private float minX = 0.27f;
    private float maxX = 6.3f;
    private float minZ = -5.4f;
    private float maxZ = 7.2f;

    // Generate random position within the range



    void Start()
    {

    }
    public void spawnEnemy()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("player2");
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 randomPosition = new Vector3(randomX, -0.27f, randomZ);

        // Spawn the prefab at the random position
        if (enemy == null)
        {
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
    public void deleteEnemy()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("player2");
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
