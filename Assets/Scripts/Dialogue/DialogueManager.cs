using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f; // smaller the number, faster the typing

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Shop UI")]
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellPanel;

    private Story currentStory;
    public bool dialogueIsPlaying {  get; private set; }

    private static DialogueManager instance;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private Input submitInput;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found!");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine &&  currentStory.currentChoices.Count == 0 
            && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit")))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        currentStory.BindExternalFunction("ShowBuyMenu", ShowBuyMenu);
        currentStory.BindExternalFunction("ShowSellMenu", ShowSellMenu);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        currentStory.UnbindExternalFunction("ShowBuyMenu");
        currentStory.UnbindExternalFunction("ShowSellMenu");

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            //dialogueText.text = currentStory.Continue();
            // display choices, if any, for this dialogue line
            
            // handle tags
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // empty the dialogue text
        dialogueText.text = "";
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            for (int i = 1; i < line.ToCharArray().Length; i++)
            {
                //yield return new WaitForSeconds(typingSpeed);
                dialogueText.maxVisibleCharacters = i;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
                {
                    dialogueText.maxVisibleCharacters = line.ToCharArray().Length;
                    break;
                }
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.text += letter;

                if (letter == '>') isAddingRichTextTag = false;
            }
            else
            {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can display! Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // go through the remaining choicces the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    private void ShowBuyMenu()
    {
        Debug.Log("Showing buy menu");

        //dialoguePanel.SetActive(false);  // Optionally hide dialogue while shopping
        //buyPanel.SetActive(true);
    }

    private void ShowSellMenu()
    {
        Debug.Log("Showing sell menu");
        //dialoguePanel.SetActive(false);  // Optionally hide dialogue while shopping
        //sellPanel.SetActive(true);
    }

    private void EndConversation()
    {
        StartCoroutine(ExitDialogueMode());
    }

    // Add methods to handle closing shop UI and returning to dialogue
    public void CloseBuyMenu()
    {
        buyPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void CloseSellMenu()
    {
        sellPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
}
