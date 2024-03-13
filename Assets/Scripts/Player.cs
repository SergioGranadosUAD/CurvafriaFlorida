using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
    [SerializeField] private WeaponData m_defaultWeapon;
    
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

    private bool isMoving = false;
    public bool Moving { get { return isMoving; } }
    private bool isDead = false;
    public bool Dead { get { return isDead; } }
    private bool godMode = false;
    public bool GodMode { get { return godMode; } set { godMode = value; } }
    private bool isTargetable = true;
    public bool Targetable { get { return isTargetable; } set { isTargetable = value; } }

    private bool m_isShooting = false;
   

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

    private List<GameObject> m_pickupsNearby = new List<GameObject>();
    public List<GameObject> PickupsNearby { get {  return m_pickupsNearby; } }
    private IWeapon m_currentWeapon;

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
        IAPlayer.BasicMovement.Shoot.canceled += Shoot;
        IAPlayer.BasicMovement.Pickup.started += PickupItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchWeapon(m_defaultWeapon, m_defaultWeapon.maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAim();

        if(m_isShooting)
        {
            m_currentWeapon.Attack();
        }

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
        if (context.phase == InputActionPhase.Started)
        {
            m_isShooting = true;
            Animator.ResetTrigger("MeleeAttack");
            Animator.SetTrigger("MeleeAttack");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {

            m_isShooting = false;

        }
    }

    public void PickupItem(InputAction.CallbackContext context)
    {
        if(m_pickupsNearby.Count != 0)
        {
            GameObject closestObject = m_pickupsNearby[0];
            foreach(GameObject pickup in  m_pickupsNearby)
            {
                if(Vector3.Distance(closestObject.transform.position, transform.position) >
                   Vector3.Distance(pickup.transform.position, transform.position))
                {
                    closestObject = pickup;
                }
            }

            Pickup weaponRef = closestObject.GetComponent<Pickup>();
            SwitchWeapon(weaponRef.WeaponData, weaponRef.CurrentAmmo);
            m_pickupsNearby.Remove(closestObject);
            GameObject.Destroy(closestObject);
        }
    }

    private void SwitchWeapon(WeaponData weaponData, int currentAmmo)
    {
        Debug.Log("Weapon loaded with: " + currentAmmo);
        if (m_currentWeapon != null)
        {
            Destroy(m_currentWeapon as Component);
        }

        if (weaponData.type.Equals("Melee"))
        {
            Animator.SetInteger("WeaponEquipped", 2);
            m_currentWeapon = transform.AddComponent<Melee>() as IWeapon;
        }
        else if(weaponData.type.Equals("Pistol"))
        {
            Animator.SetInteger("WeaponEquipped", 0);
            m_currentWeapon = transform.AddComponent<Pistol>() as IWeapon;
        }
        else if(weaponData.type.Equals("Rifle"))
        {
            Animator.SetInteger("WeaponEquipped", 1);
            m_currentWeapon = transform.AddComponent<Rifle>() as IWeapon;
        }
        else if(weaponData.type.Equals("Shotgun"))
        {
            Animator.SetInteger("WeaponEquipped", 1);
            m_currentWeapon = transform.AddComponent<Shotgun>() as IWeapon;
        }

        m_currentWeapon.ProjectilePrefab = m_projectilePrefab;
        m_currentWeapon.WeaponRoot = weaponPrefab;
        m_currentWeapon.RotationAngle = transform.rotation;
        m_currentWeapon.BulletTag = "Allied";
        m_currentWeapon.SetWeaponData(weaponData, currentAmmo);
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

    public void DamagePlayer()
    {
        if(!Dead && !GodMode)
        {
            isDead = true;
        }
    }
}