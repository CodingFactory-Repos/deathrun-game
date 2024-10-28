using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Trap : MonoBehaviour
{
    bool playerInTrap = false;
    Animator m_Animator;
    GameObject player; 

    private bool IsActive=false;

    private float damageInterval= 1.0f;

    private Coroutine damageCoroutine;



    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggered(){
       IsActive = true;
    }

    private void NotTriggered(){
        IsActive = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsActive){
                playerInTrap = true;
                player = collision.gameObject; 

                if (damageCoroutine == null){
                    damageCoroutine = StartCoroutine(ApplyDamage());
                }
            
            }
           
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTrap = false;

            if (damageCoroutine != null){
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && !playerInTrap){
             playerInTrap = true;
        }
    }

    private IEnumerator ApplyDamage(){
        while (playerInTrap){
            TakeTrapDamage();
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void TakeTrapDamage()
    {
        if (playerInTrap) 
        {
            player.GetComponent<PlayerHealth>().TakeDamage(1); 
        }
    }
}
