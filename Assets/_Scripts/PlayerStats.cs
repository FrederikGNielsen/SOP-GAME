using UnityEngine;
using Michsky.MUIP;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    
    public float energy = 100;
    public float maxEnergy = 100;
    
    public float tireness = 0;
    
    public Michsky.MUIP.ProgressBar energySlider;
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeEnergy(float amount)
    {
        energy += amount;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        if (energy < 0)
        {
            energy = 0;
        }
        energySlider.SetValue(energy);
    }
}
