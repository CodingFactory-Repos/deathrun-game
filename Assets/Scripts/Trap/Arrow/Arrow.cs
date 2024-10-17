using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 0.3f;  
    public Vector3 direction = Vector3.right;  // Direction de d�placement de la fl�che
    public float lifeTime = 5f;  // Dur�e de vie de la fl�che (si elle ne rencontre aucun obstacle)

    void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    //TODO Fixe le syst�me pour les mur
    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
