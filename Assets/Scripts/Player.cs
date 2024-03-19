using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public delegate void WeaponSwitched(string type);
    public event WeaponSwitched OnWeaponSwitched;
    public delegate void WeaponShot(int currentAmmo, int maxAmmo);
    public event WeaponShot OnWeaponShot;

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
    private CapsuleCollider m_collider;
    public CapsuleCollider Collider
    {
        get
        {
            if(m_collider == null)
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
            if(m_animator == null)
            {
                m_animator = GetComponent<Animator>();
            }
            return m_animator;
        }
    }
    private StateMachine m_stateMachine = new StateMachine();
    private WeaponData m_currentWeaponData;

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
    public IWeapon CurrentWeapon { get { return m_currentWeapon; } }
    private string m_weaponType;

    private bool m_playerActive = true;

    private void Awake()
    {
        SetupStateMachine();

        Assert.IsNotNull<IA_Player>(IAPlayer);
        //Subscribe local functions to events managed by Input Actions
        IAPlayer.BasicMovement.Enable();
        IAPlayer.UIMap.Enable();
        IAPlayer.BasicMovement.Move.started += Move;
        IAPlayer.BasicMovement.Move.canceled += Move;
        IAPlayer.BasicMovement.Shoot.started += Shoot;
        IAPlayer.BasicMovement.Shoot.canceled += Shoot;
        IAPlayer.BasicMovement.Pickup.started += PickupItem;
        IAPlayer.BasicMovement.Drop.started += DropWeapon;
        IAPlayer.UIMap.Pause.started += PauseGame;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_playerActive)
        {
            HandleAim();

            if (m_isShooting)
            {
                if(OnWeaponShot != null)
                {
                    OnWeaponShot.Invoke(m_currentWeapon.BulletCount, m_currentWeaponData.maxAmmo);
                }
                
                if (m_currentWeapon.Attack() && m_weaponType.Equals("Melee"))
                {
                    Animator.ResetTrigger("MeleeAttack");
                    Animator.SetTrigger("MeleeAttack");
                }
            }

            m_stateMachine.CurrentState.OnExecuteState();
        }
    }

    private void Move(InputAction.CallbackContext context)
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

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameManager.Instance.Paused = !GameManager.Instance.Paused;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            m_isShooting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {

            m_isShooting = false;

        }
    }

    private void PickupItem(InputAction.CallbackContext context)
    {
        if(m_pickupsNearby.Count != 0)
        {
            GameObject closestObject = m_pickupsNearby[0];
            if(closestObject != null)
            {
                foreach (GameObject pickup in m_pickupsNearby)
                {
                    if (Vector3.Distance(closestObject.transform.position, transform.position) >
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
    }

    private void DropWeapon(InputAction.CallbackContext context)
    {
        if(!m_currentWeaponData.type.Equals("Melee"))
        {
            if(m_currentWeapon.BulletCount > 0)
            {
                PickupFactory.Instance.SpawnPickup(transform.position, m_currentWeaponData, m_currentWeapon.BulletCount);
                Debug.Log("Weapon dropped with " + m_currentWeapon.BulletCount);
            }
            SwitchWeapon(m_defaultWeapon, m_defaultWeapon.maxAmmo);
        }
    }

    public void SwitchWeapon(WeaponData weaponData, int currentAmmo)
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

        m_weaponType = weaponData.type;
        m_currentWeapon.WeaponRoot = weaponPrefab;
        m_currentWeapon.RotationAngle = transform.rotation;
        m_currentWeapon.BulletTag = "Allied";
        m_currentWeapon.BottomlessClip = false;
        m_currentWeaponData = weaponData;
        m_currentWeapon.SetWeaponData(weaponData, currentAmmo);

        if(OnWeaponSwitched != null)
        {
            OnWeaponSwitched.Invoke(weaponData.type);
        }
        if(OnWeaponShot != null)
        {
            OnWeaponShot?.Invoke(currentAmmo, weaponData.maxAmmo);
        }
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

    public void SetupPlayer()
    {
        m_currentWeaponData = m_defaultWeapon;
        EnableRagdoll(false);
        SwitchWeapon(m_defaultWeapon, m_defaultWeapon.maxAmmo);
    }
    public void PausePlayer()
    {
        m_playerActive = false;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        IAPlayer.BasicMovement.Disable();
        if (!Dead)
        {
            Animator.speed = 0f;
        }
    }

    public void ResumePlayer()
    {
        m_playerActive = true;
        IAPlayer.BasicMovement.Enable();
        if (!Dead)
        {
            Animator.speed = 1.0f;
        }
    }
}