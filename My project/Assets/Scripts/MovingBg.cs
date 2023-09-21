using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBg : MonoBehaviour
{
    float spriteSize;
    public float BackgroundSpeed = 0.1f;

    public SpriteRenderer backgroundSprite;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = backgroundSprite.transform.position;
        spriteSize = backgroundSprite.bounds.size.x; 
    }

    void Update()
    {
        backgroundSprite.transform.position += Vector3.left * BackgroundSpeed * Time.deltaTime;

        // Si le fond d'écran est complètement hors de la vue à gauche, repositionnez-le à droite
        if (backgroundSprite.transform.position.x <= startPosition.x - spriteSize)
        {
            backgroundSprite.transform.position += Vector3.right * spriteSize;
        }
    }
}
