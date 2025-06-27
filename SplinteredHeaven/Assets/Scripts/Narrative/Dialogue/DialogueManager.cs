using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class DialogueManager :  UIManager<IDialogueUI>
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Choices UI")]
    private TextMeshProUGUI[] choicesText;
    private int currentChoiceNumber = 0;

    private new IDialogueUI currentUI;

    public Action OnDialogueStarted;
    public Action OnDialogueEnded;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";

    private DialogueVariables dialogueVariables;
    //private InkExternalFunctions inkExternalFunctions;


    //BUTTON INPUT
    private bool resumeButton = false;
    private bool makingChoice = false;
    private int currentChoice = 0; // This will be used to track the current choice index when making a choice
    public List<GameObject> selectorList = new List<GameObject>();

    //SCROLL
    float scrollSpeed = 0f;

    //Handle Singleton
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        //inkExternalFunctions = new InkExternalFunctions();

        //audioSource = this.gameObject.AddComponent<AudioSource>();
        //currentAudioInfo = defaultAudioInfo;
    }
    


    public static DialogueManager GetInstance()
    {
        return Instance as DialogueManager;
    }

    public override void RegisterUI(IDialogueUI newUI)
    {
        currentUI = newUI;
    }

    private void Start()
    {
        EventManager.Instance.MouseWheel += ScrollWheel;

        // handle continuing to the next line in the dialogue when submit is pressed
        EventManager.Instance.LeftMouseDownEvent += PressResumeButton;
        EventManager.Instance.FPDialogueEvent += EnterDialogueMode;

        dialogueIsPlaying = false;
        if (currentUI != null)
        {
            currentUI.SetActive(false);
            choicesText = new TextMeshProUGUI[currentUI.GetChoices().Length];
            int index = 0;
            foreach (GameObject choice in currentUI.GetChoices())
            {
                selectorList.Add(choice.transform.GetChild(1).gameObject);
                choice.transform.GetChild(1).gameObject.SetActive(false);
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }
        }

        // get the layout animator
        //layoutAnimator = dialoguePanel.GetComponent<Animator>();

        // get all of the choices text / SET UP CHOICES
        

        //InitializeAudioInfoDictionary();
    }

    private void Update()
    {
        // return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (resumeButton == true && canContinueToNextLine && makingChoice == false)
        {
            resumeButton = false;
            ContinueStory();
        }

    }

    //---------------------------------------------------------------BASIC DIALOGUE FEATURES---------------------------------------------------------------------------

    //public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator)
    public void EnterDialogueMode(TextAsset inkJSON, Vector3 pos)
    {
        if (dialogueIsPlaying)
        {
            return;
        }
        TimeManager.Instance.Stop(); // Pause the game time when entering dialogue mode
        OnDialogueStarted?.Invoke();
        EventManager.Instance.StartBlockInteraction(true); // Block interaction while dialogue is playing

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        currentUI.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        currentStory.BindExternalFunction("dialogueTrigger", (string keyword) =>
        {
            EventManager.Instance.StartDialogueTriggerEvent(keyword);
        });

        // reset portrait, layout, and speaker
        
            //currentUI.ChangeName("???");

        //portraitAnimator.Play("default");
        //layoutAnimator.Play("right");

        canContinueToNextLine = true;
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        //yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        currentStory.UnbindExternalFunction("dialogueTrigger");
        OnDialogueEnded?.Invoke();
        
        currentUI.SetActive(false);
        //dialogueText.text = "";
        currentUI.ShowDialogue("");

        // go back to default audio
        //SetCurrentAudioInfo(defaultAudioInfo.id);
        EventManager.Instance.ExitFirstPersonDialogue();

        yield return new WaitForSeconds(0.15f);
        dialogueIsPlaying = false;
        EventManager.Instance.StartBlockInteraction(false);
        TimeManager.Instance.Play();
    }


    private void ContinueStory()
    {

        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (!canContinueToNextLine && currentStory.currentChoices.Count != 0)
        {
            return;
        }
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            // handle case where the last line is an external function
            if (nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
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
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // set the text to the full line, but set the visible characters to 0
        //dialogueText.text = line;
        currentUI.ShowDialogue(line);
        currentUI.GetDialogueText().maxVisibleCharacters = 0;
        // hide items while text is typing
            //currentUI.SetActiveContinueIcon(false);
        //HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // if the submit button is pressed, finish up displaying the line right away
            if (resumeButton)
            {
                resumeButton = false;
                currentUI.GetDialogueText().maxVisibleCharacters = line.Length;
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
                AudioManager.instance.PlayOneShot(FMODEvents.instance.characterTalk, Camera.main.transform.position);
                currentUI.GetDialogueText().maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // actions to take after the entire line has finished displaying
        //currentUI.SetActiveContinueIcon(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    //---------------------------------------------------------------CHOICES---------------------------------------------------------------------------
    private void HideChoices()
    {
        foreach (GameObject choiceButton in currentUI.GetChoices())
        {
            choiceButton.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count!=0)
        {
            makingChoice = true;
        }

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > currentUI.GetChoices().Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
            makingChoice = false;
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            currentUI.GetChoices()[index].gameObject.SetActive(true);
            if(index == 0)
            {
                currentUI.GetChoices()[0].transform.GetChild(1).gameObject.SetActive(true); // Show the first choice cursor

            }
            choicesText[index].text = choice.text;
            // set the button to call the correct function when pressed
            int choiceIndex = index; // capture the current index for the lambda function
            currentUI.GetChoices()[index].GetComponent<Button>().onClick.RemoveAllListeners();
            currentUI.GetChoices()[index].GetComponent<Button>().onClick.AddListener(() => SelectChoice(choiceIndex));
            index++;
        }
        currentChoiceNumber = index; // Store the number of choices available
        currentChoice =0; // Reset the current choice index
        foreach (GameObject selector in selectorList)
        {
            selector.SetActive(false); // Hide all selectors
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < currentUI.GetChoices().Length; i++)
        {
            currentUI.GetChoices()[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    
    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        //EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        //EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            // NOTE: The below two lines were added to fix a bug after the Youtube video was made
            //InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script
            ContinueStory();
        }


    }
    

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    currentUI.ChangeName(tagValue);
                    break;
                case PORTRAIT_TAG:
                    //currentUI.portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    //currentUI.layoutAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    //SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    // This method will get called anytime the application exits.
    // Depending on your game, you may want to save variable state in other places.
    public void OnApplicationQuit()
    {
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
        
    }

    /*
    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        // set variables for the below based on our config
        //AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        // play the sound based on the config
        if (currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            AudioClip soundClip = null;
            // create predictable audio from hashing
            if (makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                // sound clip
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];
                // pitch
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                // cannot divide by 0, so if there is no range then skip the selection
                if (pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            // otherwise, randomize the audio
            else
            {
                // sound clip
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                // pitch
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }

            // play sound
            audioSource.PlayOneShot(soundClip);
        }
    }
    */
    

    
    



    /*
     private void InitializeAudioInfoDictionary()
     {
         audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
         audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
         foreach (DialogueAudioInfoSO audioInfo in audioInfos)
         {
             audioInfoDictionary.Add(audioInfo.id, audioInfo);
         }
     }

     private void SetCurrentAudioInfo(string id)
     {
         DialogueAudioInfoSO audioInfo = null;
         audioInfoDictionary.TryGetValue(id, out audioInfo);
         if (audioInfo != null)
         {
             this.currentAudioInfo = audioInfo;
         }
         else
         {
             Debug.LogWarning("Failed to find audio info for id: " + id);
         }
     }

     */


    private void PressResumeButton()
    {
        if(makingChoice == false)
        {
            resumeButton = true;
        }
        else if(canContinueToNextLine)
        {
            resumeButton = false;
            makingChoice = false;
            MakeChoice(currentChoice);
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        switch (choiceIndex)
        {
            case 0:
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
                makingChoice = false;
                break;
            case 1:
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
                makingChoice = false;
                break;
            case 2:
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
                makingChoice = false;
                break;
            case 3:
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
                makingChoice = false;
                break;
            case 4:
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
                makingChoice = false;
                break;
            default:
                break;
        }
    }

    public void ScrollWheel(float sSpeed)
    {
        if(sSpeed > 0)
        {
            ChoiceCursorUp();
        }
        else if(sSpeed < 0)
        {
            ChoiceCursorDown();
        }
        scrollSpeed = sSpeed;
    }

    public void ChoiceCursorDown()
    {
        if(dialogueIsPlaying == false || makingChoice == false)
        {
            return;
        }
        selectorList[currentChoice].SetActive(false);
        if (currentChoice < (currentChoiceNumber-1))
        {
            currentChoice++;
        }
        selectorList[currentChoice].SetActive(true);
    }

    public void ChoiceCursorUp()
    {
        if (dialogueIsPlaying == false || makingChoice == false)
        {
            return;
        }
        selectorList[currentChoice].SetActive(false);
        if (currentChoice > 0)
        {
            currentChoice--;
        }
        selectorList[currentChoice].SetActive(true);
    }

}
