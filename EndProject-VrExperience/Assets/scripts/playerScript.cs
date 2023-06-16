using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject agent;
    
       public GameObject ball;

    private bool ballHitEnemy = false;
    private bool ballHitWall = false;
    private bool canThrow = false;
    private GameObject player1;
    
        // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        if(agent.GetComponent<CapsuleAgent>().getcanThrow()){
            this.canThrow = false;
        }else{
            this.canThrow = true;
        }
    }
 


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


}
