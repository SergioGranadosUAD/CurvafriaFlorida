using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return m_instance;
        }
    }
    
    private GameObject m_player;
    private Player m_playerRef;
    public Player Player { 
        get 
        {
            if (m_player == null)
            {
                m_player = GameObject.FindGameObjectWithTag("Player");
                if(m_player != null)
                {
                    m_playerRef = m_player.GetComponent<Player>();
                }
            }
            return m_playerRef; 
        } 
    }
    private GameObject m_camera;
    private CameraFollow m_cameraRef;
    public CameraFollow Camera
    {
        get
        {
            if (m_cameraRef == null)
            {
                m_camera = GameObject.FindGameObjectWithTag("CMCamera");
                m_cameraRef = m_camera.GetComponent<CameraFollow>();
            }
            return m_cameraRef;
        }
    }
    private GameObject m_winArea;
    public GameObject WinArea
    {
        get
        {
            if(m_winArea == null)
            {
                m_winArea = GameObject.FindGameObjectWithTag("WinArea");
            }
            return m_winArea;
        }
    }

    private bool m_isPaused = false;
    public bool Paused { get {  return m_isPaused; } set { m_isPaused = value; } }

    private StateMachine m_stateMachine = new StateMachine();

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetupStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.CurrentState.OnExecuteState();
    }

    private void SetupStateMachine()
    {
        m_stateMachine.Owner = gameObject;
        m_stateMachine.AddState(new GM_PauseState(), "Pause");
        m_stateMachine.AddState(new GM_SetupState(), "SetupLevel");
        m_stateMachine.AddState(new GM_GameplayState(), "Gameplay");
        m_stateMachine.AddState(new GM_GameOverState(), "GameOver");

        m_stateMachine.ChangeState("SetupLevel");
    }
}
