using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Reference to Patrol Points
    public Transform[] patrolPoints;
    public int currentPatrolPoint;

    //Reference to NavMeshAgent
    public NavMeshAgent agent;

    //Reference to Animator
    public Animator anim;

    //Reference AI Variables
    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isAttacking
    };
    public AIState currentState;

    public float waitAtPoint = 2f;
    private float waitCounter;

    public float chaseRange;
    
    public float attackRange = 1f;
    public float betweenAttacks = 2f;
    private float attackCounter;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
    }

    // Update is called once per frame
    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);


        switch (currentState) {

            case AIState.isIdle:

                anim.SetBool("IsMoving", false);

                if(waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                } else
                {
                    currentState = AIState.isPatrolling;
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }

                if(distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.isChasing;
                    anim.SetBool("IsMoving", true);
                }

                break;

            case AIState.isPatrolling:

                if (agent.remainingDistance <= .2f)
                {
                    currentPatrolPoint++;

                    if (currentPatrolPoint >= (patrolPoints.Length - 1))
                    {
                        currentPatrolPoint = 0;
                    }

                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;

                }

                if (distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.isChasing;
                }

                anim.SetBool("IsMoving", true);
                break;

            case AIState.isChasing:

                agent.SetDestination(PlayerController.instance.transform.position);

                if(distanceToPlayer <= attackRange)
                {
                    currentState = AIState.isAttacking;
                    anim.SetTrigger("Attack");
                    anim.SetBool("IsMoving", false);

                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;

                    attackCounter = betweenAttacks;
                }

                if(distanceToPlayer > chaseRange)
                {
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;

                    agent.velocity = Vector3.zero;
                    agent.SetDestination(transform.position);
                }

                break;

            case AIState.isAttacking:

                transform.LookAt(PlayerController.instance.transform, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

                attackCounter -= Time.deltaTime;

                if(attackCounter <= 0)
                {

                    if(distanceToPlayer <= attackRange)
                    {
                        anim.SetTrigger("Attack");
                        attackCounter = betweenAttacks;
                    } else
                    {
                        currentState = AIState.isIdle;
                        waitCounter = waitAtPoint;

                        agent.isStopped = false;
                    }

                }


                break;
        }
    }
}
