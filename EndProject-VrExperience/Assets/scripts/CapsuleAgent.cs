using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
public class CapsuleAgent : Agent
{
    public GameObject ball;
    public GameObject enemy;
    public GameObject viewCamera;
    public float rotationSpeed = 5f;
    public float movementSpeed = .2f;

    private bool ballHitEnemy = false;
    private bool canThrow = false;
    private GameObject player1;
    private GameObject player2;


    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("player1");
        player2 = GameObject.FindGameObjectWithTag("player2");
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

    public override void OnEpisodeBegin()
    {
        setBallHitEnemy(false);
        ball.GetComponent<ballScript>().setOutOfBounds(false);

        //tijdelijke if wanneer er geen throwball true is bij beide spelers
        player1.GetComponent<CapsuleAgent>().setCanThrow(true);

        Debug.Log("New Episode");
        ResetBall();
        GameObject.FindGameObjectWithTag("player1").GetComponent<Transform>().position = new Vector3(-1.55999994f, 0.600000024f, 0);
        GameObject.FindGameObjectWithTag("player2").GetComponent<Transform>().position = new Vector3(2.29999995f, 0.600000024f, 0);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(canThrow);
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


        if (ballHitEnemy == true)
        {
            this.AddReward(+1);
            EndEpisode();
        }

        if (ballThrowing == 1 && this.getcanThrow() && ball.GetComponent<ballScript>().getIsbeingThrown() == false)
        {
            this.Throw();
        }
        if (this.getcanThrow() && ball.GetComponent<ballScript>().getIsbeingThrown() == false)
        {
            if (this.tag == "player1")
            {
                ball.transform.position = GameObject.FindWithTag("ballSpawn1").GetComponent<Transform>().position;
            }
            else if (this.tag == "player2")
            {
                ball.transform.position = GameObject.FindWithTag("ballSpawn2").GetComponent<Transform>().position;
            }


        }
        if (ball.transform.position.y < -1)
        {
            ResetBall();
        }
        if (ball.GetComponent<ballScript>().getOutOfBounds())
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.GetComponent<ballScript>().setIsbeingThrown(false);
        ball.GetComponent<ballScript>().setOutOfBounds(false);

        if (player1.GetComponent<CapsuleAgent>().getcanThrow())
        {
            ball.transform.position = GameObject.FindWithTag("ballSpawn1").GetComponent<Transform>().position;
        }
        else if (player2.GetComponent<CapsuleAgent>().getcanThrow())
        {
            ball.transform.position = GameObject.FindWithTag("ballSpawn2").GetComponent<Transform>().position;
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
        if (this.getcanThrow() && ball.GetComponent<ballScript>().getIsbeingThrown() == false)
        {
            throwDirection = this.viewCamera.transform.position - this.transform.position;

            ball.GetComponent<ballScript>().ThrowBall(throwDirection);
            if (player1.GetComponent<CapsuleAgent>().getcanThrow() == true && player2.GetComponent<CapsuleAgent>().getcanThrow() == false)
            {
                player1.GetComponent<CapsuleAgent>().setCanThrow(false);
                player2.GetComponent<CapsuleAgent>().setCanThrow(true);
            }
            else if (player1.GetComponent<CapsuleAgent>().getcanThrow() == false && player2.GetComponent<CapsuleAgent>().getcanThrow() == true)
            {
                player1.GetComponent<CapsuleAgent>().setCanThrow(true);
                player2.GetComponent<CapsuleAgent>().setCanThrow(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MidfieldBorder" || other.tag == "border")
        {

            this.AddReward(-1f);
            EndEpisode();
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball" && this.getcanThrow() == false)
        {
            this.AddReward(-1f);
        }
    }
}

