using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{

    public float ballSpeed = 10f;
    private GameObject player1;
    private GameObject player2;
    private bool outOfBounds = false;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindWithTag("player1");
        player2 = GameObject.FindWithTag("player2");


    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -1)
        {
            outOfBounds = true;
        }
    }

    public void ThrowBall(Vector3 direction, GameObject thrower)
    {
        direction.y = 0f; // project onto the horizontal plane
        direction = direction.normalized;
        if (thrower.tag == "player1")
        {
            this.transform.position = GameObject.FindGameObjectWithTag("ballSpawn1").GetComponent<Transform>().position;
        }
        if (thrower.tag == "player2")
        {
            this.transform.position = GameObject.FindGameObjectWithTag("ballSpawn2").GetComponent<Transform>().position;

        }
        gameObject.GetComponent<Rigidbody>().velocity = direction * ballSpeed;
        player1.GetComponent<CapsuleAgent>().setCanThrow(false);
    }

    public bool getOutOfBounds()
    {
        return outOfBounds;
    }
    public void setOutOfBounds(bool change)
    {
        outOfBounds = change;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "field1")
        {
            Debug.Log("field 1");
            player1.GetComponent<CapsuleAgent>().setCanThrow(true);
            setOutOfBounds(true);
        }
        if (other.gameObject.tag == "field2")
        {
            Debug.Log("field 2");
            player1.GetComponent<CapsuleAgent>().setCanThrow(true);
            setOutOfBounds(true);
        }
        if (other.gameObject.tag == "player1")
        {
            Debug.Log("player 1 got hit");
            player2.GetComponent<CapsuleAgent>().setBallHitEnemy(true);
            player1.GetComponent<CapsuleAgent>().setCanThrow(true);
            player2.GetComponent<CapsuleAgent>().setCanThrow(false);
        }
        if (other.gameObject.tag == "player2")
        {
            Debug.Log("player 2 got hit");
            player1.GetComponent<CapsuleAgent>().setBallHitEnemy(true);
            player2.GetComponent<CapsuleAgent>().setCanThrow(true);
            player1.GetComponent<CapsuleAgent>().setCanThrow(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "border")
        {
            outOfBounds = true;
            if (player1.GetComponent<CapsuleAgent>().getcanThrow())
            {
                player2.GetComponent<CapsuleAgent>().setCanThrow(true);
                player1.GetComponent<CapsuleAgent>().setCanThrow(false);
            }
            if (player2.GetComponent<CapsuleAgent>().getcanThrow())
            {
                player1.GetComponent<CapsuleAgent>().setCanThrow(true);
                player2.GetComponent<CapsuleAgent>().setCanThrow(false);
            }
        }
    }
}
