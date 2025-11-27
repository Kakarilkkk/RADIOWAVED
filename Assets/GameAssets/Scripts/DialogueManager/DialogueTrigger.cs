using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public TextAsset inkJSON;
    public AudioClip voice;


    public void StartDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, null);
    }
}
