using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Variable declaration
    [SerializeField] private float playerSpeed = 500;
    [SerializeField] private float idleTimer = 15;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject m_projectilePrefab;
    
    public float PlayerSpeed { get { return playerSpeed; } }
    public float IdleTimer { get { return idleTimer; } }
    private IA_Player m_playerInputActions;
    private Rigidbody m_rigidBody;
    public Rigidbody Rigidbody
    {
        get
        {
            if(m_rigidBody == null)
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
            if(m_animator == null)
            {
                m_animator = GetComponent<Animator>();
            }
            return m_animator;
        }
    }
    private StateMachine m_stateMachine = new StateMachine();
    private GameObject m_projectileSpawner;

    private bool isMoving = false;
    public bool Moving { get { return isMoving; } }
    private bool isDead = false;
    public bool Dead { get { return isDead; } }
   

        public IA_Player IAPlayer
    {
        get
        {
            if(m_playerInputActions == null)
            {
                m_playerInputActions = new IA_Player();
            }
            return m_playerInputActions;
        }
    }

    private void Awake()
    {
        SetupStateMachine();

        Assert.IsNotNull<IA_Player>(IAPlayer);
        //m_projectileSpawner = transform.Find("ProjectileSpawner").gameObject;

        //Subscribe local functions to events managed by Input Actions
        IAPlayer.BasicMovement.Enable();
        IAPlayer.BasicMovement.Move.started += Move;
        IAPlayer.BasicMovement.Move.canceled += Move;
        IAPlayer.BasicMovement.Shoot.started += Shoot;
        IAPlayer.BasicMovement.Pickup.started += PickupItem;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleAim();
        m_stateMachine.CurrentState.OnExecuteState();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            isMoving = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            
            isMoving = false;
            
        }
    }

    private void SetupStateMachine()
    {
        m_stateMachine.Owner = gameObject;
        m_stateMachine.AddState(new IdleState(), "Idle");
        m_stateMachine.AddState(new MovingState(), "Moving");
        m_stateMachine.AddState(new DeathState(), "Death");

        m_stateMachine.ChangeState("Idle");
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        ProjectileFactory.Instance.SpawnProjectile(m_projectilePrefab, weaponPrefab.transform.Find("ProjectileSpawner").transform.position, 3000, transform.rotation, "Allied");
    }

    public void PickupItem(InputAction.CallbackContext context)
    {

    }

    private void HandleAim()
    {
        Vector3 mousePos = IAPlayer.BasicMovement.Aim.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (ground.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            point = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(point);
        }
    }
}