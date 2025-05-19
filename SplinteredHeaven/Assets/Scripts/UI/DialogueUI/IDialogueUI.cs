using TMPro;
using UnityEngine;

public interface IDialogueUI
{

    void SetActive(bool state);
    void SetActiveContinueIcon(bool state);
    GameObject[] GetChoices();
    void ShowDialogue(string text);
    void ChangeName(string name);
    TextMeshProUGUI GetDialogueText();
    // Add any additional UI functions like ShowChoices, SetPortrait, etc.
}