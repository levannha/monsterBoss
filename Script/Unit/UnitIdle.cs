using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class UnitIdle : StateMachineBehaviour
{
    AttackController attack;
   
    private UnitController unit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack = animator.transform.GetComponent<AttackController>();
       
        unit = animator.GetComponent<UnitController>();
    }

     //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (unit.GetHp() <= 0)
        {
          
            animator.SetBool("isDie", true);
            return;
        }
   
     
        if (attack.targerToAttack && animator.GetComponent<UnitController>().GetNumberBullets() <= 0 &&  !unit.GetClose_combat() || !attack.targerToAttack)
        {
            if (unit.GetDistanceLong().activeSelf) {
                unit.GetDistanceLong().SetActive(false);
            }
            return;
        }
        else 
        {
            unit.GetDistanceLong().SetActive(true);
            animator.SetBool("isWalk", true);
        }
    }

}
