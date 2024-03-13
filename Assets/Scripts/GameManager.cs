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
        //m_stateMachine.AddState(new AI_IdleState(), "MainMenu");
        //m_stateMachine.AddState(new AI_PatrolState(), "SetupLevel");
        //m_stateMachine.AddState(new AI_ChaseState(), "Gameplay");
        //m_stateMachine.AddState(new AI_DeathState(), Patrol");

        m_stateMachine.ChangeState("Setup");
    }
}
