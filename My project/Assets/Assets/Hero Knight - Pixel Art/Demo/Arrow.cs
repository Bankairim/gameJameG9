using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
    [SerializeField] float m_Speed;

    [SerializeField] float timer;

    public GameObject ArrowObject;
    

    Vector3 myDirection;

	// Use this for initialization
	void Start () {

	}

    public void HandleMove(float speed) {
        Vector3 vector = new Vector3(0.1f, 0, 0);
        ArrowObject.transform.Translate(vector);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("touché");
            Destroy(gameObject);
        }
        
    }

    public void setGameObject(GameObject go){
        this.ArrowObject = go;
    }
}
