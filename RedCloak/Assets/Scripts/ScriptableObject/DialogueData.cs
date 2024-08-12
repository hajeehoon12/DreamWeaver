using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName ="NewDialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string name;
        public string dialogueText;
    }

    public List<DialogueLine> dialogueLines;
}
