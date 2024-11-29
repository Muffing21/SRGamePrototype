using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Line")]
public class DialogueLineSO : ScriptableObject
{
    public string speakerName;
    public string dialogueText;
    public GameObject speakerAvatar;
}
