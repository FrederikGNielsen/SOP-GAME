using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public float Energy = 100;
    public float MaxEnergy = 100;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeEnergy(float amount)
    {
        Energy += amount;
        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
        }
        if (Energy < 0)
        {
            Energy = 0;
        }
    }
}
