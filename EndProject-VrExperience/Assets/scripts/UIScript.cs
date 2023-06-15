using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIScript : MonoBehaviour
{

    [SerializeField] scoreScript ScoreScript;
    [SerializeField] TextMeshProUGUI agentScoreUI, playerScoreUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agentScoreUI.text = ScoreScript.getAgentScore().ToString();
        playerScoreUI.text = ScoreScript.getPlayerScore().ToString();
    }
}
