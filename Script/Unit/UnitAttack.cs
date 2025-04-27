using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttack : StateMachineBehaviour
{
    private AttackController attack;
   
    private float attackTime = 1.5f;
    private float attack_root =0;
    private UnitController unit;
   
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack = animator.GetComponent<AttackController>();
        unit = animator.GetComponent<UnitController>(); 
       if (animator.GetFloat("attack")==0)
        {
            attackTime = 2.667f;
        }
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (unit.GetHp() <= 0)
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("isDie", true);
            return;
        }
  
        if (!attack.targerToAttack || attack.targerToAttack && attack.targerToAttack.GetComponent<UnitController>() && attack.targerToAttack.GetComponent<UnitController>().GetHp() <= 0)
        {
            attack.targerToAttack = null;
            attack.okAttack = false;
            animator.SetBool("isAttack", false);


        } else
        {
            
            animator.transform.LookAt(attack.targerToAttack);// luôn hướng về mục tiêu
            
           
          if (animator.GetFloat("attack") == 0) 
            {
               
               if ( !attack.okAttack )
                {

                    // kẻ dịch bỏ chạy
                 
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isWalk", true);
                    attack.okAttack = false;
                   
                } else
                {

                    // tấn công cận chiến
                    AttackTarget(0, animator);
                }
            }
            else if (animator.GetFloat("attack") == 1)
            {
               
                if (unit.GetNumberBullets() <= 0)
                {
                    //hết đạn
                   
                 
                    animator.SetBool("isAttack", false);
                    attack.okAttack = false;
                } else
                {
                    // tấn công tầm xa
                    AttackTarget(1, animator);
                }
            }
        }

    }
  
    public void AttackTarget(int x,Animator animator)
    {
        attack.okAttack = false;
        if (attack_root >= attackTime)
        {

            attack_root = 0;
            if (x == 0)
            {
                Attack(animator);
            }
            if (x == 1)
            {
                AttackBullet(animator);
            }
        }
        else
        {
            attack_root += Time.deltaTime;
        }
    
    }
    private void Attack(Animator _animator)
    {

        float distance = Vector3.Distance(GameManager.Instance.GetCenter().position, _animator.transform.position);
        float fill = 20f;
        if (distance <= fill)
        {

            SoundManager.Instance.Play(unit.PlayAttack(), distance / fill);

        }
        if (attack.targerToAttack.CompareTag("house")|| attack.targerToAttack.CompareTag("tower"))
        {
            attack.targerToAttack.GetComponent<HouseController>().TakeDamage(_animator.GetComponent<UnitController>().GetDamage());
        }
        else
        {
            attack.targerToAttack.GetComponent<UnitController>().TakeDamage(_animator.GetComponent<UnitController>().GetDamage());
        }
        
    }
    private void AttackBullet(Animator _animator)
    {
        float distance = Vector3.Distance(GameManager.Instance.GetCenter().position, _animator.transform.position);
        float fill = 20f;
        if (distance <= fill)
        {

            SoundManager.Instance.Play(unit.PlayShoot(), distance / fill);

        }
        unit.ShootGun(attack.targerToAttack);

        
      
            if (attack.targerToAttack.CompareTag("house") || attack.targerToAttack.CompareTag("tower"))
            {
                attack.targerToAttack.GetComponent<HouseController>().TakeDamage(_animator.GetComponent<UnitController>().GetDamage() );
            }
            else
            {
                attack.targerToAttack.GetComponent<UnitController>().TakeDamage(_animator.GetComponent<UnitController>().GetDamage());
            }
        
        unit.SetBulletNumber();
        
    }
    
}
