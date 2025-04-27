using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitVision : MonoBehaviour
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

            if (!attack.targerToAttack)
            {
                if (transform.parent.gameObject.layer != other.gameObject.layer)
                {
                    attack.targerToAttack = other.transform;
                    attack.okAttack = false;
                }
            }
            else if (attack.targerToAttack && transform.parent.gameObject.layer != other.gameObject.layer && attack.targerToAttack != other.transform && Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position,attack.targerToAttack.position))
            {
                attack.targerToAttack = other.transform;


            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() || other.GetComponent<HouseController>())
        {
            if (!attack.targerToAttack)
            {
                if (transform.parent.gameObject.layer != other.gameObject.layer)
                {
                    attack.targerToAttack = other.transform;
                    attack.okAttack = false;
                }
            }
            else if (attack.targerToAttack && transform.parent.gameObject.layer != other.gameObject.layer && attack.targerToAttack != other.transform && Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, attack.targerToAttack.position))
            {
                attack.targerToAttack = other.transform;


            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == attack.targerToAttack)
        {


            attack.targerToAttack = null;
            attack.okAttack = false;

        }
    }
}
