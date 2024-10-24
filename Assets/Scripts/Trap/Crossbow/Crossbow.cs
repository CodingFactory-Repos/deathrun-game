using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;  // Point d'où la flèche sera tirée
    public Vector3 arrowDirection = Vector3.right;  // Direction de la flèche
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StartShooting());
    }

    IEnumerator StartShooting()
    {
        while (true)
        {
            animator.SetTrigger("Shoot");
            // Attendre que l'animation atteigne le point où la flèche doit être tirée
            yield return new WaitForSeconds(0.5f); // Ajustez en fonction du timing de votre animation
            FireArrow();
            yield return new WaitForSeconds(0.5f); // Ajustez pour correspondre à l'intervalle de tir souhaité
        }
    }

    public void FireArrow()
    {
        // Calculer l'angle entre la direction de la flèche et l'axe X positif
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg - 90f;

        // Instancier la flèche avec la rotation correcte
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
    }
}
