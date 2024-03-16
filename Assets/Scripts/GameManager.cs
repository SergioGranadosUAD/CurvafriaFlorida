using System.Collections;
using System.Collections.Generic;
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
                m_player = GameObject.Find("Player");
                m_playerRef = m_player.GetComponent<Player>();
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
                m_camera = GameObject.Find("Virtual Camera");
                m_cameraRef = m_camera.GetComponent<CameraFollow>();
            }
            return m_cameraRef;
        }
    }

    private StateMachine m_stateMachine = new StateMachine();

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupStateMachine()
    {
        m_stateMachine.Owner = gameObject;
        //m_stateMachine.AddState(new GM_PauseState(), "Pause");
        //m_stateMachine.AddState(new GM_SetupState(), "SetupLevel");
        //m_stateMachine.AddState(new GM_GameplayState(), "Gameplay");
        //m_stateMachine.AddState(new GM_GameOverState(), GameOver");

        m_stateMachine.ChangeState("Setup");
    }
}
