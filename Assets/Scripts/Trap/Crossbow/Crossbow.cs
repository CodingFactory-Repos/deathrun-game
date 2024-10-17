using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab;  // Référence au prefab de la flèche
    public Transform firePoint;  // Point d'où la flèche sera tirée
    public Vector3 arrowDirection = Vector3.right;  // Direction de la flèche (1 ou -1 en x, puis 1 ou -1 en Y, globalement droite gauche haut bas)
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
            yield return new WaitForSeconds(1f);  // Intervalle entre chaque animation
        }
    }

    public void FireArrow()
    {
        // Instancier la flèche au firePoint
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        // Configurer la direction de la flèche
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.direction = arrowDirection;
    }
}
