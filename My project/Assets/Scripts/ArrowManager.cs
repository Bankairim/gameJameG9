using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour
{
    public GameObject arrowPrefab; // Le préfab de la flèche
    public Transform spawnPoint; // Le point de départ des flèches
    public float shootInterval = 2.0f; // Intervalle de tir en secondes

    private void Start()
    {
        StartCoroutine(ShootArrowsPeriodically());
    }

    private IEnumerator ShootArrowsPeriodically()
    {
        while (true)
        {
            ShootArrow();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Applique une force vers la droite pour simuler le tir
            rb.AddForce(Vector3.right * 10f, ForceMode.Impulse);
        }
    }
}
