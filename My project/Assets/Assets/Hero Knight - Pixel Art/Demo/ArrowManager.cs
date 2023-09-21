using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] float coolDown;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject spawn;
    [SerializeField] float speed = 0.1f;
    [SerializeField] int level = 1;

    private float timer = 0;
    private List<GameObject> arrows = new List<GameObject>();
    private List<GameObject> rocks = new List<GameObject>();

    void FixedUpdate() {
        if (timer % coolDown == 0) {
            var arrowToSpawnCount = Random.value * level;
            var rockToSpawnCount = Random.value * (level / 2);

            for (int i = 0; i < arrowToSpawnCount; i++)
            {
                var randomPos = spawn.transform.position;
                randomPos.y = Random.Range(-2.25f, 2.25f);
                randomPos.x = Random.Range(-15f, -5f);

                var newArrow = Instantiate(arrowPrefab, randomPos, Quaternion.identity);
                newArrow.tag = "Ennemy";
                var scale = new Vector3(1, 1, 1) * Random.Range(0.5f, 1f);

                newArrow.transform.localScale = scale;

                var collider = newArrow.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0f, 0f);
                collider.size = new Vector2(1f, 0.15f);
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
    }

    private void LateUpdate()
    {
        arrows = arrows.Where(a => !a.IsDestroyed()).ToList();
        rocks = rocks.Where(r=> !r.IsDestroyed()).ToList();

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
    }
}
