using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;


public class UnitWalk : StateMachineBehaviour
{

    private AttackController attack;
    private NavMeshAgent agent;
    private UnitController unit;
    
     
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack = animator.GetComponent<AttackController>();
        agent = animator.GetComponent<NavMeshAgent>();
        unit = animator.GetComponent<UnitController>();
       
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (unit.GetHp()<=0)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isDie", true);
         
        } else if (agent.enabled && animator.GetComponent<UnitMovement>().check_move)
        {
               


                if ( agent.enabled &&agent.isOnNavMesh  &&agent.isActiveAndEnabled && !agent.pathPending && agent.remainingDistance <= 0)
                {
                    agent.velocity = agent.desiredVelocity.normalized * agent.speed;
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        // Đã tới nơi

                        animator.GetComponent<UnitMovement>().StopMoving();
                    }
                } else
                {

               


                    animator.GetComponent<UnitMovement>().MoveToRestLoad();
                
                }
        } else  if (attack.targerToAttack)
        {
           
            if (attack.okAttack)
            {
             
                // hết đạn và  đánh cận chiến
                if (unit.GetNumberBullets() <= 0 && unit.GetClose_combat())
                {
                    
                    if (attack.okAttack)
                    {
                        
                        animator.GetComponent<UnitMovement>().StopMovingTarget();
                        animator.SetBool("isAttack", true);
                        animator.SetFloat("attack", 0);
                    }
                    else
                    {
                        Move(animator);
                    }
                   
                   
                } 
                // hết đạn và không đánh cận chiến
                else if (unit.GetNumberBullets() <= 0 && !unit.GetClose_combat())
                {
                    animator.GetComponent<UnitMovement>().StopMovingTarget();
                }
                else
                {
                    animator.GetComponent<UnitMovement>().StopMovingTarget();
                    animator.SetBool("isAttack", true);
                    animator.SetFloat("attack", 1f);
                }


            }  else
            {
                Move(animator);
            }
        } else
        {
         
            animator.GetComponent<UnitMovement>().StopMovingTarget();
        }


   
       


    } 
    public void Move(Animator animator)
    {

        if (Vector3.Distance(agent.destination, attack.targerToAttack.position) > 0.1f)
        {
        

            animator.GetComponent<UnitMovement>().MoveToTarget(attack.targerToAttack.position);
        }
       
    }
   public bool CheckMap(Collider[] _collider)
    {
        foreach (Collider oll in _collider)
        {
            if (oll.transform == attack.targerToAttack) return true;
        }
        return false;
    }

}

          





       
    

 


        
        
    


