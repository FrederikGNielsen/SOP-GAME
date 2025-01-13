using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerClock : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI DateText;

    private void Start()
    {
        TimeText = GetComponent<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        TimeText.text = GameManager.Instance.timeString;
        DateText.text = GameManager.Instance.dateString;
    }
}
