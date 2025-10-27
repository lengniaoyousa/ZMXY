using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Vector2 minVector = Vector2.zero;
    private Vector2 maxVector = Vector2.zero;
    
    private SunController sunController;


    public void Update()
    {
        if (sunController!=null)
        {
            transform.position = sunController.transform.position;
            
            if (transform.position.x < minVector.x)
            {
                transform.position = new Vector3(minVector.x, transform.position.y, transform.position.z);
            }
            if (transform.position.y < minVector.y)
            {
                transform.position = new Vector3(transform.position.x, minVector.y, transform.position.z);
            }
        
            if (transform.position.x > maxVector.x)
            {
                transform.position = new Vector3(maxVector.x, transform.position.y, transform.position.z);
            }
            if (transform.position.y > maxVector.y)
            {
                transform.position = new Vector3(transform.position.x, maxVector.y, transform.position.z);
            }
        }
    }

    public void SetSunController(SunController sunController)
    {
        this.sunController = sunController;
    }

    public void SetCameraOffsetValue(Vector2 minVector, Vector2 maxVector)
    {
        this.minVector = minVector;
        this.maxVector = maxVector;
    }
}
