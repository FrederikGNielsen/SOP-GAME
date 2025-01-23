using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactEvent;
    public UnityEvent selectEvent;
    public UnityEvent deselectEvent;
    
    public bool isInteractable = true;
    
    public bool HoldInteract = false;
    public float HoldTime = 1f;

    public void Interact()
    {
        interactEvent.Invoke();
    }
    
    public void Select()
    {
        selectEvent.Invoke();
    }
    
    public void Deselect()
    {
        deselectEvent.Invoke();
    }
}