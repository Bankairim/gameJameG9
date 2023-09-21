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

    // private void OnDrawGizmosSelected(){
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, banditDetectionDistance);
    // }
    
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

        // if (banditManager.Enemylives > 0)
        // {
        //     float distanceBetweenPlayerBandit = Vector3.Distance(transform.position, player.position);
        
        //     if((distanceBetweenPlayerBandit < banditDetectionDistance) &&
        //         distanceBetweenPlayerBandit > attackBanditDistance)
        //     {
        //         agent.destination = player.position;
        //     }
        //     if((distanceBetweenPlayerBandit <= attackBanditDistance) && banditAttack) 
        //     {
        //         banditAttack = false;
        //         GetComponent<Animator>().SetTrigger("attack");
        //         StartCoroutine("AttackCoroutine");
        //     }

        //     if (distanceBetweenPlayerBandit > banditDetectionDistance) {
        //         agent.destination = initialPosition;
        //     }
        //     Vector2 currentPosition = transform.position;

        //     // Utilisez MoveTowards pour obtenir la nouvelle position
        //     Vector2 newPosition = Vector2.MoveTowards(currentPosition, player.position, speed * Time.deltaTime);

        //     // Appliquez la nouvelle position à l'ennemi
        //     transform.position = newPosition;
        //     Debug.Log(newPosition);
        // }
        

        // //Vérifier si le personnage vient de toucher le sol
        // if (!m_grounded && m_groundSensor.State()) {
        //     m_grounded = true;
        //     m_animator.SetBool("Grounded", m_grounded);
        // }

        // // // // Vérifier si le personnage vient de commencer à tomber
        // if(m_grounded && !m_groundSensor.State()) {
        //     m_grounded = false;
        //     m_animator.SetBool("Grounded", m_grounded);
        // }

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
       


        // // -- Gérer l'entrée et le mouvement --
        // float inputX = Input.GetAxis("Horizontal");

        // // // Changer la direction du sprite en fonction de la direction de la marche
        // if (inputX > 0)
        //     transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        // else if (inputX < 0)
        //     transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // // Déplacer
        // m_body2d.velocity = new Vector2(2 * m_speed, m_body2d.velocity.y);

        // // Définir AirSpeed dans l'animator
        // m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // // -- Gérer les animations --
        // // Mort
        // if (Input.GetKeyDown("e")) {
        //     if (!m_isDead)
        //         m_animator.SetTrigger("Death");
        //     else
        //         m_animator.SetTrigger("Recover");

        //     m_isDead = !m_isDead;
        // }
            
        // // Blessure
        // else if (Input.GetKeyDown("q"))
        //     m_animator.SetTrigger("Hurt");

        // // Attaque
        // else if (Input.GetMouseButtonDown(0)) {
        //     m_animator.SetTrigger("Attack");
        // }

        // // Changer entre l'attente et l'attente de combat
        // else if (Input.GetKeyDown("f"))
        //     m_combatIdle = !m_combatIdle;

        // Sauter
        // else if (Input.GetKeyDown("space") && m_grounded) {
        //     m_animator.SetTrigger("Jump");
        //     m_grounded = false;
        //     m_animator.SetBool("Grounded", m_grounded);
        //     m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        //     m_groundSensor.Disable(0.2f);
        // }

        // Courir
        // else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        //     m_animator.SetInteger("AnimState", 2);

        // // Attente de combat
        // else if (m_combatIdle)
        //     m_animator.SetInteger("AnimState", 1);

        // // Attente
        // else
        //     m_animator.SetInteger("AnimState", 0);
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

