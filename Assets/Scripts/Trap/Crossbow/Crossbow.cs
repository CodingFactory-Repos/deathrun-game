using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;  // Point d'où la flèche sera tirée
    public Vector3 arrowDirection = Vector3.right;  // Direction de la flèche
    private Animator animator;

    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartShooting());
    }

    IEnumerator StartShooting()
    {
        while (true)
        {
            PlayShootSound();
            yield return new WaitForSeconds(1f);
        
            animator.SetTrigger("Shoot");
        }
    }

    public void FireArrow()
    {
        // Calculer l'angle entre la direction de la flèche et l'axe X positif
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg - 90f;

        // Instancier la flèche avec la rotation correcte
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
    }

    private void PlayShootSound()
    {
        if (audioSource != null && audioSource.clip != null){
            audioSource.Play();
        }
    }
}
