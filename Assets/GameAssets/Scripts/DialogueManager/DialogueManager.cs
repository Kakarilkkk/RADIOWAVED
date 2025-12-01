using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;




    [Header("ChoisesUI")]
    [SerializeField] GameObject[] choises;
    private TextMeshProUGUI[] choicesText;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    public UnityEvent pickingUpObj;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    



    // TAG VARS
    private const string SPEAKER_TAG = "speaker";
    private const string FUNCTION_TAG = "func";
    private const string AUDIO_TAG = "voice";

    //private DialogueVars dialogueVars;

    #region  Singleton
    private static DialogueManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;



//dialogueVars = new DialogueVars(loadGlobalsJSON);

        //dialogueEvents = GetComponent<DialogueEvents>();
        
    }
    #endregion

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choises.Length];
        int index = 0;
        foreach (GameObject choice in choises)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        
    }
    

    

    void Update()
    {
        if (dialogueIsPlaying)
        {
            
        }

        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    #region Enter / Exit / Continue Story

    public void EnterDialogueMode(TextAsset inkJSON, Void functionName)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //dialogueVars.StartListenting(currentStory);

        currentStory.BindExternalFunction("doUnityEvent", (string eventName) =>
        {
            //unityEvent.Invoke();
        });
        currentStory.BindExternalFunction("doUnityFunction", (string functionName) =>
        {
            //dialogueEvents.CallTheFunction(functionName); // call the void by its name
        });
        currentStory.BindExternalFunction("NextAct", (string functionName) =>
        {
            FindAnyObjectByType<TimeManager>().NextAct(); // call the void by its name
        });

        // reset the dialog window
        displayNameText.text = "";
        // animator play default 

        ContinueStory();
    }

    void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //dialogueVars.StopListenting(currentStory);
        currentStory.UnbindExternalFunction("doUnityEvent");

        dialogueText.text = ""; // resets the texts in the box

        pickingUpObj.Invoke();
        //DialogueAudioManager.instance.SetCurrentAudioInfo(DialogueAudioManager.instance.defaultAudioInfo.id);
        
        //RadioSwicher.instance.waves.Remove(RadioSwicher.instance.currentwave); // if i switch time, the list also switches, so it doesnt matter
        RadioSwicher.instance.currentwave = null;
        RadioSwicher.instance.ListenButton.SetActive(false);
        RadioSwicher.instance.caughtTheWave = false;
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            // handle case where the last line is an external function
            if (nextLine.Equals("") && !currentStory.canContinue)
            {
                ExitDialogueMode();
            }
            // otherwise, handle the normal case for continuing the story
            else
            {
                // handle tags
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
        }
        else
        {
            ExitDialogueMode();
        }
    }
    #endregion

    #region Choises
    private void DisplayChoices()
    {
        continueIcon.SetActive(false);
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choises.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choises[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choises.Length; i++)
        {
            choises[i].gameObject.SetActive(false);
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choice in choises)
        {
            choice.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
            // all choises ui pannels set active - false
        }

    }
    #endregion

    private IEnumerator DisplayLine(string line)
    {
        // set the text to the full line, but set the visible characters to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcon.SetActive(false);
        // hide items while text is typing
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            if (Input.GetMouseButtonDown(0))
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            // if not rich text, add the next letter and wait a small time
            else
            {
                
                DialogueAudioManager.instance.PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // actions to take after the entire line has finished displaying
        DisplayChoices();
        continueIcon.SetActive(true);
        canContinueToNextLine = true;
    }


    #region Tags and Vars
    void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("Smth wrong with tag" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;

                    case FUNCTION_TAG:
                    TimeManager time = FindAnyObjectByType<TimeManager>();
                    time.NextAct();
                    break;
                // case PLAYER_SPRITE_TAG:
                //     PlayerChangeSprite.Play(tagValue);
                //     break;
                // case NPC_SPRITE_TAG:
                //     NpcChangeSprite.Play(tagValue);
                //     break;
                // case TALKING_INT:
                //     string talking = tagValue;
                //     int result = int.Parse(talking);
                //     SpriteAnimator.SetInteger("TalkingAnimationState", result);
                //     break;
                // case AUDIO_TAG:
                //     SetCurrentAudioInfo(tagValue);
                //     break;
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        //dialogueVars.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
    #endregion

}
