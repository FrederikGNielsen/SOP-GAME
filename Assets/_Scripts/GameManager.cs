using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public PlayerCameraController playerCameraController;
    //Time
    [HideInInspector]public float timeScale = 1f;
    public int days = 0;
    public float timeOfDay = 0f;
    public float dayLength = 60f;
    [HideInInspector] public float timeMultiplier = 1f;
    public GameObject sun;
    [HideInInspector] public string timeString;
    [HideInInspector] public string dateString;
    public float sunriseOffset;
    
    //states
    bool OpenComputer = false;
    bool OpenBed = false;
    
    //objects
    public GameObject computerUI;
    public GameObject bedUI;
    
    private void Awake()
    {
        Instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        TimeUpdate();
        DisableMovement();
    }

    public void DisableMovement()
    {
        if (OpenComputer)
        {
            playerCameraController.mouseIsLocked = false;
        }
        else if (OpenBed)
        {
            playerCameraController.mouseIsLocked = false;
        }
        else
        {
            playerCameraController.mouseIsLocked = true;
        }
    }
    
    public void OpenCloseComputer()
    {
        OpenComputer = !OpenComputer;
        computerUI.SetActive(OpenComputer);
    }
    
    public void OpenCloseBed(bool state)
    {
        OpenBed = state;
        bedUI.gameObject.SetActive(OpenBed);
    }

    public void TimeUpdate()
    {
        timeOfDay += Time.deltaTime * timeScale * timeMultiplier;
        sun.transform.rotation = Quaternion.Euler(new Vector3((timeOfDay / dayLength * 360f) + sunriseOffset, 0, 0));

        int hours = Mathf.FloorToInt((timeOfDay / dayLength) * 24);
        int minutes = Mathf.FloorToInt(((timeOfDay / dayLength) * 24 * 60) % 60);
        timeString = string.Format("{0:00}:{1:00}", hours, minutes);

        dateString = days + 18 + "/11/2024";

        if (timeOfDay >= dayLength)
        {
            timeOfDay = 0;
            days++;
        }
    }
    
    public void ChangeTime(float time)
    {
        timeOfDay += time * 60; // Convert minutes to seconds
        if (timeOfDay >= dayLength)
        {
            timeOfDay -= dayLength;
            days++;
        }
        sun.transform.rotation = Quaternion.Euler(new Vector3((timeOfDay + sunriseOffset) / dayLength * 360f, 0, 0));
    }
    
    public void ChangeTimeScale(float scale)
    {
        timeScale = scale;
    }
    
    public void ChangeTimeMultiplier(float multiplier)
    {
        timeMultiplier = multiplier;
    }
}
