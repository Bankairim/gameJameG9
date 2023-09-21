using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public int Enemylives = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Enemylives--;
            animator.SetTrigger("hitted");
            if (Enemylives <= 0)
            {
                animator.SetTrigger("died");
                Destroy(this.gameObject, 2);
            }
        }
    }
}
