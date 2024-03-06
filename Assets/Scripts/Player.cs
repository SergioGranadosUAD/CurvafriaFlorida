using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Variable declaration
    [SerializeField] private float playerSpeed = 1;

    private IA_Player m_playerInputActions;
    private Rigidbody m_rigidBody;
    private Animator m_animator;
    private StateMachine m_stateMachine = new StateMachine();
    public Rigidbody Rigidbody { get { return m_rigidBody; } }

    private bool isMoving = false;
    public bool Moving { get { return isMoving; } }
    private float idleTime = 0;

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
        m_rigidBody = transform.GetChild(0).GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

        //Subscribe local functions to events managed by Input Actions
        IAPlayer.BasicMovement.Enable();
        IAPlayer.BasicMovement.Move.started += Move;
        IAPlayer.BasicMovement.Move.canceled += Move;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Move the player if its movement input value is different than zero.
        Vector2 rawMovementValue = IAPlayer.BasicMovement.Move.ReadValue<Vector2>();
        if(rawMovementValue != Vector2.zero)
        {
            m_animator.SetFloat("dirX", rawMovementValue.x);
            m_animator.SetFloat("dirY", rawMovementValue.y);
            Vector3 velocity = new Vector3(rawMovementValue.x, 0f, rawMovementValue.y);
            m_rigidBody.velocity = velocity * playerSpeed;
        }

        if(!isMoving)
        {
            idleTime += Time.deltaTime;
        }

        if(idleTime > 20)
        {
            m_animator.ResetTrigger("LongWait");
            m_animator.SetTrigger("LongWait");
            idleTime = 0;
        }
    }

    //Unused
    public void Move(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            idleTime = 0;
            m_animator.SetBool("IsMoving", true);
            isMoving = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            m_animator.SetBool("IsMoving", false);
            isMoving = false;
            Rigidbody.velocity = Vector3.zero;
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
}