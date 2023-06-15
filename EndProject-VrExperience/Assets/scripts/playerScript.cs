using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject ball;
    public GameObject viewCamera;
    public float rotationSpeed = 5f;
    public float movementSpeed = .2f;

    private bool ballHitEnemy = false;
    private bool ballHitWall = false;
    private bool canThrow = false;
    private GameObject player1;
    private Vector3 enemyPosition;


    public List<Transform> borders = new List<Transform>();


    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("player1");

    }

    public void setBallHitBorder(bool change)
    {
        ballHitWall = change;
    }


    public void setBallHitEnemy(bool change)
    {
        ballHitEnemy = change;
    }
    public void setCanThrow(bool change)
    {
        canThrow = change;
    }
    public bool getcanThrow()
    {
        return canThrow;
    }













    Vector3 throwDirection;
    void Throw()
    {
        Debug.Log($"{this.gameObject.tag} has thrown the ball");
        throwDirection = this.viewCamera.transform.position - this.transform.position;
        ball.GetComponent<ballScript>().setIsbeingThrown(true);
        ball.GetComponent<ballScript>().ThrowBall(throwDirection);

        player1.GetComponent<CapsuleAgent>().setCanThrow(false);
    }




    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("ball hit enemy");
        }
    }
}
