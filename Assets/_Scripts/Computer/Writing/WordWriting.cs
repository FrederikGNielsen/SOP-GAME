using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;

public class WordWriting : MonoBehaviour
{
    public bool isWriting;
    public TextMeshProUGUI text;
    public float wordsPerMinute = 60f;

    private float wordDelay;
    private float scrollSpeed;
    private bool isWritingCoroutineRunning;

    private void Update()
    {
        if (!isWriting)
        {
            StopAllCoroutines();
            return;
        }

        CalculateDelays();
        ScrollText();

        if (!isWritingCoroutineRunning)
        {
            StartCoroutine(WriteText());
        }
    }

    public void StartWriting()
    {
        isWriting = true;
    }

    public void StopWriting()
    {
        isWriting = false;
        isWritingCoroutineRunning = false;
    }

    private void CalculateDelays()
    {
        wordDelay = 60 / (wordsPerMinute * ComputerDocument.instance.TimeSpending);
        float charactersPerMinute = (wordsPerMinute * ComputerDocument.instance.TimeSpending) * 5; // Assuming an average word length of 5 characters
        float linesPerMinute = charactersPerMinute / 45; // Assuming an average line length of 45 characters
        scrollSpeed = linesPerMinute / 10; // Convert lines per minute to lines per second
    }

    private void ScrollText()
    {
        text.transform.Translate(Vector3.up * (Time.deltaTime * scrollSpeed));
    }

    private IEnumerator WriteText()
    {
        isWritingCoroutineRunning = true;
        string[] lines = File.ReadAllLines("Assets/_Scripts/Computer/Writing/RandomWriting.txt");

        while (isWriting)
        {
            for (int i = 0; i < lines.Length; i += 2)
            {
                string line = lines[i];
                foreach (var character in line)
                {
                    text.text += character;
                    yield return new WaitForSeconds(wordDelay / 5); // Adjust delay for characters
                }
                text.text += "\n";
            }
        }
        isWritingCoroutineRunning = false;
    }
}