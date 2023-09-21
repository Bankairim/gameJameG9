using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{
    Transform player;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private int m_lives = 3;
    private float m_timeSinceDeath = 0.0f;
    private float m_timeToDisappear = 2.0f;
    private float m_timeSinceAttack = -1f;
    private float m_attackCooldown = 1.5f;

    public float m_speed = 2.0f;
    public float vitessePoursuite = 1.0f; // Vitesse de poursuite
    public float distanceDetection = 8.0f; // Distance à partir de laquelle l'ennemi détecte le joueur
    public float distanceAttaque = 5.0f; // Distance à partir de laquelle l'ennemi attaque le joueur
    private bool m_dead = false;

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        player = GameObject.Find("HeroKnight").transform;
        m_body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_dead)
        {
            m_timeSinceDeath += Time.deltaTime;

            if (m_timeSinceDeath >= m_timeToDisappear)
            {
                Destroy(gameObject);
            }

            return;
        }

        if (m_body2d.velocity.x >= 0){
            if (!m_animator.GetBool("running")) {
                m_animator.SetBool("running", true);
                m_animator.SetTrigger("run");
            }
        } else {
            m_animator.SetBool("running", false);
        }

        if (m_lives > 0)
        {
            float distanceJoueur = Vector2.Distance(transform.position, player.position);

            // Si le joueur est dans la zone de détection
            if (distanceJoueur < distanceDetection)
            {
                // Déplacez l'ennemi vers le joueur
                transform.position = Vector2.MoveTowards(transform.position, player.position, vitessePoursuite * Time.deltaTime);

                // Si le joueur est dans la zone d'attaque
                if (distanceJoueur <= distanceAttaque && (m_timeSinceAttack <= -0.5 || m_timeSinceAttack >= m_attackCooldown))
                {
                    m_timeSinceAttack = 0f;
                    Attack();
                }
            }

            float distanceGame = player.position.x - transform.position.x;

            if (distanceGame > 0)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (distanceGame < 0)
            {
                // Le joueur est à gauche de l'ennemi, donc l'ennemi regarde vers la gauche
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            if (m_timeSinceAttack >= 0)
            {
                m_timeSinceAttack += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            if (HeroKnight.Instance.Attacking)
            {
                m_animator.SetTrigger("hitted");
                m_lives -= 1;

                if (m_lives <= 0)
                {
                    m_dead = true;
                    m_animator.SetTrigger("died");
                }
            }

        }
        else if (collision.collider.CompareTag("Player"))
        {
            if (HeroKnight.Instance.Blocking)
            {
                return;
            }
            else
            {
                HeroKnight.Instance.TakeDamage(20);
            }
        }
    }

    void Attack()
    {
        GetComponent<Animator>().SetTrigger("attack");
    }
}

