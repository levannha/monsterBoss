using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
   private NavMeshAgent agent;
    [SerializeField] private LayerMask ground;
   private Animator _animator;
    public bool check_move = false;
    private Vector3 position_move;
   
   
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
       
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void MoveTo(Vector3 destination)
    {
        _animator.SetBool("isWalk", true);
        GetComponent<AttackController>().targerToAttack = null;
        check_move = true;
        if (agent.isOnNavMesh)
        {
            position_move = destination;
            agent.ResetPath();
            agent.SetDestination(destination);
        }
    }
    public void MoveToRestLoad()
    {
        if (agent.isOnNavMesh && Vector3.Distance(agent.destination,position_move) > 0.1f)
            
        {
            agent.ResetPath();

            agent.SetDestination(position_move);
        }
    }
    public void MoveToTarget(Vector3 destination)
    {
        if (agent.isOnNavMesh)
        {
           
            agent.ResetPath();
         
            agent.SetDestination(destination);

        }
    }

    public void StopMovingTarget()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
       
        _animator.SetBool("isWalk", false);
        
        UnitSelectManager.Instance.OffGroundMarket(transform);

    }

    public void StopMoving()
    {
        agent.isStopped = true;
        check_move = false;      
        _animator.SetBool("isWalk", false);
       
        UnitSelectManager.Instance.OffGroundMarket(transform);
    }

   



}
