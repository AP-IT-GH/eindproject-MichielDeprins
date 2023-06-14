using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{

    public float ballSpeed = 100f;
    private GameObject player1;
    private GameObject player2;
    private bool outOfBounds = false;
    private bool isBeingThrown = false;
    public enemyScript enemyScript;

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
            this.transform.position = new Vector3(-1.6f, 1f, 0);

        }
        if (isBeingThrown)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().useGravity = false;

        }
    }

    public void ThrowBall(Vector3 direction)
    {
        isBeingThrown = true;
        direction.y = 0f; // project onto the horizontal plane
        direction = direction.normalized;
        gameObject.GetComponent<Rigidbody>().velocity = direction * ballSpeed;


    }

    public bool getOutOfBounds()
    {
        return outOfBounds;
    }
    public void setOutOfBounds(bool change)
    {
        outOfBounds = change;
    }
    public bool getIsbeingThrown()
    {
        return isBeingThrown;
    }
    public void setIsbeingThrown(bool change)
    {
        isBeingThrown = change;
    }
    private void OnCollisionEnter(Collision other)
    {
        isBeingThrown = false;
        if (other.gameObject.tag == "field1")
        {

            setOutOfBounds(true);
        }
        else if (other.gameObject.tag == "field2")
        {
            setOutOfBounds(true);
        }

        else if (other.gameObject.tag == "player2")
        {
            Debug.Log("player 2 got hit");
            enemyScript.deleteEnemy();
            enemyScript.spawnEnemy();
            player1.GetComponent<CapsuleAgent>().setBallHitEnemy(true);
        }
        player1.GetComponent<CapsuleAgent>().setCanThrow(true);
    }

    void OnTriggerEnter(Collider other)
    {
        isBeingThrown = false;
        if (other.tag == "border")
        {
            setOutOfBounds(true);
            player1.GetComponent<CapsuleAgent>().setBallHitBorder(true);
            player1.GetComponent<CapsuleAgent>().setCanThrow(true);
        }

    }
}
