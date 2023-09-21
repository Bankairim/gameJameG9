using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    public float vitessePoursuite = 1.0f; // Vitesse de poursuite
    public float distanceDetection = 8.0f; // Distance à partir de laquelle l'ennemi détecte le joueur
    public float distanceAttaque = 5.0f; // Distance à partir de laquelle l'ennemi attaque le joueur

    private bool estEnPoursuite = false;

    public float m_speed = 2.0f;

    // [SerializeField] public float attackBanditDistance = 2.5f;

    // [SerializeField] public float banditDetectionDistance = 5;

    [SerializeField] public EnemyManager banditManager;

    [SerializeField] bool banditAttack = true;
    [SerializeField] Vector3 initialPosition;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;

    Transform player;

    private bool m_grounded = true;
    // private bool m_combatIdle = false;
    // private bool m_isDead = false;


    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        // m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        // initialPosition = transform.position;
        player = GameObject.Find("HeroKnight").transform;
        // joueur = GameObject.FindGameObjectWithTag("Player")[0].transform; // Assurez-vous que le tag "Player" est défini pour le joueur dans Unity
    }
    
    // Update is called once per frame
    void Update () {

        Debug.Log(banditAttack);

        if (m_body2d.velocity.x >= 0){
            if (!m_animator.GetBool("running")) {
                m_animator.SetBool("running", true);
                m_animator.SetTrigger("run");
            }
        } else {
            m_animator.SetBool("running", false);
        }

        if (banditManager.Enemylives > 0)
        {
            float distanceJoueur = Vector2.Distance(transform.position, player.position);

            // Si le joueur est dans la zone de détection
            if (distanceJoueur < distanceDetection)
            {
                estEnPoursuite = true;

                // Déplacez l'ennemi vers le joueur
                transform.position = Vector2.MoveTowards(transform.position, player.position, vitessePoursuite * Time.deltaTime);

                // Si le joueur est dans la zone d'attaque
                if (distanceJoueur <= distanceAttaque && banditAttack)
                {
                    banditAttack = false;
                    Attack();
                }
            }
            else
            {
                estEnPoursuite = false;
            }

            float distanceGame = player.position.x - transform.position.x;

            if (distanceGame > 0)
            {
                Debug.Log("gauche");
                // m_body2d.velocity = new Vector2(2, m_body2d.velocity.y);
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (distanceGame < 0)
            {
                Debug.Log("droite");
                // m_body2d.velocity = new Vector2(1, m_body2d.velocity.y);
                // Le joueur est à gauche de l'ennemi, donc l'ennemi regarde vers la gauche
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("touché");
        }
    }

    void Attack()
    {
        banditAttack = false;
        GetComponent<Animator>().SetTrigger("attack");
        StartCoroutine("AttackCoroutine");
    }

    IEnumerator AttackCoroutine()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(10);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(10);
        banditAttack = true;
    }

}

