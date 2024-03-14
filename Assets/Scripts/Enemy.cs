using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    private float m_detectionRange = 10;
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
    private NavMeshAgent m_navAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            if(m_navAgent == null)
            {
                m_navAgent = GetComponent<NavMeshAgent>();
            }
            return m_navAgent;
        }
    }
    private IWeapon m_currentWeapon;

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
        NavAgent.speed = data.speed;
        m_detectionRange = data.detectionRange;
    }

    public void SetWeaponData(WeaponData weaponData)
    {
        if (m_currentWeapon != null)
        {
            Destroy(m_currentWeapon as Component);
        }

        if (weaponData.type.Equals("Melee"))
        {
            m_currentWeapon = transform.AddComponent<Melee>() as IWeapon;
        }
        else if (weaponData.type.Equals("Pistol"))
        {
            m_currentWeapon = transform.AddComponent<Pistol>() as IWeapon;
        }
        else if (weaponData.type.Equals("Rifle"))
        {
            m_currentWeapon = transform.AddComponent<Rifle>() as IWeapon;
        }
        else if (weaponData.type.Equals("Shotgun"))
        {
            m_currentWeapon = transform.AddComponent<Shotgun>() as IWeapon;
        }

        m_currentWeapon.ProjectilePrefab = m_projectilePrefab;
        m_currentWeapon.WeaponRoot = weaponPrefab;
        m_currentWeapon.RotationAngle = transform.rotation;
        m_currentWeapon.BulletTag = "Enemy";
        m_currentWeapon.SetWeaponData(weaponData, weaponData.maxAmmo);
    }

    private void CheckPlayerDistance()
    {
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        playerPos = new Vector3(playerPos.x, playerPos.y + 1, playerPos.z);
        if (Vector3.Distance(transform.position, playerPos) <= m_detectionRange)
        {
            RaycastHit hit;
            Vector3 enemyPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Linecast(enemyPos, playerPos, out hit))
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
