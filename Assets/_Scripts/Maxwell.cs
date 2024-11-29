using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maxwell : MonoBehaviour
{
    NavMeshAgent _maxwell;
    
    Vector3 _destination;
    
    public Transform target;
    void Start()
    {
        _maxwell = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _maxwell.SetDestination(target.position);
    }
}
