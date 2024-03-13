using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject patrolPath;
    [SerializeField] private float idleTimer = 15;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private WeaponData m_defaultWeapon;

    private StateMachine m_stateMachine = new StateMachine();

    public GameObject PatrolPath {  get { return patrolPath; } }
    private bool m_playerDetected = false;
    public bool PlayerDetected {  get { return m_playerDetected; } }
    public float IdleTimer { get { return idleTimer; } }
    private string m_type;
    public string Type { get { return m_type; } }
    private float m_speed;
    public float Speed {  get { return m_speed; } }
    private float m_detectionRange;
    public float DetectionRange {  get { return m_detectionRange; } }

    private bool isMoving = false;
    public bool Moving { get { return isMoving; } }
    private bool isDead = false;
    public bool Dead { get { return isDead; } }
    public Vector3 Position { get { return transform.position; } }

    private Rigidbody m_rigidBody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (m_rigidBody == null)
            {
                m_rigidBody = GetComponent<Rigidbody>();
            }
            return m_rigidBody;
        }
    }
    private Animator m_animator;
    public Animator Animator
    {
        get
        {
            if (m_animator == null)
            {
                m_animator = GetComponent<Animator>();
            }
            return m_animator;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.CurrentState.OnExecuteState();

        if(!PlayerDetected && !Dead && GameManager.Instance.Player.Targetable)
        {
            CheckPlayerDistance();
        }
    }

    public void SetEnemyData(EnemyData data)
    {
        m_type = data.type;
        m_speed = data.speed;
        m_detectionRange = data.detectionRange;
    }

    private void CheckPlayerDistance()
    {
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        if (Vector3.Distance(transform.position, playerPos) <= m_detectionRange)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, playerPos, out hit))
            {
                if(hit.transform.CompareTag("Player"))
                {
                    m_playerDetected = true;
                }
            }
                
        }
    }
    private void SetupStateMachine()
    {
        m_stateMachine.Owner = gameObject;
        m_stateMachine.AddState(new AI_IdleState(), "Idle");
        m_stateMachine.AddState(new AI_PatrolState(), "Patrol");
        m_stateMachine.AddState(new AI_ChaseState(), "Chase");
        m_stateMachine.AddState(new AI_DeathState(), "Death");

        if(patrolPath == null)
        {
            m_stateMachine.ChangeState("Idle");
        }
        else
        {
            m_stateMachine.ChangeState("Patrol");
        }
        
    }

    public void DamageEnemy()
    {
        if(!Dead)
        {
            isDead = true;
        }
    }
}
