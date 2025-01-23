using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [System.Serializable]
    public class Section
    {
        public float startValue;
        public float endValue;
        public Image image;
        public float writingQuality; // 0-100
    }

    public List<Section> sections;
    public float maxValue = 36000f;
    public GameObject progressBarContainer;
    public GameObject sectionPrefab;
    public float value;

    void Start()
    {
        InitializeSections();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sections.Count > 0)
            {
                AddSection(sections[sections.Count - 1].endValue, sections[sections.Count - 1].endValue + 10000,
                    sections[sections.Count - 1].color == Color.red ? Color.green : Color.red);
            }
            else
            {
                AddSection(0, 10000, Color.red);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (sections.Count > 0)
            {
                RemoveSection(sections.Count - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (sections.Count > 0)
            {
                EditSection(sections.Count - 1, sections[sections.Count - 1].startValue,
                    sections[sections.Count - 1].endValue + 1000,
                    sections[sections.Count - 1].color == Color.red ? Color.green : Color.red);
            }
        }
        */
    }

    public void AddSection(float startValue, float endValue, Color color, float writingQuality)
    {
        if (startValue >= maxValue)
        {
            Debug.LogError("Start value is greater than or equal to max value, section not added");
            return;
        }

        Section newSection = new Section
        {
            startValue = startValue,
            endValue = endValue,
            writingQuality = writingQuality
        };

        if (startValue >= endValue)
        {
            Debug.LogError("Start value is greater than or equal to end value, increasing start value to end value - 1");
            return;
        }

        if (startValue < 0)
        {
            Debug.LogError("Start value is less than 0, increasing start value to 0");
            newSection.startValue = 0;
        }

        if (endValue > maxValue)
        {
            Debug.LogError("End value is greater than max value, reducing end value to max value");
            newSection.endValue = maxValue;
        }
        sections.Add(newSection);
        CreateSection(newSection);
        InitializeSections();
    }

    public void InitializeSections()
    {
        value = 0;
        
        //DESTROY ALL CHILDREN
        foreach (Transform child in progressBarContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Section section in sections)
        {
            CreateSection(section);
            value += section.endValue - section.startValue;
        }
    }

    void CreateSection(Section section)
    {
        GameObject sectionObject = Instantiate(sectionPrefab, progressBarContainer.transform);
        section.image = sectionObject.GetComponent<Image>();
        section.image.color = Color.Lerp(Color.red, Color.green, section.writingQuality / 100);

        // Set the position and size of the section
        RectTransform rectTransform = section.image.GetComponent<RectTransform>();
        float sectionStartNormalized = section.startValue / maxValue;
        float sectionEndNormalized = section.endValue / maxValue;
        float sectionWidth = sectionEndNormalized - sectionStartNormalized;

        rectTransform.anchorMin = new Vector2(sectionStartNormalized, 0);
        rectTransform.anchorMax = new Vector2(sectionEndNormalized, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        sectionObject.GetComponent<SectionButton>().section = section;
        sectionObject.GetComponent<SectionButton>().progressBar = this;
    }
    
    public void RemoveSection(int index)
    {
        if (index < 0 || index >= sections.Count)
        {
            Debug.LogError("Index out of range, section not removed");
            return;
        }
        sections.RemoveAt(index);
        InitializeSections();
    }

    public void EditSection(int index, float startValue, float endValue, Color color)
    {
        if (index < 0 || index >= sections.Count)
        {
            Debug.LogError("Index out of range, section not edited");
            return;
        }
        sections[index].startValue = startValue;
        sections[index].endValue = endValue;
        InitializeSections();
    }
    
    public Section getLatestSection()
    {
        if(sections.Count == 0)
        {
            return null;
        }
        return sections[sections.Count - 1];
    }
}   