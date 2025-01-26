using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : MonoBehaviour
{
    public float energy = 10;
    float tireness = 5;
    
    public void Consume()
    {
        PlayerStats.instance.ChangeEnergy(energy);
        Destroy(gameObject);
    }
}
