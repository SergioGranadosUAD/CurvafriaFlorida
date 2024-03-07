using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState
{
    private StateMachine m_stateController;
    public StateMachine StateController
    {
        get { return m_stateController; }
        set { m_stateController = value; }
    }
    private GameObject m_owner;
    public GameObject Owner
    {
        get { return m_owner; }
        set { m_owner = value; }
    }
    private Player m_playerRef;
    private Player PlayerRef
    {
        get
        {
            if (m_playerRef == null)
            {
                m_playerRef = m_owner.GetComponent<Player>();
            }
            return m_playerRef;
        }
    }

    public void CheckStateConditions()
    {
        if(PlayerRef.Dead)
        {
            StateController.ChangeState("Death");
        }
        else if(!PlayerRef.Moving)
        {
            StateController.ChangeState("Idle");
        }
    }

    public void OnExecuteState()
    {
        //Move the player if its movement input value is different than zero.
        Vector2 rawMovementValue = PlayerRef.IAPlayer.BasicMovement.Move.ReadValue<Vector2>();
        if (rawMovementValue != Vector2.zero)
        {
            Vector3 relativeDir = Quaternion.Euler(0f, 0f, PlayerRef.transform.eulerAngles.y) * rawMovementValue;
            PlayerRef.Animator.SetFloat("dirX", relativeDir.x);
            PlayerRef.Animator.SetFloat("dirY", relativeDir.y);
            Vector3 velocity = new Vector3(rawMovementValue.x, 0f, rawMovementValue.y);
            PlayerRef.Rigidbody.velocity = velocity * PlayerRef.PlayerSpeed * Time.deltaTime;
        }
        CheckStateConditions();
    }

    public void OnExitState()
    {
        PlayerRef.Animator.SetBool("IsMoving", false);
        PlayerRef.Rigidbody.velocity = Vector3.zero;
    }

    public void OnStateEnter()
    {
        PlayerRef.Animator.SetBool("IsMoving", true);
    }
}