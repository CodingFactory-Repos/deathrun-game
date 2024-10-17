using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Nécessaire pour manipuler l'UI

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;  // Points de vie maximum
    private int currentHealth;

    public GameObject heartPrefab;  // Le prefab du cœur à dupliquer dans la HealthBar
    public Transform healthBar;  // Le conteneur de la HealthBar (l'endroit où les cœurs seront ajoutés)

    private List<GameObject> hearts = new List<GameObject>();  // Liste pour stocker les cœurs actifs

    void Start()
    {
        currentHealth = maxHealth;  // Commencer avec la vie maximale
        UpdateHealthBar();  // Mettre à jour la barre de santé pour afficher les cœurs
    }

    // Méthode pour infliger des dégâts au joueur
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;  // Eviter les points de vie négatifs
        }
        UpdateHealthBar();  // Mettre à jour l'affichage des cœurs
    }

    // Méthode pour soigner le joueur
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;  // Eviter de dépasser la vie maximale
        }
        UpdateHealthBar();  // Mettre à jour l'affichage des cœurs
    }

    // Mettre à jour la barre de santé (ajouter ou supprimer des cœurs)
    void UpdateHealthBar()
    {
        // Supprimer tous les cœurs actuels
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Ajouter un cœur pour chaque point de vie actuel
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthBar);
            hearts.Add(newHeart);  // Ajouter le cœur à la liste pour le traquer
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Infliger des dégâts au joueur lorsqu'il entre en collision avec un ennemi
            GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

}
