using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float InteractionDistance = 10;
    private GameObject selectedObject;
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
            if(hit.collider.gameObject.GetComponent<Interactable>())
            {
                GameObject obj = hit.collider.gameObject;

                if (selectedObject == null)
                {
                    obj.GetComponent<Interactable>().Select();
                    Debug.Log("Selected");
                } else if(selectedObject != obj)
                {
                    obj.GetComponent<Interactable>().Deselect();
                    Debug.Log("Deselected");
                }
                selectedObject = obj;

                
                if(Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.gameObject.GetComponent<Interactable>().Interact();
                    Debug.Log("Interacted");
                }
            }
            else
            {
                if(selectedObject != null)
                {
                    Deselect();
                }
            }
        } else
        {
            if(selectedObject != null)
            {
                Deselect();
            }
        }
    }

    private void Deselect()
    {
        selectedObject.GetComponent<Interactable>().Deselect();
        Debug.Log("Deselected");
        selectedObject = null;
    }
}
