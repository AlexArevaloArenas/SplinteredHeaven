using TMPro;
using UnityEngine;

        public class SceneUIDialogue : SceneUI<DialogueManager, IDialogueUI>, IDialogueUI
        {
            [Header("General UI Items")]
            public GameObject dialoguePanel;
            public GameObject continueIcon;
            public TextMeshProUGUI dialogueText;
            public TextMeshProUGUI displayNameText;
            public Animator portraitAnimator;
            public Animator layoutAnimator;

            [Header("Choices Button List")]
            public GameObject[] choices;

            

            public override void InitializeUI()
            {
                // Optional UI setup
            }

            protected override DialogueManager GetManagerInstance()
            {
                return (DialogueManager)DialogueManager.GetInstance();
            }

            void IDialogueUI.ChangeName(string name)
            {
                //displayNameText.text = name;
                dialogueText.text += "<color=\"red\">" + name + "</color>";
            }

            GameObject[] IDialogueUI.GetChoices()
            {
                return choices;
            }

            TextMeshProUGUI IDialogueUI.GetDialogueText()
            {
                 return dialogueText;
            }

            void IDialogueUI.SetActive(bool state)
            {
                dialoguePanel.SetActive(state);
            }

            void IDialogueUI.SetActiveContinueIcon(bool state)
            {
                continueIcon.SetActive(state);
            }

            void IDialogueUI.ShowDialogue(string text)
            {
                dialogueText.text = text;
            }

            // Implement IDialogueUI methods below
}