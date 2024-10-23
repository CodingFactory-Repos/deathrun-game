using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_Trap : MonoBehaviour
{
    bool playerInTrap = false;
    Animator m_Animator;
    GameObject player; 
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTrap = true;
            m_Animator.SetBool("IsTouch", true); 
            player = collision.gameObject; 
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTrap = false;
        }
    }

    public void TakeTrapDamage()
    {
        Debug.Log(playerInTrap);
        if (playerInTrap) 
        {
            player.GetComponent<PlayerHealth>().TakeDamage(1); 
        }
    }
}
