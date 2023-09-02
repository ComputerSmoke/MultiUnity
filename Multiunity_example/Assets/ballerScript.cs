using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.transform.position.y < -5) 
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, 0);
    }
}
