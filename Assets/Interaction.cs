using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float InteractionDistance = 10;
    
    Transform _camera;
    
    
    void Start()
    {
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(_camera.position, _camera.forward, out hit, InteractionDistance))
        {
            if(hit.collider.CompareTag("NPC"))
            {
                Debug.Log("NPC");
            }
        }
    }
}
