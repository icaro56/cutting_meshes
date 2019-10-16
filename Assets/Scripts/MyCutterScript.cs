using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCutterScript : MonoBehaviour
{
    public Material cutMaterial;
    public bool fillCut = true;
    public bool addRigidbody = true;

    private Camera myCamera;

    private Vector3 lastPoint;
    private Vector3 lastNormal;

    private Vector3 lastRayOrigin;
    private Vector3 lastRayDirection;

    private void Start()
    {
        myCamera = GetComponent<Camera>();

        if (myCamera == null)
        {
            myCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            lastRayOrigin = ray.origin;
            lastRayDirection = ray.direction;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Cuttable")
                {
                    float sqr = hit.collider.bounds.size.sqrMagnitude;
                    sqr = sqr / 2;

                    Vector3 direction = Vector3.Cross(-ray.direction, myCamera.transform.TransformDirection(Vector3.forward)).normalized * sqr;

                    lastPoint = hit.point;
                    lastNormal = direction;

                    Cutter.Cut(hit.collider.gameObject, lastPoint, lastNormal, cutMaterial, fillCut, addRigidbody);
                }
            }
        }

        DrawPlane(lastPoint, lastNormal);
        Debug.DrawRay(lastRayOrigin, lastRayDirection * 100.0f, Color.blue, 10.0f);
    }

    void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; 

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
        
        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }
}
