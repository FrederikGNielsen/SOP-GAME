using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    public GameObject bedUI;
    public TextMeshProUGUI timeText;
    public Slider timeSlider;


    private void Start()
    {
        timeText.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationMono-Regular SDF"); // Use a TMP_FontAsset
    }

    public void Interact()
    {
        bedUI.SetActive(bedUI.activeSelf ? false : true);
        GameManager.Instance.OpenCloseBed(bedUI.activeSelf);

    }

    public void UpdateSlider()
    {
        float value = timeSlider.value * 60;
        int hours = Mathf.FloorToInt(value / 60);
        int minutes = Mathf.FloorToInt(value % 60);
        int seconds = Mathf.FloorToInt((value * 60) % 60);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        timeText.text = formattedTime;
    }

    public void Sleep()
    {
        if (timeSlider == null || timeText == null)
        {
            Debug.LogWarning("timeSlider or timeText is not assigned.");
            return;
        }
        StartCoroutine(SleepRoutine());
    }
    
    
    IEnumerator SleepRoutine()
    {
        yield return new WaitForSeconds(1/6f);
        while (timeSlider.value > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                timeSlider.value -= 1f / 6f;
                UpdateSlider();
                GameManager.Instance.ChangeTime(10);
                PlayerStats.instance.ChangeEnergy(10f / 6f);
                yield return new WaitForSeconds(1f / 6f);
            }
        }
        
        GameManager.Instance.OpenCloseBed(false);
    }
}
