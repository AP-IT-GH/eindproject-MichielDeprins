using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{

    public GameObject ballInstance;
    // Start is called before the first frame update
    [SerializeField] public List<GameObject> spawners = new List<GameObject>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerable SpawnBall()
    {
        while (true)
        {
            Instantiate(ballInstance, spawners[Random.Range(0, spawners.Count)].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(20f);
        }
    }
}
