using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBall : MonoBehaviour
{

    [SerializeField] private GameLogic gameLogic;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(-3,101,1);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < 70 ){
            this.transform.position = new Vector3(-3,101,1);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "ExitTrigger"){
            Application.Quit();
        }
        
        if(other.gameObject.tag =="PlayTrigger"){
            gameLogic.gameStarted = true;
            //GameObject.FindGameObjectWithTag("player1").SetActive(true);
        }
    }
}
