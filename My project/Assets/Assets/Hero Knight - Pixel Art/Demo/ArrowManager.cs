using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float coolDown;
    [SerializeField] GameObject wall;


    private float timer = 0;
    private List<Arrow> arrows;

    void Start()
    {
        
    }
 
    void FixedUpdate() {
        if (timer % coolDown == 0) {
            var newArrow = new Arrow();
            newArrow.setGameObject(Instantiate(arrowPrefab, new Vector3(0, 0, 0), Quaternion.identity));
            arrows.Add(newArrow);
        }
        timer += Time.deltaTime * 1000;
        foreach (Arrow a in arrows)
        {
            a.HandleMove(0.1f);
            if (a.transform.position.x >= wall.transform.position.x) {
                Destroy(a);
            }
        };
    }
}
