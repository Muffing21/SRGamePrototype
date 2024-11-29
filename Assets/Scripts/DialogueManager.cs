using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.VisualScripting;
// using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    #region UI STUFF
    public TMP_Text dialogueText; // Dialogue text field
    public TMP_Text speakerNameText; // Speaker name text field
    public GameObject dialoguePanel;
    private GameObject currentAvatarInstance;
    private RectTransform dialoguePanelRectTransform;
    public GameObject endPanel;

    [SerializeField] GameObject superResumeCanvas;
    [SerializeField] RectTransform superResumeRectTransform;
    public Button nextButton; // Button to progress dialogue
    [SerializeField] Button nextButtonPhase2;
    private GameObject modifiedAvatar;
    #endregion

    [Header("Dialogue Data")]
    #region SCRIPTABLE OBJECTS

    public List<DialogueLineSO> dialogueLines; // List of dialogue lines
    [Header("Dialogue Action")]
    public List<DialogueActionSO> dialogueActions; // List of dialogue lines
    public List<DialogueLineSO> dialogueLinesPhase2; // List of dialogue lines  

    #endregion
    #region VARIABLES
    private int currentLineIndex = 0; // Current dialogue line index
    private bool actionInProgress = false; // Flag to check if an action is currently executing
    private int currentActionCounter = 0;

    private bool isPlayerSpeaking = true;
    private bool isMoving = false;
    private Coroutine typingCoroutine;
    #endregion

    public static DialogueManager Instance { get; set; }

    private void Awake(){
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple DialogueManager instances found! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dialoguePanelRectTransform = dialoguePanel.GetComponent<RectTransform>();
        nextButton.onClick.AddListener(DisplayNextLine);
        nextButtonPhase2.onClick.AddListener(TransitionToLast);
        DisplayNextLine();
    }

    public void DisplayNextLine(){
        // Check if we've reached the end of the dialogue
        if (currentLineIndex < dialogueLines.Count){
            // Get the current dialogue line
            DialogueLineSO currentLine = dialogueLines[currentLineIndex];

            // Update UI elements
            speakerNameText.text = currentLine.speakerName;
            StartTyping(currentLine.dialogueText);
            UpdateAvatarLine(currentLine.speakerAvatar);

            // Advance to the next line
            currentLineIndex++;
        }
        else{
            currentLineIndex = 0;
            EndDialogue();
        }
    }

    public void DisplayNextLinePhase2(){
        if (currentLineIndex < dialogueLinesPhase2.Count){
            Debug.Log(currentLineIndex);
            DialogueLineSO currentLine = dialogueLinesPhase2[currentLineIndex];

            speakerNameText.text = currentLine.speakerName;
            StartTyping(currentLine.dialogueText);
            
            if(!isPlayerSpeaking){
                UpdateAvatarLine(currentLine.speakerAvatar);    
            }
            
            isPlayerSpeaking = !isPlayerSpeaking;
            currentLineIndex++;
        }
        else{
            GameOver();
        }
    }

    public void ExecuteCurrentAction(DialogueActionSO currentAction, string spriteToChange)
    {
        if (actionInProgress) return; // Prevent triggering another action if one is already in progress

        actionInProgress = true; // Set the action in progress flag
            
        if (currentActionCounter < dialogueActions.Count){
            UpdateAvatar(currentAction.speakerAvatar, spriteToChange);
            speakerNameText.text = currentAction.speakerName;
            
            StartTyping(currentAction.dialogueText);
            currentActionCounter++;
            
        }
        if(currentActionCounter == dialogueActions.Count){
            modifiedAvatar = currentAction.speakerAvatar.GetComponent<GameObject>();
            EndActionDialogue();
        }
        
        
        actionInProgress = false; // Reset the flag after the action is done

    }

    void EndDialogue()
    {
        Debug.Log("Dialogue Ended");

        TransitionToActions();

    }

    void EndActionDialogue(){
        Debug.Log("All actions completed.");
        nextButtonPhase2.gameObject.SetActive(true);
    }

    void GameOver(){
        endPanel.SetActive(true);
    }

    void TransitionToActions(){
        nextButton.onClick.RemoveAllListeners();
        dialoguePanelRectTransform.sizeDelta = new Vector2(900, 400);
        dialoguePanelRectTransform.anchoredPosition = new Vector2(-450, -320);
        speakerNameText.rectTransform.anchoredPosition = new Vector2(5, 110);
        speakerNameText.rectTransform.sizeDelta = new Vector2(850, 140);
        dialogueText.rectTransform.sizeDelta = new Vector2(850, 200);
        dialogueText.rectTransform.anchoredPosition = new Vector2(0, 10);
        nextButton.gameObject.SetActive(false);
        ShowCanvas(superResumeCanvas);
    }
    
    void TransitionToLast(){
        // DisplayNextLinePhase2();
        nextButtonPhase2.gameObject.SetActive(false);
        nextButtonPhase2.onClick.RemoveListener(TransitionToLast);        
        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        UpdateDialogueUI(
            new Vector2(1800, 400), new Vector2(0, -300),
            new Vector2(1260, 50), new Vector2(-210, 140),
            new Vector2(1700, 180), new Vector2(7, -15)
        );
        superResumeRectTransform.anchoredPosition = new Vector2(5, 9);
        nextButton.onClick.AddListener(DisplayNextLinePhase2);
    }




    void UpdateDialogueUI(
    Vector2 panelSize, Vector2 panelPosition,
    Vector2 nameSize, Vector2 namePosition,
    Vector2 textSize, Vector2 textPosition
)
{
    dialoguePanelRectTransform.sizeDelta = panelSize;
    dialoguePanelRectTransform.anchoredPosition = panelPosition;

    speakerNameText.rectTransform.sizeDelta = nameSize;
    speakerNameText.rectTransform.anchoredPosition = namePosition;

    dialogueText.rectTransform.sizeDelta = textSize;
    dialogueText.rectTransform.anchoredPosition = textPosition;
}


    void HideCanvas(GameObject canvas){
        canvas.SetActive(false);
    }

    void ShowCanvas(GameObject canvas){
        canvas.SetActive(true);
    }
    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
        dialogueText.text += c;
        yield return new WaitForSeconds(0.01f);
        }   
    }

    public void StartTyping(string text){
        if(typingCoroutine != null){
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    void UpdateAvatar(GameObject newAvatarPrefab, string spriteToChange = null){

        if (newAvatarPrefab != null){
            // currentAvatarInstance = Instantiate(newAvatarPrefab);
            ModifyChildSprite(currentAvatarInstance, spriteToChange);
     
        }
    }

    void UpdateAvatarLine(GameObject newAvatarPrefab){
        if (currentAvatarInstance != null){
            Destroy(currentAvatarInstance);
        }
        if (newAvatarPrefab != null){
            currentAvatarInstance = Instantiate(newAvatarPrefab);
        }
    }

    void ModifyChildSprite(GameObject speakerAvatar, string childName)
    {
        Transform child = speakerAvatar.transform.Find(childName);

        if (child != null)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                switch (childName)
                {
                    case "Hair":
                        Transform hairSprite = speakerAvatar.transform.Find("grad-cap");
                        Debug.Log(hairSprite != null);
                        hairSprite.gameObject.SetActive(true);
                        break;

                    case "Face":
                        if(spriteRenderer.sprite == Resources.Load<Sprite>("Images/smile-face")){
                            Sprite faceSprite2 = Resources.Load<Sprite>("Images/concerned-face");
                            spriteRenderer.sprite = faceSprite2;
                        }
                        else{
                            Sprite faceSprite = Resources.Load<Sprite>("Images/smile-face");
                            spriteRenderer.sprite = faceSprite;
                        }
                        break;

                    case "Acc":
                        Debug.Log("Modifying Head");
                        Sprite accessorySprite = Resources.Load<Sprite>("Images/bow");
                        spriteRenderer.sprite = accessorySprite;
                        break;
                    
                    case "Acc2":
                        Debug.Log("Modifying Acc2");
                        Sprite accessory2Sprite = Resources.Load<Sprite>("Images/moustache");
                        spriteRenderer.sprite = accessory2Sprite;
                        break;

                    case "Hair_Back":
                        Debug.Log("Modifying Head");
                        Sprite hairBackSprite = Resources.Load<Sprite>("Images/grad-cap");
                        spriteRenderer.sprite = hairBackSprite;
                        break;
                        
                    default:
                        Debug.LogWarning($"No specific action defined for {childName}");
                        break;
                }
            }
            else
            {
                Debug.LogWarning($"{childName} does not have a SpriteRenderer component.");
                return;
            }
        }
        else
        {
            Debug.LogWarning($"{childName} not found in PlayerAvatar.");
            return;
        }
    }
}

