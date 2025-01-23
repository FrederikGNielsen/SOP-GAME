using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public GameObject computerUI; 
    
    
    public void OpenComputer()
    {
        computerUI.SetActive(true);
    }
    
    public void CloseComputer()
    {
        computerUI.SetActive(false);
    }
}
