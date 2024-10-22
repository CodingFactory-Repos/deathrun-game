using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public string selectedScene;

   private SocketIOUnity clientSocket;


   void Start()
   {
      clientSocket = SocketManager.Instance.ClientSocket;
   }

   public void QuitGame()
   {

      Application.Quit();
   }

   public async void StartRoom()
   {
      await clientSocket.EmitAsync("rooms:create");

      SceneManager.LoadScene(selectedScene);
   }
}
