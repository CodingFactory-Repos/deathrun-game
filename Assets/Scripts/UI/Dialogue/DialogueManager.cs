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
                Debug.Log("Received message from God!");
                Debug.Log(response.ToString());

                JArray trapDataArray = JArray.Parse(response.ToString());
                JObject messageData = (JObject)trapDataArray[0][0];

                Debug.Log($"Message data: {messageData}");

                string godId = messageData["godID"].ToString();
                string messageText = messageData["message"].ToString();

                Debug.Log($"Received message from God {godId}: {messageText}");

                // Check if the god ID exists in the dictionary
                if (godPrefabDictionary.TryGetValue(godId, out GodInfo godInfo))
                {
                    Debug.Log($"God with ID '{godId}' found in the dictionary.");
                    Debug.Log($"God name: {godInfo.godName}");
                    EnqueueMessage(godInfo.godSprite, messageText, godInfo.godName);
                    Debug.Log($"Message enqueued for God {godId}");
                }
                else
                {
                    Debug.LogWarning($"God with ID '{godId}' not found in the dictionary.");
                }
            });
        }


        private void EnqueueMessage(Sprite godSprite, string dialogue, string title)
        {
            Debug.Log("Enqueuing message...");
            Debug.Log($"dialogue: {dialogue}, title: {title}");
            messageQueue.Enqueue(new GodMessage(godSprite, dialogue, title));
        }

        private IEnumerator ProcessMessageQueue()
        {
            while (true)
            {
                if (messageQueue.Count > 0 && !isDisplayingMessage)
                {
                    GodMessage nextMessage = messageQueue.Dequeue();

                    Debug.Log("Displaying message...");

                    Debug.Log($"dialogue: {nextMessage.dialogue}, title: {nextMessage.title}");

                    DisplayDialogue(nextMessage.godSprite, nextMessage.dialogue, nextMessage.title);
                    isDisplayingMessage = true;

                    yield return new WaitForSeconds(20f);

                    isDisplayingMessage = false;
                }

                yield return null; // Attendre le frame suivant
            }
        }

        public void DisplayDialogue(Sprite newGodSprite, string newDialogue, string newTitle)
        {
            if (!dialogueCanvas.activeSelf)
            {
                dialogueCanvas.SetActive(true);
            }

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
