using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;  // Points de vie maximum
    private int currentHealth;

    public GameObject heartPrefab;  
    public Transform healthBar;  
    private List<GameObject> hearts = new List<GameObject>(); 

    void Start()
    {
        currentHealth = maxHealth;  
        UpdateHealthBar(); 
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;  
        }
        UpdateHealthBar();  
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

}
