using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] float m_health = 100;
    [SerializeField] float m_blockCooldown = 1.5f;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] GameObject m_healthbar;
    [SerializeField] BoxCollider2D m_wallCollider;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private CapsuleCollider2D   m_swordCollider = null;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_attacking = false;
    private bool                m_blocking = false;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private bool                m_doubleJump = true;
    private bool                m_isAlive = true;
    private bool                m_glitching = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_timeSinceBlock = -1f;
    private float               m_blockTime = 0.5f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_timeToShake = 0f;
    private float               m_rollCurrentTime;

    // SINGLETON
    public static HeroKnight Instance;

    public bool Attacking { get => m_attacking; }
    public bool Blocking { get => m_blocking; }

    // Use this for initialization
    void Start ()
    {
        Instance = this;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!m_isAlive) return;

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling && (m_timeSinceBlock <= -0.1f || m_timeSinceBlock >= m_blockTime)) {
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            if (m_swordCollider != null)
            {
                Destroy(m_swordCollider);
                m_swordCollider = null;
            }

            m_swordCollider = gameObject.AddComponent<CapsuleCollider2D>();
            m_swordCollider.offset = m_facingDirection == 1 ?
                new Vector2(0.71f, 0.95f) : new Vector2(-0.71f, 0.95f);
            m_swordCollider.size = new Vector2(1.55f, 1.7f);
            m_swordCollider.isTrigger = true;
            m_swordCollider.tag = "Sword";

            m_attacking = true;
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && (m_timeSinceBlock <= -0.1f || m_timeSinceBlock >= m_blockCooldown))
        {
            m_blocking = true;
            m_timeSinceBlock = 0.0001f;
            m_animator.SetTrigger("Block");
            m_body2d.velocity = new Vector2(0f, m_body2d.velocity.y);
        }

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }

        //Jump
        else if (Input.GetKeyDown("space") && !m_rolling)
        {
            if (m_grounded)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
            else if (m_doubleJump)
            {
                m_animator.SetTrigger("DoubleJump");
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_doubleJump = false;
            }

        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_blocking = false;
        }
        if (m_timeSinceAttack >= 0.5f)
        {
            m_attacking = false;

            if (m_swordCollider != null)
            {
                Destroy(m_swordCollider);
                m_swordCollider = null;
            }
        }

        if (m_wallCollider.enabled && m_glitching)
        {
            m_wallCollider.enabled = false;
        }
        else if (!m_glitching)
        {
            m_wallCollider.enabled = true;
        }

        if (m_timeSinceBlock >= 0.0f)
        {
            m_timeSinceBlock += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (m_grounded && !m_doubleJump)
        {
            m_doubleJump = true;
        }
        if (m_timeToShake >= 0.0f)
        {
            var circlePos = new Vector3(-0.41f, 0.25f, -10f) + Random.insideUnitSphere * 0.1f;
            Camera.main.transform.position = new Vector3(circlePos.x, circlePos.y, -10);
            m_timeToShake -= Time.deltaTime;

            if (m_timeToShake <= 0.0f)
            {
                Camera.main.transform.position = new Vector3(-0.41f, 0.25f, -10f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_glitching)
        {
            Debug.Log("GLITCHING");
            m_body2d.velocity = new Vector2(m_body2d.velocity.x + ArrowManager.GLITCH_SPEED, m_body2d.velocity.y);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ennemy") && m_isAlive)
        {
            if ((m_attacking || m_blocking) && transform.forward.x >= 0 && m_facingDirection == -1)
            {
                var vfxPos = new Vector3(
                    (transform.position.x + 0.85f * m_facingDirection),
                    transform.position.y + 0.8f,
                    transform.position.z
                );
                VFXManager.Instance.Create("Blocked", vfxPos);
                Destroy(collision.gameObject);
                return;
            }

            TakeDamage(10);

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("GlitchedSword"))
        {
            m_glitching = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("GlitchedSword"))
        {
            m_glitching = false;
        }
    }

    public void TakeDamage(int damage)
    {
        m_timeToShake = 0.5f;
        m_health -= damage;
        m_healthbar.GetComponent<Slider>().value = m_health;
        m_animator.SetTrigger("Hurt");

        if (m_health <= 0)
        {
            m_isAlive = false;
            m_animator.SetTrigger("Death");
        }
    }
}
