namespace DialogueSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Newtonsoft.Json.Linq;

    public class DialogueManager : MonoBehaviour
    {
        private GameObject dialogueCanvas;
        public Image godImage;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI dialogueTitle;

        private SocketIOUnity clientSocket;
        private Queue<GodMessage> messageQueue = new Queue<GodMessage>();
        private bool isDisplayingMessage = false;

        public List<Sprite> godSprites = new List<Sprite>();

        private struct GodInfo
        {
            public Sprite godSprite;
            public string godName;

            public GodInfo(Sprite sprite, string name)
            {
                godSprite = sprite;
                godName = name;
            }
        }

        private struct GodMessage
        {
            public Sprite godSprite;
            public string dialogue;
            public string title;

            public GodMessage(Sprite sprite, string dialogue, string title)
            {
                godSprite = sprite;
                this.dialogue = dialogue;
                this.title = title;
            }
        }

        private Dictionary<string, GodInfo> godPrefabDictionary = new Dictionary<string, GodInfo>();
        private Coroutine hideCanvasCoroutine;

        void Start()
        {
            dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue");
            clientSocket = SocketManager.Instance.ClientSocket;
            dialogueCanvas.SetActive(false);

            godPrefabDictionary.Add("1", new GodInfo(godSprites[0], "Greed"));
            godPrefabDictionary.Add("2", new GodInfo(godSprites[1], "Chaos"));
            godPrefabDictionary.Add("3", new GodInfo(godSprites[2], "Gluttony"));
            godPrefabDictionary.Add("4", new GodInfo(godSprites[3], "Envy"));
            godPrefabDictionary.Add("5", new GodInfo(godSprites[4], "Death"));
            godPrefabDictionary.Add("6", new GodInfo(godSprites[5], "Vanity"));
            godPrefabDictionary.Add("7", new GodInfo(godSprites[6], "Sloth"));

            StartCoroutine(ProcessMessageQueue());
        }

        public void Update()
        {
            clientSocket.On("god:message", response =>
            {
                Debug.Log(response.ToString());

                JArray trapDataArray = JArray.Parse(response.ToString());
                JObject messageData = (JObject)trapDataArray[0][0];

                string godId = messageData["godID"].ToString();
                string messageText = messageData["message"].ToString();

                if (godPrefabDictionary.TryGetValue(godId, out GodInfo godInfo))
                {
                    EnqueueMessage(godInfo.godSprite, messageText, godInfo.godName);
                }
                else
                {
                    Debug.LogWarning($"God with ID '{godId}' not found in the dictionary.");
                }
            });
        }


        private void EnqueueMessage(Sprite godSprite, string dialogue, string title)
        {
            messageQueue.Enqueue(new GodMessage(godSprite, dialogue, title));
        }
        
        private IEnumerator ProcessMessageQueue()
        {
            while (true)
            {
                if (messageQueue.Count > 0 && !isDisplayingMessage)
                {
                    GodMessage nextMessage = messageQueue.Dequeue();

                    DisplayDialogue(nextMessage.godSprite, nextMessage.dialogue, nextMessage.title);
                    isDisplayingMessage = true;

                    yield return new WaitForSeconds(20f);

                    isDisplayingMessage = false;
                }

                // Continue processing messages in the next frame
                yield return null;
            }
        }

     
        public void DisplayDialogue(Sprite newGodSprite, string newDialogue, string newTitle)
        {
            dialogueCanvas.SetActive(true);

            if (newGodSprite != null)
            {
                godImage.sprite = newGodSprite;
            }

            dialogueText.text = !string.IsNullOrEmpty(newDialogue) ? newDialogue : "";
            dialogueTitle.text = !string.IsNullOrEmpty(newTitle) ? newTitle : "";

            if (hideCanvasCoroutine != null)
            {
                StopCoroutine(hideCanvasCoroutine);
            }

            // Start the coroutine to hide the canvas after the specified delay
            hideCanvasCoroutine = StartCoroutine(HideCanvasAfterDelay(20f));
        }

        private IEnumerator HideCanvasAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (dialogueCanvas != null && messageQueue.Count == 0)
            {
                dialogueCanvas.SetActive(false);
            }
        }
    }
}
