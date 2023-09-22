using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float startTime = 90.0f; 

    private float currentTime;

    void Start()
    {
        currentTime = startTime;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    void Update()
    {
        if (GameManager.Instance.GetState() != e_GameState.PLAY) return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "00:00";
        }
        if (currentTime <= 0)
        {
            timerText.text = "00:00";
            GameManager.Instance.SetState(e_GameState.VICTORY);
        }
    }
}
