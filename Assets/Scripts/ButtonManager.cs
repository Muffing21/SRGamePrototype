using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonAction
    {
        public Button button;          // Reference to the UI Button
        public string actionType;      // Action identifier (e.g., "Jump", "Run")
    }

    public List<ButtonAction> buttons = new List<ButtonAction>(); // List of buttons and their actions

    private void Awake()
    {
        // Assign actions to buttons during initialization
        foreach (var buttonAction in buttons)
        {
            if (buttonAction.button != null)
            {
                buttonAction.button.onClick.AddListener(() => ExecuteAction(buttonAction.actionType));
            }
        }
    }

    private void ExecuteAction(string actionType)
    {
        switch (actionType)
        {
            case "continue":
                Debug.Log("Executing Jump Action");
                // Perform Jump Logic
                break;

            case "Run":
                Debug.Log("Executing Run Action");
                // Perform Run Logic
                break;

            case "Attack":
                Debug.Log("Executing Attack Action");
                // Perform Attack Logic
                break;

            default:
                Debug.LogWarning($"Action '{actionType}' is not defined.");
                break;
        }
    }
}
