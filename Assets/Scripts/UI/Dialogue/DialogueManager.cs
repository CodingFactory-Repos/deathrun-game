
namespace DialogueSystem
{
    using System.Collections;
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
   
    public class DialogueManager : MonoBehaviour
    {

        private GameObject dialogueCanvas;
        public Image godImage;
        public Sprite godSprite;
        public TextMeshProUGUI  dialogueText;
        public string actualDialogue;
        public TextMeshProUGUI  dialogueTitle;
        public string actualTitle;

        private Coroutine hideCanvasCoroutine;

        void Start()
        {
            dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue");

            if (dialogueCanvas != null) DisplayDialogue(godSprite,actualDialogue,actualTitle);
        }

        public void DisplayDialogue(Sprite newGodSprite, string newDialogue,string newTitle)
        {

             if (!dialogueCanvas.activeSelf)
            {
                dialogueCanvas.SetActive(true); 
            }
            if (newGodSprite != null)godImage.sprite = newGodSprite;
            if (newDialogue != null && newDialogue.Length > 0)dialogueText.text = newDialogue;
            if (newTitle != null && newDialogue.Length > 0) dialogueTitle.text = newTitle;

            if (hideCanvasCoroutine !=null) StopCoroutine(hideCanvasCoroutine);

            hideCanvasCoroutine = StartCoroutine(HideCanvasAfterDelay(20f));
        }
         private IEnumerator HideCanvasAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
          
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(false);
            }
        }

    }

}
