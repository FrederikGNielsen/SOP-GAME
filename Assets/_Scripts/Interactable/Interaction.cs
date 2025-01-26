using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float InteractionDistance = 10;
    private GameObject selectedObject;
    private Transform _camera;
    private float holdTimer = 0f;
    private bool isHolding = false;
    
    public TextMeshProUGUI interactText;
    public Michsky.MUIP.ProgressBar progressBar;

    void Start()
    {
        _camera = Camera.main.transform;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, InteractionDistance))
        {
            if (hit.collider.CompareTag("NPC"))
            {
                Debug.Log("NPC");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DialogueSystem.Instance.Talk(hit.collider.GetComponent<NPC>().name);
                }
            }
            if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                GameObject obj = hit.collider.gameObject;

                if (selectedObject == null)
                {
                    obj.GetComponent<Interactable>().Select();
                    Debug.Log("Selected");
                    interactText.text = obj.GetComponent<Interactable>().interactText;
                }
                else if (selectedObject != obj)
                {
                    obj.GetComponent<Interactable>().Deselect();
                    Debug.Log("Deselected");
                    interactText.text = string.Empty;
                }
                selectedObject = obj;

                Interactable interactable = obj.GetComponent<Interactable>();
                if (interactable.HoldInteract)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        holdTimer += Time.deltaTime;
                        progressBar.currentPercent = (holdTimer / interactable.HoldTime) * 100;
                        if (holdTimer >= interactable.HoldTime)
                        {
                            interactable.Interact();
                            Debug.Log("Interacted with hold");
                            holdTimer = 0f;
                            progressBar.currentPercent = 0;
                        }
                    }
                    else
                    {
                        holdTimer = 0f;
                        progressBar.currentPercent = 0;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                        Debug.Log("Interacted");
                    }
                }
            }
            else
            {
                if (selectedObject != null)
                {
                    Deselect();
                }
            }
        }
        else
        {
            if (selectedObject != null)
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
        holdTimer = 0f;
        interactText.text = string.Empty;
    }
}