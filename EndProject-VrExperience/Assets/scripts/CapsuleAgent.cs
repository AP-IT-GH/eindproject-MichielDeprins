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
        // Reset the enemy position and ball instance
        Debug.Log("New Episode");
        GameObject.FindGameObjectWithTag("player1").GetComponent<Transform>().position = new Vector3(-1.55999994f, 0.600000024f, 0);
        GameObject.FindGameObjectWithTag("player2").GetComponent<Transform>().position = new Vector3(2.29999995f, 0.600000024f, 0);
        //PART II: let the target move
        /*
        enemyPosition = OpponentMove.GetRandomPosition();
        */
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

        float BallThrowing = actionBuffers.ContinuousActions[2];

        transform.Rotate(0.0f, 2 * actionBuffers.ContinuousActions[3], 0.0f);

        Debug.Log(this.tag + " can throw: " + this.getcanThrow());
        //ifs
        if (ballHitEnemy == true)
        {
            this.AddReward(+1);
            EndEpisode();
        }

        if (BallThrowing > 0 && this.getcanThrow())
        {
            Debug.Log("Ball thrown by " + this.tag);
            this.Throw();
        }
        if (this.getcanThrow())
        {
            if (this.tag == "player1")
            {
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.transform.position = GameObject.FindWithTag("ballSpawn1").GetComponent<Transform>().position;
            }
            if (this.tag == "player2")
            {
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.transform.position = GameObject.FindWithTag("ballSpawn2").GetComponent<Transform>().position;
            }


        }
        if (ball.transform.position.y < -1)
        {
            ResetBall();
            EndEpisode();
        }
        if (ball.GetComponent<ballScript>().getOutOfBounds())
        {
            ResetBall();
            EndEpisode();
        }
    }

    private void ResetBall()
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        if (this.getcanThrow())
        {
            if (this.tag == "player1")
            {
                ball.transform.position = GameObject.FindWithTag("ballSpawn1").GetComponent<Transform>().position;
            }
            if (this.tag == "player2")
            {
                ball.transform.position = GameObject.FindWithTag("ballSpawn2").GetComponent<Transform>().position;
            }
            else
            {
                ball.transform.position = new Vector3(-1.6f, 1f, 0);
            }
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    //TESTING
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical"); // Forwards/Backwards movement
        continuousActionsOut[1] = Input.GetAxis("Horizontal"); // Turning
        continuousActionsOut[2] = Input.GetKey(KeyCode.F) ? 1f : 0f; // Ball throwing
        continuousActionsOut[3] = Input.GetAxis("rotating");
    }

    void Throw()
    {
        if (this.getcanThrow())
        {
            Debug.Log("inside if getCanthrow");
            ball.GetComponent<Rigidbody>().useGravity = true;
            ball.GetComponent<ballScript>().ThrowBall(viewCamera.transform.position - transform.position, this.gameObject);
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
        if (other.gameObject.tag == "Ball")
        {
            this.AddReward(-1f);
            Debug.Log(this.tag + " actually got hit");
        }
    }
}

