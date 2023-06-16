using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{

    public float ballSpeed = 100f;
    [SerializeField] private GameObject player1;
    [SerializeField]  private GameObject player2;
    private bool outOfBounds = false;
    private bool isBeingThrown = false;
    public scoreScript score;

    // Start is called before the first frame update
    void Start()
    {
       
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
            // this.GetComponent<Rigidbody>().useGravity = false;

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
            Debug.Log("Ball on field 1");
            ResetBall();
        }
        else if (other.gameObject.tag == "player2Body")
        {
            score.updateAgentScore();
        }
        else if (other.gameObject.tag == "player1")
        {
            score.updatePlayerScore();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isBeingThrown = false;
        if (other.tag == "border")
        {

        }

    }

    private void ResetBall()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        setIsbeingThrown(false);
        setOutOfBounds(false);
        player1.GetComponent<CapsuleAgent>().setCanThrow(true);
    }
}
