using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 0.3f;  
    public Vector3 direction = Vector3.right;  // Direction de déplacement de la flèche
    public float lifeTime = 5f;  // Durée de vie de la flèche (si elle ne rencontre aucun obstacle)

    void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    //TODO Fixe le système pour les mur
    void OnCollisionEnter2D(Collision2D collision)
    {
        UnityEngine.Debug.Log("Collision détectée avec : " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
