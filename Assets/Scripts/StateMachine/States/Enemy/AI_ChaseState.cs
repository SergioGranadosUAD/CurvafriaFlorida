using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class AI_ChaseState : IState
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
    private Enemy m_enemyRef;
    private Enemy EnemyRef
    {
        get
        {
            if (m_enemyRef == null)
            {
                m_enemyRef = m_owner.GetComponent<Enemy>();
            }
            return m_enemyRef;
        }
    }
    private float m_rotationThreshold = 20;
    private float m_rotationSpeed = 3;

    public void CheckStateConditions()
    {
        if (EnemyRef.Dead)
        {
            StateController.ChangeState("Death");
        }
    }

    public void OnExecuteState()
    {
        //Check if enemy is away from the minimum distance to player.
        float enemyPlayerDist = Vector3.Distance(GameManager.Instance.Player.transform.position, EnemyRef.transform.position);
        if (enemyPlayerDist > EnemyRef.MinimumDistance)
        {
            //If not, move to player
            Vector3 lookToMovement = EnemyRef.NavAgent.velocity;
            if (lookToMovement.sqrMagnitude > 0)
            {
                EnemyRef.transform.rotation = Quaternion.Slerp(EnemyRef.transform.rotation, Quaternion.LookRotation(lookToMovement), Time.deltaTime * m_rotationSpeed);
            }
            EnemyRef.NavAgent.destination = GameManager.Instance.Player.transform.position;
        }
        else
        {
            //Back off from player
            NavMeshHit hit;
            Quaternion rot;
            float startRotation = 0;
            Vector3 backPosition = EnemyRef.transform.position - (EnemyRef.transform.forward * .5f);
            bool isBlocked = NavMesh.Raycast(EnemyRef.transform.position, backPosition, out hit, NavMesh.AllAreas);
            if(isBlocked)
            {
                //I know this is pretty crude, but my brain isn't braining anymore, should fix later
                for (int i = 0; i < 9; ++i)
                {
                    //Something is blocking the way, retry at a different angle.
                    rot = Quaternion.AngleAxis(startRotation, Vector3.up);
                    backPosition = EnemyRef.transform.position - (rot * (EnemyRef.transform.forward * .5f));
                    if (NavMesh.Raycast(EnemyRef.transform.position, backPosition, out hit, NavMesh.AllAreas))
                    {
                        break;
                    }

                    rot = Quaternion.AngleAxis(-startRotation, Vector3.up);
                    backPosition = EnemyRef.transform.position - (rot * (EnemyRef.transform.forward * .5f));
                    if (NavMesh.Raycast(EnemyRef.transform.position, backPosition, out hit, NavMesh.AllAreas))
                    {
                        break;
                    }
                }
                startRotation += 10;
            }

            //Move enemy back away from player
            EnemyRef.NavAgent.destination = backPosition;
        }
        
        if(CheckPlayerVisibility())
        {
            Vector3 lookToPlayerVector = GameManager.Instance.Player.transform.position - EnemyRef.transform.position;
            EnemyRef.transform.rotation = Quaternion.Slerp(EnemyRef.transform.rotation, Quaternion.LookRotation(lookToPlayerVector), Time.deltaTime * m_rotationSpeed);
            
            if(Vector3.Angle(EnemyRef.transform.forward, lookToPlayerVector) < m_rotationThreshold && enemyPlayerDist < EnemyRef.AttackRange)
            {
                if (EnemyRef.CurrentWeapon.Attack() && EnemyRef.Type.Equals("Melee"))
                {
                    EnemyRef.Animator.ResetTrigger("MeleeAttack");
                    EnemyRef.Animator.SetTrigger("MeleeAttack");
                }
            }
        }

        CheckStateConditions();
    }

    public void OnExitState()
    {

    }

    public void OnStateEnter()
    {

    }

    private bool CheckPlayerVisibility()
    {
        Vector3 playerPos = GameManager.Instance.Player.transform.position;

        if(Vector3.Distance(playerPos, EnemyRef.transform.position) <= EnemyRef.AttackRange)
        {
            playerPos = new Vector3(playerPos.x, playerPos.y + 1, playerPos.z);
            Vector3 enemyPos = new Vector3(EnemyRef.transform.position.x, EnemyRef.transform.position.y + 1, EnemyRef.transform.position.z);
            RaycastHit hit;
            if (Physics.Linecast(enemyPos, playerPos, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}