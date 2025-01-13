using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public Vector3 moveOffset; //Where it moves the player when interacting with the door
    public GameObject connectedDoor;
    void Start()
    {
        if(connectedDoor == null)
            Debug.LogError("Door " + gameObject.name + " is not connected to any door!", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        //Move the player to the door
        PlayerController.Instance.MovePlayer(connectedDoor.transform.position + moveOffset);
    }

    private void OnDrawGizmosSelected()
    {
        if (connectedDoor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, connectedDoor.transform.position);
            Gizmos.DrawWireSphere(connectedDoor.transform.position + moveOffset, 0.5f);
        }
    }
}
