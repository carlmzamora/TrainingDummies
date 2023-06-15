using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject dummyPrefab;

    [SerializeField] private BoxCollider groundCollider;
    private float groundExtentsX;
    private float groundExtentsZ;
    Vector3 rayOrigin;
    
    void Start()
    {        
        groundExtentsX = groundCollider.bounds.extents.x;
        groundExtentsZ = groundCollider.bounds.extents.z;

        InvokeRepeating("CalculateSpawnpoint", 1, 1f);

        

        //check if outside camera view
    }

    // Update is called once per frame
    void Update()
    {        
    }
    
    //spawn within ground area but outside camera
    void CalculateSpawnpoint()
    {
        Vector3 spawnPt = Vector3.zero;
        bool validPoint = false;
        int counter = 0;
        
        
        RaycastHit hit = new RaycastHit();

        while(!validPoint && counter < 100)
        {
            float rayOriginX = Random.Range(-groundExtentsX, groundExtentsX);
            float rayOriginZ = Random.Range(-groundExtentsZ, groundExtentsZ);

            rayOrigin = new Vector3(rayOriginX, 100, rayOriginZ);
            Ray groundRay = new Ray(rayOrigin, Vector3.down);            

            if(Physics.Raycast(groundRay, out hit))
            {
                if(hit.collider.gameObject.CompareTag("Ground"))
                {
                    spawnPt = hit.point;
                    validPoint = IsOutsideCamera(spawnPt);
                }
            }
            counter++;
        }
        Instantiate(dummyPrefab, new Vector3(spawnPt.x, dummyPrefab.transform.position.y, spawnPt.z), dummyPrefab.transform.rotation);
    }

    bool IsOutsideCamera(Vector3 hitPoint)
    {
        Vector3 viewportCoords00 = Vector3.zero;
        Vector3 viewportCoords01 = Vector3.zero;
        Vector3 viewportCoords11 = Vector3.zero;
        Vector3 viewportCoords10 = Vector3.zero;

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, -0.65f, 0));

        //calculate 00
        Vector3 rayDirection = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        float rayLength;

        Ray ray = new Ray(Camera.main.transform.position, rayDirection);
        if(groundPlane.Raycast(ray, out rayLength))
        {
            viewportCoords00 = ray.GetPoint(rayLength);
        }

        //calculate 01
        rayDirection = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.farClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));

        ray = new Ray(Camera.main.transform.position, rayDirection);
        if(groundPlane.Raycast(ray, out rayLength))
        {
            viewportCoords01 = ray.GetPoint(rayLength);
        }

        //calculate 11
        rayDirection = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.farClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        ray = new Ray(Camera.main.transform.position, rayDirection);
        if(groundPlane.Raycast(ray, out rayLength))
        {
            viewportCoords11 = ray.GetPoint(rayLength);
        }

        //calculate 10
        rayDirection = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.farClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));

        ray = new Ray(Camera.main.transform.position, rayDirection);
        if(groundPlane.Raycast(ray, out rayLength))
        {
            viewportCoords10 = ray.GetPoint(rayLength);
        }

        /*Debug.DrawLine(new Vector3(viewportCoords00.x, -0.55f, viewportCoords00.z), new Vector3(viewportCoords01.x, -0.55f, viewportCoords01.z), Color.blue);
        Debug.DrawLine(new Vector3(viewportCoords01.x, -0.55f, viewportCoords01.z), new Vector3(viewportCoords11.x, -0.55f, viewportCoords11.z), Color.blue);
        Debug.DrawLine(new Vector3(viewportCoords11.x, -0.55f, viewportCoords11.z), new Vector3(viewportCoords10.x, -0.55f, viewportCoords10.z), Color.blue);
        Debug.DrawLine(new Vector3(viewportCoords10.x, -0.55f, viewportCoords10.z), new Vector3(viewportCoords00.x, -0.55f, viewportCoords00.z), Color.blue);*/

        if((hitPoint.x < viewportCoords01.x || hitPoint.x > viewportCoords11.x) ||
        (hitPoint.z < viewportCoords00.z || hitPoint.z > viewportCoords11.z))
        {
            return true;
        }

        return false;
    }
}
