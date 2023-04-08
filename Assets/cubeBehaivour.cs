using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeBehaivour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider Other)
    {
        if(Other.tag == "hand")
        {
            gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
