using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreScript : MonoBehaviour
{
    private int playerScore;
    private int agentScore;
    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        agentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int getPlayerScore()
    {
        return playerScore;
    }
    public int getAgentScore()
    {
        return agentScore;
    }
    public void updatePlayerScore()
    {
        playerScore++;
    }
    public void updateAgentScore()
    {
        agentScore++;
    }
}

