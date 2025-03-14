using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSetWidth : MonoBehaviour
{
    public float speed;
    public float maxX;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = new Vector3(-maxX, transform.position.y, transform.position.z);
        transform.position = initialPos;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if(transform.position.x > maxX) {
            transform.position = initialPos;
        }
        
    }
}
