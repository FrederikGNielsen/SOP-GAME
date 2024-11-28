using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maxwell : MonoBehaviour
{
    NavMeshAgent _maxwell;
    
    Vector3 _destination;
    void Start()
    {
        _destination = new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5));
        _destination = transform.position + _destination;
        _maxwell = GetComponent<NavMeshAgent>();
        _maxwell.SetDestination(_destination);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, _destination) < 1)
        {
            _destination = new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5));
            _maxwell.SetDestination(_destination);
        }
    }
}
