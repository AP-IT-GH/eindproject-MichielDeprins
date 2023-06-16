using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CapsuleAgent : Agent
{
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

    public void setCanThrow(bool change)
    {
        canThrow = change;
    }
    public bool getcanThrow()
    {
        return canThrow;
    }

    public override void OnEpisodeBegin()
    {
        player1.GetComponent<CapsuleAgent>().setCanThrow(true);

        Debug.Log("New Episode");
        //player stop moving
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("player1").GetComponent<Transform>().position = new Vector3(-1.55999994f, -0.27f, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(canThrow);
        sensor.AddObservation(this.ballHitEnemy);
        sensor.AddObservation(this.ballHitWall);
        sensor.AddObservation(enemyPosition);
        foreach (Transform b in borders)
        {
            sensor.AddObservation(b.position);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        // move player
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * movementSpeed);

        controlSignal.z = actionBuffers.ContinuousActions[1];
        transform.Translate(controlSignal * movementSpeed);

        //float BallThrowing = actionBuffers.ContinuousActions[2];
        float ballThrowing = actionBuffers.DiscreteActions[0];

        transform.Rotate(0.0f, 2 * actionBuffers.ContinuousActions[2], 0.0f);
        if (GameObject.FindGameObjectWithTag("player2") != null)
        {
            enemyPosition = GameObject.FindGameObjectWithTag("player2").GetComponent<Transform>().position;
        }

        if (this.getcanThrow() && ball.GetComponent<ballScript>().getIsbeingThrown() == false)
        {
            ball.transform.position = GameObject.FindWithTag("ballSpawn1").GetComponent<Transform>().position;
            ball.GetComponent<Rigidbody>().useGravity = false;
            if (ballThrowing == 1)
            {
                this.Throw();
            }
        }else{
            ball.GetComponent<Rigidbody>().useGravity=true;
        }

    }


    //TESTING
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical"); // Forwards/Backwards movement
        continuousActionsOut[1] = Input.GetAxis("Horizontal"); // Turning
        continuousActionsOut[2] = Input.GetAxis("rotating");

        discreteActionsOut[0] = Input.GetKey(KeyCode.F) ? 1 : 0; // Ball throwing
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
        if (other.gameObject.tag == "Ball" && other.gameObject.tag == "player2")
        {
            Debug.Log(this.gameObject.tag + " got hit by ball");
        }
    }
}

