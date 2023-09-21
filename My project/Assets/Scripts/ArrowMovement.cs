using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float verticalSpeed = 2.0f; // Adjust this to control the vertical speed of the arrow
    public float horizontalSpeed = 2.0f; // Adjust this to control the horizontal speed of the arrow

    private void Update()
    {
        // Calculate the vertical movement
        float verticalMovement = Mathf.Sin(Time.time * verticalSpeed);

        // Calculate the horizontal movement
        float horizontalMovement = Time.time * horizontalSpeed;

        // Set the arrow's position
        transform.position = new Vector3(0, 1, 0);
    }

}
