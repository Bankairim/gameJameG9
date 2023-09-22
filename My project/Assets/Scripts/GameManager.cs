using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public enum e_GameState
{
    MENU,
    PLAY,
    PAUSE,
    VICTORY,
    GAMEOVER
}

public class GameManager : MonoBehaviour
{

    #region Game Manager Singleton
    public static GameManager Instance { get; private set; }
    #endregion

    #region Game Manager Variables
    public GameObject mainCamera;
    public e_GameState m_State;
    public GameObject pause;
    public GameObject gameOver;
    public GameObject victory;
    public GameObject restart;
    public GameObject play;
    public GameObject HeroKnight;

    #endregion


    void Awake() {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("Pause");
        restart = GameObject.FindGameObjectWithTag("Restart");
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        victory = GameObject.FindGameObjectWithTag("Victory");
        play = GameObject.FindGameObjectWithTag("Play");
    }

    private void Update()
    {
        switch (m_State)
        {
            case e_GameState.MENU:
                break;
            case e_GameState.PLAY:
                play.SetActive(true);
                pause.SetActive(false);
                victory.SetActive(false);
                gameOver.SetActive(false);
                SetState(e_GameState.PLAY);
                break;
            case e_GameState.PAUSE:
                pause.SetActive(true);
                pause.SetActive(false);
                play.SetActive(false);
                gameOver.SetActive(false);
                SetState(e_GameState.PAUSE);
                break;
            case e_GameState.VICTORY:
                pause.SetActive(false);
                play.SetActive(false);
                gameOver.SetActive(false);
                victory.SetActive(true);
                SetState(e_GameState.VICTORY);
                break;
            case e_GameState.GAMEOVER:
                gameOver.SetActive(true);
                pause.SetActive(false);
                victory.SetActive(false);
                play.SetActive(false);
                SetState(e_GameState.GAMEOVER);
                break;
            default:
                break;
        }
    }

    public void SetState(e_GameState newState)
    {
        m_State = newState;
    }

    public e_GameState GetState(){
        return m_State;
    }
}
