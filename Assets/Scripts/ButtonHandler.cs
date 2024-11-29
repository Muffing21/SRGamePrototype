using UnityEngine;

public class ButtonActionHandler : MonoBehaviour
{
    public DialogueActionSO dialogueAction;

    public void TriggerAction()
    {
        if (dialogueAction != null)
        {
            dialogueAction.ExecuteAction(gameObject); // Pass the button's GameObject
        }
        else
        {
            Debug.LogWarning("No DialogueActionSO assigned to this button.");
        }
    }
}