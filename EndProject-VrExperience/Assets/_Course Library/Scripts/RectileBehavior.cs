using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectileBehavior : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateSelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator RotateSelf(){

        while(true){
            yield return new WaitForSeconds(0.01f);
            this.transform.rotation *= Quaternion.Euler(0f,1f,0f);
        }

    }
}