using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueAction", menuName = "Dialogue/Dialogue Action")]
public class DialogueActionSO : ScriptableObject
{
    public string speakerName;

    public string actionName;

    public string dialogueText;

    public GameObject speakerAvatar;

    public string spriteToChange;
    public void ExecuteAction(GameObject buttonGameObject)
    {
        Debug.Log($"Executing Action: {actionName}");

        // Hide the parent of the button
        if (buttonGameObject != null && buttonGameObject.transform.parent != null)
        {
            DialogueManager.Instance.ExecuteCurrentAction(this, spriteToChange);
            buttonGameObject.transform.parent.gameObject.SetActive(false);
            
        }
        else
        {
            Debug.LogWarning("Button or its parent is null.");
        }
    }
}