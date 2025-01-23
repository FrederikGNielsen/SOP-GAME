using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    public bool isTalking;

    private bool isTyping;
    private bool skipTyping;
    public float timescale;
    
    //Dialogue UI
    #region Dialogue UI
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject optionsPanel;
    public GameObject optionPrefab;
    #endregion
    
    
    private int currentDialogueId;
    public List<Dialogue> dialogues;

    private void Awake()
    {
        DialogueSystem.Instance = this;
    }

    private void Update()
    {
        Time.timeScale = timescale;
    }

    void Start()
    {
        NewCharacterJsonFile();
        //Talk("Martin");
        
        dialoguePanel.SetActive(false); 
    }
    
    public void Talk(string CharacterName)
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        Camera.main.GetComponent<PlayerCameraController>().enabled = false;
        Camera.main.transform.parent.GetComponent<PlayerController>().enabled = false;
        
        //unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        LoadDialogues("Assets/_Scripts/Dialogue/" + CharacterName + ".json");
        DisplayDialogue(1);
        
        DebugListAllEvents();

    }
    
    public void DebugListAllEvents()
    {
        foreach (Dialogue dialogue in dialogues)
        {
            Debug.Log($"Dialogue ID: {dialogue.ID}, Speaker: {dialogue.Speaker}");
            foreach (Option option in dialogue.Options)
            {
                foreach (string eventName in option.Events)
                {
                    Debug.Log($"Event: {eventName}");
                }
            }
        }
    }
    
    
    public void StopTalking()
    {
        isTalking = false;

        // Reset dialogue panel and options
        dialoguePanel.SetActive(false);
        foreach (Transform oldOption in optionsPanel.transform)
        {
            Destroy(oldOption.gameObject);
        }

        // Reset the current dialogue ID to an invalid state or default
        currentDialogueId = -1;

        // Re-enable other components, e.g., player controls
        Camera.main.GetComponent<PlayerCameraController>().enabled = true;
        Camera.main.transform.parent.GetComponent<PlayerController>().enabled = true;
        
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Stopped Talking");
    }

    void LoadDialogues(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        DialogueContainer dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonString);

        dialogues = new List<Dialogue>(dialogueContainer.dialogues);
    }

    void NewCharacterJsonFile()
    {
        // Create a new character JSON file
        Dialogue dialogue = new Dialogue
        {
            ID = 0,
            Speaker = "Character Name",
            Text = "Character's first dialogue",
            Options = new List<Option>
            {
                new Option
                {
                    Text = "Option 1",
                    NextDialogueId = 1
                },
                new Option
                {
                    Text = "Option 2",
                    NextDialogueId = 2,
                    Events = new List<string> { "Event1", "Event2" }
                }
            }
        };
        string jsonString = JsonUtility.ToJson(dialogue, true);
        File.WriteAllText("Assets/_Scripts/Dialogue/character.json", jsonString);
    }

    #region MyRegion
    

    void DisplayDialogue(int dialogueId)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.ID == dialogueId)
            {
                currentDialogueId = dialogueId;
                speaker.text = dialogue.Speaker;
                WriteText(dialogueText, dialogue.Text);

                // Clear existing options
                foreach (Transform oldOption in optionsPanel.transform)
                {
                    Destroy(oldOption.gameObject);
                }
            }
        }
    }
    
        
    public void WriteText(TextMeshProUGUI text, string message)
    {
        //type out text
        StopAllCoroutines();    
        StartCoroutine(TypeText(text, message));
    }
    
    public IEnumerator TypeText(TextMeshProUGUI text, string message)
    {
        isTyping = true;
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return null;
        }

        isTyping = false;
        DisplayOptions();
    }
    
    void DisplayOptions()
    {
        if (!isTyping)
        {
            Dialogue currentDialogue = dialogues.Find(d => d.ID == currentDialogueId);
            if (currentDialogue != null)
            {
                foreach (Option option in currentDialogue.Options)
                {
                    GameObject newOption = Instantiate(optionPrefab, optionsPanel.transform);
                    newOption.GetComponentInChildren<TextMeshProUGUI>().text = option.Text;
                }
            }
        }
    }
    
    public void SelectOption(int optionIndex)
    {
        Dialogue currentDialogue = dialogues.Find(d => d.ID == currentDialogueId);
        if (currentDialogue == null)
        {
            Debug.LogError($"Dialogue ID {currentDialogueId} not found.");
            return;
        }

        if (currentDialogue.Options == null || optionIndex < 0 || optionIndex >= currentDialogue.Options.Count)
        {
            Debug.LogError($"Invalid option index: {optionIndex}");
            return;
        }

        Option selectedOption = currentDialogue.Options[optionIndex];

        // Handle events first
        foreach (string eventName in selectedOption.Events)
        {
            HandleEvent(eventName, selectedOption.Rewards); // Pass rewards here
            if (eventName == "StopTalking")
            {
                return; // Stop further processing if StopTalking is triggered
            }
        }

        // Proceed to next dialogue if no StopTalking event is triggered
        currentDialogueId = selectedOption.NextDialogueId;
        DisplayDialogue(currentDialogueId);
    }


    
    #endregion
    

    #region Event Handling
    void HandleEvent(string eventName, List<Reward> rewards)
    {
        switch (eventName)
        {
            case "Event1":
                //Event1();
                break;
            case "Event2":
                //Event2();
                break;
            case "StopTalking":
                StopTalking();
                Debug.Log("Stopped Talking");
                break;
            default:
                Debug.LogWarning($"Event '{eventName}' not recognized.");
                break;
        }

        foreach (var reward in rewards)
        {
            if (!string.IsNullOrEmpty(reward.RewardType) && reward.RewardAmount > 0)
            {
                GiveReward(reward.RewardType, reward.RewardAmount);
                Debug.Log($"Given {reward.RewardAmount} {reward.RewardType}");
            }
        }
    }

    void GiveReward(string rewardType, int rewardAmount)
    {
        switch (rewardType)
        {
            case "Plagiat":
                // Implement logic to give Plagiat
                Debug.Log($"Given {rewardAmount} Plagiat");
                break;
            case "Grade":
                // Implement logic to give Grade
                Debug.Log($"Given {rewardAmount} Grade");
                break;
            default:
                Debug.LogWarning($"Reward type '{rewardType}' not recognized.");
                break;
        }
    }
    
    #endregion
}

[System.Serializable]
public class Dialogue
{
    public int ID;
    
    public string Speaker;
    public string Text;
    public List<Option> Options;
}

[System.Serializable]
public class Option
{
    public string Text;
    public int NextDialogueId;
    public List<string> Events; // List of events
    public List<Reward> Rewards; // List of rewards
}

[System.Serializable]
public class DialogueContainer
{
    public List<Dialogue> dialogues;
}

[System.Serializable]
public class Reward
{
    public string RewardType; // "Plagiat" or "Grade"
    public int RewardAmount;  // Amount to give
}