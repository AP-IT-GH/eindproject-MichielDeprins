using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBall : MonoBehaviour
{

    [SerializeField] private GameLogic gameLogic;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(20,80,6);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < 70 ){
            this.transform.position = new Vector3(20,80,6);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "ExitTrigger"){
            Application.Quit();
        }
        
        if(other.gameObject.tag =="StartTrigger"){
            gameLogic.gameStarted = true;
        }
    }
}
