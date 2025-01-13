    using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class WordWriting : MonoBehaviour
{
    public bool IsWriting;

    public TextMeshProUGUI text;

    public float wordsPerMinute = 60f;

    private float wordDelay;
    private float scrollSpeed;

    private void Start()
    {
        IsWriting = true;

        StartCoroutine(WriteText());
    }

    private void Update()
    {
        if(!IsWriting)
            return;
        wordDelay = 60 / wordsPerMinute;
        float charactersPerMinute = wordsPerMinute * 5; // Assuming an average word length of 5 characters
        float linesPerMinute = charactersPerMinute / 45; // Assuming an average line length of 45 characters
        scrollSpeed = linesPerMinute / 10; // Convert lines per minute to lines per second
        text.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
    }

    IEnumerator WriteText()
    {
        // Take lorem ipsum lines from file and write every second line
        string[] lines = File.ReadAllLines("Assets/_Scripts/Writing/RandomWriting.txt");

        while (IsWriting)
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
    }
}