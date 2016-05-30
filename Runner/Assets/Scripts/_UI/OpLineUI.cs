using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OpLineUI : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float width = 0.001f;
    public float threshold = 0.001f;
    Camera thisCamera;
    private bool dragging = false;

    public Vector3 startPos;
    public Vector3 endPos;

    void Start()
    {
        thisCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetWidth(0, 0);
        lineRenderer.SetVertexCount(2);
    }

    void Update()
    {
        if(dragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                lineRenderer.SetWidth(0, 0);
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = thisCamera.nearClipPlane;
                endPos = thisCamera.ScreenToWorldPoint(mousePos);

                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos);
                lineRenderer.SetWidth(width, width);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = thisCamera.nearClipPlane;
            startPos = thisCamera.ScreenToWorldPoint(mousePos);
        }
    }

}