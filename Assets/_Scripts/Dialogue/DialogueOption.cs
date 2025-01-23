using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    public int OptionIndex;
    void Start()
    {
        OptionIndex = transform.GetSiblingIndex();
    }
    
    public void ChooseOption()
    {
        DialogueSystem.Instance.SelectOption(OptionIndex);
    }
}
