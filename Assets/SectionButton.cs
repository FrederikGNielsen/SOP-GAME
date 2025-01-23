using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionButton : MonoBehaviour
{
    public ProgressBar.Section section;
    public ProgressBar progressBar;
    
    public void ImproveSection()
    {
        ComputerDocument.instance.sectionToImprove = section;
        ComputerDocument.instance.isImproving = true;
    }
}
