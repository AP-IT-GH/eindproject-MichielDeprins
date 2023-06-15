using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] scoreScript ScoreScript;

    private bool agentVictory,playerVictory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ScoreScript.getAgentScore() > 2){
            agentVictory = true;
        } 
        else if(ScoreScript.getPlayerScore() > 2){
            playerVictory = true;
        }
    }
}
