using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;  // Points de vie maximum
    private int currentHealth;

    public GameObject heartPrefab;  
    public Transform healthBar;  
    private List<GameObject> hearts = new List<GameObject>();

    
    private SocketIOUnity clientSocket;

    public GameObject deadCanvas;

    void Start()
    {
        
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (deadCanvas != null) deadCanvas.SetActive(false);
        clientSocket = SocketManager.Instance.ClientSocket;
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }

    private async void SocketEmitter()
    {
        await clientSocket.EmitAsync("rooms:end");
    }

    void Die()
    {
        SocketEmitter();
        if (deadCanvas != null)
        {
            deadCanvas.SetActive(true);
        }
        GetComponent<PlayerMovement>().enabled = false;
    }


    void UpdateHealthBar()
    {
       
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

      
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthBar);
            hearts.Add(newHeart);  // Ajouter le c�ur � la liste pour le traquer
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

      public void ReturnToMenu()
    {
        GameObject socket = GameObject.FindWithTag("Temporary");
        if (socket != null) Destroy(socket.gameObject);
        SceneManager.LoadScene("Menu");
    }

}
