using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBg : MonoBehaviour
{
    float spriteSize;
    public float BackgroundSpeed = 0.1f;
    public string direction = "left";

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
        switch (direction)
    {
        case "right":
            if (backgroundSprite.transform.position.x >= startPosition.x + spriteSize)
                backgroundSprite.transform.position += Vector3.left * spriteSize;
            backgroundSprite.transform.position += Vector3.right * BackgroundSpeed * Time.deltaTime;
            break;

        case "left":
            if (backgroundSprite.transform.position.x <= startPosition.x - spriteSize)
                backgroundSprite.transform.position += Vector3.right * spriteSize;
            backgroundSprite.transform.position += Vector3.left * BackgroundSpeed * Time.deltaTime;
            break;

        case "up":
            if (backgroundSprite.transform.position.y >= startPosition.y + spriteSize)
                backgroundSprite.transform.position += Vector3.down * spriteSize;
            backgroundSprite.transform.position += Vector3.up * BackgroundSpeed * Time.deltaTime;
            break;

        case "down":
            if (backgroundSprite.transform.position.y <= startPosition.y - spriteSize)
                backgroundSprite.transform.position += Vector3.up * spriteSize;
            backgroundSprite.transform.position += Vector3.down * BackgroundSpeed * Time.deltaTime;
            break;

        case "upright":
            if (backgroundSprite.transform.position.x >= startPosition.x + spriteSize)
                backgroundSprite.transform.position += Vector3.left * spriteSize;
            if (backgroundSprite.transform.position.y >= startPosition.y + spriteSize)
                backgroundSprite.transform.position += Vector3.down * spriteSize;
            backgroundSprite.transform.position += (Vector3.up + Vector3.right) * BackgroundSpeed * Time.deltaTime;
            break;

        case "upleft":
            if (backgroundSprite.transform.position.x <= startPosition.x - spriteSize)
                backgroundSprite.transform.position += Vector3.right * spriteSize;
            if (backgroundSprite.transform.position.y >= startPosition.y + spriteSize)
                backgroundSprite.transform.position += Vector3.down * spriteSize;
            backgroundSprite.transform.position += (Vector3.up + Vector3.left) * BackgroundSpeed * Time.deltaTime;
            break;

        case "downright":
            if (backgroundSprite.transform.position.x >= startPosition.x + spriteSize)
                backgroundSprite.transform.position += Vector3.left * spriteSize;
            if (backgroundSprite.transform.position.y <= startPosition.y - spriteSize)
                backgroundSprite.transform.position += Vector3.up * spriteSize;
            backgroundSprite.transform.position += (Vector3.down + Vector3.right) * BackgroundSpeed * Time.deltaTime;
            break;

        case "downleft":
            if (backgroundSprite.transform.position.x <= startPosition.x - spriteSize)
                backgroundSprite.transform.position += Vector3.right * spriteSize;
            if (backgroundSprite.transform.position.y <= startPosition.y - spriteSize)
                backgroundSprite.transform.position += Vector3.up * spriteSize;
            backgroundSprite.transform.position += (Vector3.down + Vector3.left) * BackgroundSpeed * Time.deltaTime;
            break;

        default:
            Debug.LogWarning("Direction not recognized: " + direction);
            break;
    }
    
    // Si le fond d'écran est complètement hors de la vue à gauche, repositionnez-le à droite
        
    }
}
