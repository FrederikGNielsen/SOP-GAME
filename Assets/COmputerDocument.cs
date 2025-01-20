using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class COmputerDocument : MonoBehaviour
{
    public static COmputerDocument instance;
    public WordWriting wordWriting;
    public ProgressBar progressBar;

    public float TimeSpending = 0; // how much time the player wants to spend
    public float TimeSpent = 0; // how much time the player has spent in this study session

    public float EnergySpending = 0; // how much energy the player wants to spend in this study session

    public float productivity = 0; // how productive the player is in this study session (words per min)
    public float approximateWords = 0; // how many words the player will be able to write in this study session

    public Slider timeSlider;
    public float maxTime = 100;

    // GameObjects
    public GameObject blocker;
    public TextMeshProUGUI timeSliderText;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        maxTime = PlayerStats.Instance.Energy / 0.5f;
        timeSlider.maxValue = maxTime;
        TimeSpending = timeSlider.value;
        EnergySpending = TimeSpending * 0.5f;
        approximateWords = EnergySpending * productivity; // 1 min is 100 words

        timeSliderText.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationMono-Regular SDF"); // Use a TMP_FontAsset

        int hours = Mathf.FloorToInt(TimeSpending / 60);
        int minutes = Mathf.FloorToInt(TimeSpending % 60);
        int seconds = Mathf.FloorToInt((TimeSpending * 60) % 60);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        timeSliderText.text = formattedTime + " - Energy Spent: " + EnergySpending.ToString("F1").PadLeft(5, '0') + "/" + PlayerStats.Instance.Energy.ToString("F1").PadLeft(5, '0') + " - Words: " + approximateWords.ToString("F1").PadLeft(5, '0');

        if (Input.GetKeyDown("space"))
        {
            Study();
        }
    }

    public void Study()
    {
        StartCoroutine(StudySession());
    }

    IEnumerator StudySession()
    {
        blocker.SetActive(false);
        wordWriting.StartWriting();
        float originalTimeMultiplier = GameManager.Instance.timeMultiplier;
        GameManager.Instance.timeMultiplier = 0;
        float timeToWait = TimeSpending; // TimeSpending is in minutes
        Debug.Log("Waits for " + timeToWait / 10 + " seconds");

        float elapsedTime = 0;
        float timeIncrement = TimeSpending / (timeToWait / 10); // Calculate the time increment per frame
        while (elapsedTime < timeToWait / 10)
        {
            elapsedTime += Time.deltaTime;
            GameManager.Instance.ChangeTime(timeIncrement * Time.deltaTime);
            yield return null;
        }

        GameManager.Instance.timeMultiplier = originalTimeMultiplier;
        PlayerStats.Instance.Energy -= EnergySpending;
        Debug.Log("forwarded time by " + timeToWait + " minutes");
        blocker.SetActive(true);
        wordWriting.StopWriting();

        // Add to progressBar
        float startValue = 0;
        var latestSection = progressBar.getLatestSection();
        if (latestSection != null)
        {
            startValue = latestSection.endValue;
        }
        float endValue = startValue + approximateWords;
        if (startValue >= endValue)
        {
            endValue = startValue + 1; // Ensure endValue is greater than startValue
        }
        Color color = Color.green;
        progressBar.AddSection(startValue, endValue, color);
    }
}