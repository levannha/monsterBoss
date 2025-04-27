using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttack : MonoBehaviour
{
    private AttackController attack;
    // Start is called before the first frame update
    private void Awake()
    {
        attack = transform.parent.GetComponent<AttackController>();
    }

    private void OnTriggerEnter(Collider other)
    {
     
            if (other.GetComponent<UnitController>() || other.GetComponent<HouseController>())
            {
                if (attack.targerToAttack && other.transform == attack.targerToAttack)
                {
                    attack.okAttack = true;
                }
            }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() || other.GetComponent<HouseController>())
        {
            if (attack.targerToAttack && other.transform == attack.targerToAttack)
            {
                attack.okAttack = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == attack.targerToAttack)
        {
                    attack.okAttack =false;
                         

        }
    }
}
