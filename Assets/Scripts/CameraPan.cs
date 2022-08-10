using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 touchStart = Vector3.zero;
    public float xMin, xMax;
    private Vector3 targetPos;
    private float groundZ = 0;

    void Awake()
    {
        targetPos = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.GetMouseButton(0))
        {
            targetPos.x = Mathf.Clamp(mainCamera.transform.position.x + (touchStart.x - GetWorldPosition(groundZ).x), xMin, xMax);
            mainCamera.transform.position = targetPos;
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
