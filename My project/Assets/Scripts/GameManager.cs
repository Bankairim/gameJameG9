using UnityEngine;
using UnityEngine.SceneManagement;

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
    public e_GameState m_State;
    public GameObject pause;
    public GameObject gameOver;
    public GameObject victory;
    public GameObject menu;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        m_State = e_GameState.MENU;
    }

    private void Update()
    {
        if ((m_State == e_GameState.VICTORY || m_State == e_GameState.GAMEOVER) && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        switch (m_State)
        {
            case e_GameState.MENU:
                pause.SetActive(false);
                victory.SetActive(false);
                gameOver.SetActive(false);
                menu.SetActive(true);
                break;
            case e_GameState.PLAY:
                pause.SetActive(false);
                victory.SetActive(false);
                gameOver.SetActive(false);
                menu.SetActive(false);
                SetState(e_GameState.PLAY);
                break;
            case e_GameState.PAUSE:
                pause.SetActive(true);
                victory.SetActive(false);
                menu.SetActive(false);
                gameOver.SetActive(false);
                SetState(e_GameState.PAUSE);
                break;
            case e_GameState.VICTORY:
                pause.SetActive(false);
                gameOver.SetActive(false);
                menu.SetActive(false);
                victory.SetActive(true);
                SetState(e_GameState.VICTORY);
                break;
            case e_GameState.GAMEOVER:
                gameOver.SetActive(true);
                pause.SetActive(false);
                victory.SetActive(false);
                menu.SetActive(false);
                SetState(e_GameState.GAMEOVER);
                break;
            default:
                break;
        }

        if (Input.anyKey && m_State == e_GameState.MENU)
        {
            m_State = e_GameState.PLAY;
        } 
    }

    public void SetState(e_GameState newState)
    {
        m_State = newState;
    }

    public e_GameState GetState(){
        return m_State;
    }

    public void Jouer()
    {
        if (m_State != e_GameState.PLAY)
        {
            m_State = e_GameState.PLAY;
        }

        else if (m_State == e_GameState.PLAY)
        {
            m_State = e_GameState.PAUSE;
        }
    }
}
