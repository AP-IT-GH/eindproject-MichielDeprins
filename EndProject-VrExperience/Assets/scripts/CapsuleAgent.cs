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
        // Observe the enemy position relative to the agent's position
        Vector3 relativePosition = enemy.transform.position - transform.position;
        sensor.AddObservation(relativePosition.normalized);
        sensor.AddObservation(canThrow);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // PART II: Get the horizontal and vertical movement inputs from the agent (verander continuous action terug!)
        /*
        float horizontalInput = actionBuffers.ContinuousActions[0];
        float verticalInput = actionBuffers.ContinuousActions[1];
        float BallThrowing = actionBuffers.ContinuousActions[2];

        // Move the agent based on the input
        transform.Translate(new Vector3(horizontalInput, 0f, verticalInput) * Time.deltaTime * 5f);
        */
        //PART I: gooi de bal, draai de speler
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        // transform.Translate(controlSignal * 5);

        float BallThrowing = actionBuffers.ContinuousActions[2];
        transform.Rotate(0.0f, 2 * actionBuffers.ContinuousActions[1], 0.0f);
        //TODO: bool geven aan script voor enemy (checken collision met ball)
        //als bool true is reward geven + endepisode
        if (ballHitEnemy == true)
        {
            AddReward(+1);
            EndEpisode();
        }

        if (BallThrowing > 0)
        {
            Debug.Log("Ball thrown");
            this.Throw();
            //AddReward(-1f / MaxStep);
            AddReward(0.1f);
        }
        if (ball.GetComponent<ballScript>().getOutOfBounds() || ball.GetComponent<Transform>().position.y < 0.5f)
        {
            Debug.Log("ball out of bounds | episode ended");
            EndEpisode();
        }
    }



    //TESTING
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("keydown pressed");
            Throw();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, -rotationSpeed, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, rotationSpeed, 0);
        }

    }
    void Throw()
    {
        if (canThrow)
        {
            Debug.Log(this.tag + " is throwing");
            ball.GetComponent<ballScript>().ThrowBall(viewCamera.transform.position - transform.position, this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MidfieldBorder" || other.tag == "border")
        {
            Debug.Log("border touched");
            AddReward(-1);
            EndEpisode();
        }

    }
}

