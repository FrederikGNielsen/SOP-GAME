using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent _npc;
    Animator _animator;

    public Transform Destination1;
    public Transform Destination2;
    private bool walking1;
    
    public string Name;
    
    //Info
    public float Velocity;
    
    
    
    void Start()
    {
        _npc = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _npc.SetDestination(Destination1.position);
        StartCoroutine(WalkToDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Velocity = _npc.velocity.magnitude;
        _animator.SetFloat("speed", Velocity);
    }
    
    
    IEnumerator WalkToDestination()
    {
        if (walking1)
        {
            _npc.SetDestination(Destination1.position);
            if(Vector3.Distance(transform.position, Destination1.position) < 1)
            {
                walking1 = false;
            }
        }
        else
        {
            _npc.SetDestination(Destination2.position);
            if(Vector3.Distance(transform.position, Destination2.position) < 1)
            {
                walking1 = true;
            }
        }

        yield return new WaitForSeconds(3);
        StartCoroutine(WalkToDestination());
    }
}
