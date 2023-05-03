using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CapsuleAgent : Agent
{
    public GameObject ballPrefab;
    public GameObject enemy;
    public GameObject viewCamera;
    public float ballSpeed = 100f;
    public float rotationSpeed = 5f;

    private Vector3 enemyPosition;
    private GameObject ballInstance;

    public override void OnEpisodeBegin()
    {
        // Reset the enemy position and ball instance
        enemyPosition = enemy.transform.position;
        Destroy(ballInstance);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the enemy position relative to the agent's position
        Vector3 relativePosition = enemy.transform.position - transform.position;
        sensor.AddObservation(relativePosition.normalized);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Get the horizontal and vertical movement inputs from the agent
        float horizontalInput = actionBuffers.ContinuousActions[0];
        float verticalInput = actionBuffers.ContinuousActions[1];

        // Move the agent based on the input
        transform.Translate(new Vector3(horizontalInput, 0f, verticalInput) * Time.deltaTime * 5f);


        //TODO: bool geven aan script voor enemy (checken collision met ball)
        //als bool true is reward geven + endepisode
        // if ()
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("keydown pressed");
            ThrowBall();
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
    void ThrowBall()
    {
        Debug.Log("Method Throwball called");
        // Destroy the existing ball instance, if any
        if (ballInstance != null)
        {
            Destroy(ballInstance);
        }

        // Instantiate a new ball instance and throw it at the enemy's position  
        ballInstance = Instantiate(ballPrefab, new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z), Quaternion.identity);

        Vector3 direction = viewCamera.transform.position - transform.position;
        direction.y = 0f; // project onto the horizontal plane
        direction = direction.normalized;
        ballInstance.GetComponent<Rigidbody>().velocity = direction * ballSpeed;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemyField")
        {
            SetReward(-1);
            EndEpisode();
        }
    }
}

