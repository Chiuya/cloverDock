using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed;
    private float camWidth;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        initialPos = new Vector3(-camWidth, transform.position.y, transform.position.z);
        transform.position = initialPos;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if(transform.position.x > camWidth) {
            transform.position = initialPos;
        }
        
    }
}
