using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] scoreScript ScoreScript;
    [SerializeField] public bool gameStarted,decisionMade,startOnce;
    [SerializeField]  GameObject player;

    [SerializeField] private GameObject enemy;

    [SerializeField] Transform startPosition, startedPosition;
    private bool agentVictory,playerVictory;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        decisionMade = false;
        player = GameObject.FindGameObjectWithTag("player2");
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && decisionMade == false && startOnce == false){
            
            startOnce = true;
        }else if(gameStarted && decisionMade == false){
            player.GetComponent<Transform>().position = startedPosition.position;
            decisionMade = true;
            enemy.SetActive(true);
        }
        if(ScoreScript.getAgentScore() > 2){
            agentVictory = true;
        } 
        else if(ScoreScript.getPlayerScore() > 2){
            playerVictory = true;
        }
    }
}
