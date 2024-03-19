using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float idleTimer = 15;
    [SerializeField] private GameObject weaponPrefab;
    public GameObject WeaponPrefab { get {  return weaponPrefab; } }

    private StateMachine m_stateMachine = new StateMachine();

    private GameObject m_patrolPath;
    public GameObject PatrolPath {  get { return m_patrolPath; } }
    private bool m_playerDetected = false;
    public bool PlayerDetected {  get { return m_playerDetected; } }
    public float IdleTimer { get { return idleTimer; } }
    private string m_type;
    public string Type { get { return m_type; } }
    private float m_speed;
    public float Speed {  get { return m_speed; } }
    private float m_detectionRange = 10;
    public float DetectionRange {  get { return m_detectionRange; } }
    private float m_minimumDistance = 0;
    public float MinimumDistance {  get { return m_minimumDistance; } }
    private float m_atkRange = 0;
    public float AttackRange {  get {  return m_atkRange; } }

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
    private CapsuleCollider m_collider;
    public CapsuleCollider Collider
    {
        get
        {
            if (m_collider == null)
            {
                m_collider = GetComponent<CapsuleCollider>();
            }
            return m_collider;
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
    public IWeapon CurrentWeapon { get {  return m_currentWeapon; } }
    private WeaponData m_weaponData;

    private bool m_EnemyActive = true;

    // Start is called before the first frame update
    void Start()
    {
        EnableRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Si el enemigo no se encuentra pausado, se actualiza de forma normal.
        if(m_EnemyActive)
        {
            m_stateMachine.CurrentState.OnExecuteState();
            //Revisa la distancia del jugador y si ha sido detectado.
            if (!Dead)
            {
                if (!PlayerDetected && GameManager.Instance.Player.Targetable)
                {
                    CheckPlayerDistance();
                }
            }

            //Mueve al jugador y ajusta las animaciones para reflejarlo.
            if (NavAgent.desiredVelocity != Vector3.zero)
            {
                Animator.SetBool("IsMoving", true);
                isMoving = true;
            }
            else
            {
                Animator.SetBool("IsMoving", false);
                isMoving = false;
            }

            if (isMoving)
            {
                Vector2 rawMovementValue = new Vector2(NavAgent.desiredVelocity.x, NavAgent.desiredVelocity.z);
                rawMovementValue.Normalize();
                if (rawMovementValue != Vector2.zero)
                {
                    Vector3 relativeDir = Quaternion.Euler(0f, 0f, transform.eulerAngles.y) * rawMovementValue;
                    Animator.SetFloat("dirX", relativeDir.x);
                    Animator.SetFloat("dirY", relativeDir.y);
                }
            }
        }
    }

    //Obtiene la informaci�n del enemigo actual y se la asigna.
    public void SetEnemyData(EnemyData data, GameObject patrolPath)
    {
        m_type = data.type;
        NavAgent.speed = data.speed;
        m_detectionRange = data.detectionRange;
        m_minimumDistance = data.distanceRange;
        m_atkRange = data.atkRange;
        m_patrolPath = patrolPath;

        SetupStateMachine();
    }

    //Le asigna la informaci�n al arma actual.
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

        m_currentWeapon.WeaponRoot = weaponPrefab;
        m_currentWeapon.RotationAngle = transform.rotation;
        m_currentWeapon.BulletTag = "Hostile";
        m_currentWeapon.BottomlessClip = true;
        m_weaponData = weaponData;
        m_currentWeapon.SetWeaponData(weaponData, weaponData.maxAmmo);
    }

    //Revisa si el jugador se encuentra a la distancia m�nima y si no hay obst�culos en su campo de visi�n.
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

    //Inicializa la m�quina de estados.
    private void SetupStateMachine()
    {
        m_stateMachine.Owner = gameObject;
        m_stateMachine.AddState(new AI_IdleState(), "Idle");
        m_stateMachine.AddState(new AI_PatrolState(), "Patrol");
        m_stateMachine.AddState(new AI_ChaseState(), "Chase");
        m_stateMachine.AddState(new AI_DeathState(), "Death");

        if(m_patrolPath == null)
        {
            m_stateMachine.ChangeState("Idle");
        }
        else
        {
            m_stateMachine.ChangeState("Patrol");
        }
        
    }

    //Mata al enemigo y suelta su arma.
    public void DamageEnemy()
    {
        if(!Dead)
        {
            isDead = true;
            PickupFactory.Instance.SpawnPickup(transform.position, m_weaponData, m_weaponData.maxAmmo);
        }
    }

    //Activa el modo ragdoll al morir.
    public void EnableRagdoll(bool enabled)
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = !enabled;
        }
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = enabled;
        }

        Rigidbody.isKinematic = enabled;
        Collider.enabled = !enabled;
        Animator.enabled = !enabled;
    }

    //Pausa la actualizaci�n del enemigo.
    public void PauseEnemy()
    {
        m_EnemyActive = false;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        if (!Dead)
        {
            Animator.speed = 0f;
            NavAgent.enabled = false;
        }
    }

    //Continua con la actualizaci�n del enemigo.
    public void ResumeEnemy()
    {
        m_EnemyActive = true;
        if (!Dead)
        {
            Animator.speed = 1.0f;
            NavAgent.enabled = true;
        }
    }
}
