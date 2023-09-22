using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowManager : MonoBehaviour
{
    public static float GLITCH_SPEED = 2.5f;

    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] GameObject ennemyPrefab;
    [SerializeField] GameObject toGlitch;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject musicAudio;
    [SerializeField] float      coolDown;
    [SerializeField] float      timeToIncrement = 15000;
    [SerializeField] float      speed = 0.1f;
    [SerializeField] int        level = 1;

    private float               timer = 0;
    private List<GameObject>    arrows = new List<GameObject>();
    private List<GameObject>    rocks = new List<GameObject>();
    private GameObject          sword = null;
    private bool                m_hardcore = false;

    public static ArrowManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    void FixedUpdate() {
        if (GameManager.Instance.GetState() != e_GameState.PLAY) return;

        if (sword != null && sword.IsDestroyed() == true)
        {
            sword = null;
        }

        if (timer % coolDown == 0) {
            var arrowsPos = new Vector2(Random.Range(-15f, -8f), Random.Range(-1.75f, 1f));
            // Special glitched sword
            if (level >= 3 && Random.value < 0.25 && sword == null)
            {
                var swordPos = swordPrefab.transform.position;
                swordPos.x = -10f;
                sword = Instantiate(swordPrefab, swordPos, Quaternion.identity);
                var swordCollider = sword.AddComponent<BoxCollider2D>();
                swordCollider.tag = "GlitchedSword";
                swordCollider.isTrigger = false;
                swordCollider.size = new Vector2(1f, 0.2f);

                //TODO: Adding a damage collider to the glitched sword unglitches it.
                //var swordHitCollider = sword.AddComponent<CircleCollider2D>();
                //swordHitCollider.isTrigger = true;
                //swordHitCollider.offset = new Vector2(0.55f, -0.05f);
                //swordHitCollider.radius = 0.045f;
                //swordHitCollider.tag = "Ennemy";
            }

            var arrowToSpawnCount = Random.value * level;
            var rockToSpawnCount = Random.value * (level / 2);

            for (int i = 0; i < arrowToSpawnCount*2; i++)
            {
                var randomPos = spawn.transform.position;
                randomPos.y = arrowsPos.y + Random.Range(-0.5f, 0.5f);
                randomPos.x = arrowsPos.x + Random.Range(-0.5f, 0.5f);

                var newArrow = Instantiate(arrowPrefab, randomPos, Quaternion.identity);
                newArrow.tag = "Ennemy";
                var scale = new Vector3(1, 1, 1) * Random.Range(0.5f, 1f);

                newArrow.transform.localScale = scale;

                var collider = newArrow.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0f, 0f);
                collider.size = new Vector2(1f * scale.x, 0.15f);
                collider.isTrigger = true;

                arrows.Add(newArrow);
            }

            for (int i = 0; i < rockToSpawnCount; i++)
            {
                var randomPos = spawn.transform.position;
                randomPos.y = 2.25f;
                randomPos.x = Random.Range(-16f, -7f);

                var newRock = Instantiate(rockPrefab, randomPos, Quaternion.identity);
                newRock.tag = "Ennemy";
                var scale = new Vector3(1, 1, 1) * Random.Range(0.4f, 0.8f);
                newRock.transform.localScale = scale;

                var rb = newRock.AddComponent<Rigidbody2D>();
                rb.mass = scale.x / 100;
                rb.velocity = new Vector3(speed, speed, 0);

                var collider = newRock.AddComponent<CircleCollider2D>();
                collider.offset = new Vector2(0.07f, 0f);
                collider.radius = 0.45f;
                collider.isTrigger = true;    

                rocks.Add(newRock);
            }
        }
        timer += Time.deltaTime * 1000;

        if (timer != 0 && timer % timeToIncrement == 0)
        {
            level += 2;
            speed += 2;
            musicAudio.GetComponent<AudioSource>().pitch += 0.05f;

            if (level % 4 == 0)
            {
                Instantiate(ennemyPrefab, new Vector3(-8.69f, -1.7f, 0.61f), Quaternion.identity);
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.GetState() != e_GameState.PLAY) return;

        arrows = arrows.Where(a => !a.IsDestroyed()).ToList();
        rocks = rocks.Where(r => !r.IsDestroyed()).ToList();

        foreach (GameObject a in arrows)
        {
            a.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (a.transform.position.x >= wall.transform.position.x)
            {
                Destroy(a);
            }
        };

        foreach (GameObject r in rocks)
        {
            r.transform.Translate(speed * Time.deltaTime, 0, 0);
            r.transform.Rotate(0, 0, -2f, Space.Self);

            if (r.transform.position.x >= wall.transform.position.x || r.transform.position.y <= wall.transform.position.y)
            {
                Destroy(r);
            }
        };

        if (sword != null)
        {
            sword.transform.Translate(GLITCH_SPEED * Time.deltaTime, 0, 0);

            if (sword.transform.position.x >= wall.transform.position.x + 5)
            {
                Destroy(sword);
                sword = null;
            }
        }
    }

    public void ToggleHardcoreMode()
    {
        if (!m_hardcore)
        {
            m_hardcore = true;
            level = 8;
            speed = 6;
            coolDown = 750;
        }
        else
        {
            m_hardcore = false;
            level = 1;
            speed = 2;
            coolDown = 1500;
        }
    }
}
