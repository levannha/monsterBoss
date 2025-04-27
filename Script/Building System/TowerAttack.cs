using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private float deslayTime;
    [SerializeField] private GameObject weponse;
    private float time = 0;
    private AttackController attack;
    private HouseController house;
    void Start()
    {
        attack = GetComponent<AttackController>();
        house = GetComponent<HouseController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       
            if (attack.targerToAttack)
            {
            if (attack.targerToAttack.GetComponent<UnitController>()&& attack.targerToAttack.GetComponent<UnitController>().GetHp()<=0)
            {
                attack.targerToAttack = null;
                return;
            }

                Vector3 direction = attack.targerToAttack.position - gun.transform.position;
                direction.y = 0;
                // Look at target
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Bù lại xoay -90 độ X của parent
                Quaternion correction = Quaternion.Euler(90f, 0,0f); // Bù lại -90 -> +90
               
                gun.transform.rotation = lookRotation * correction;
                gun.transform.Rotate(Vector3.up, 180f);
                gun.transform.Rotate(Vector3.forward, 180f);
            if (time > 0)
            {
                time -= Time.deltaTime;

            }
            else
            {
                time = deslayTime;
                house.ShootGun(attack.targerToAttack);
                if (house.GetShooting_missile())
                {
                    SoundManager.Instance.PlayShootRocket();
                }
                else
                {
                    SoundManager.Instance.PlayInfantryAttackShootSound();
                }
                if (attack.targerToAttack.CompareTag("house") || attack.targerToAttack.CompareTag("tower"))
                {
                    attack.targerToAttack.GetComponent<HouseController>().TakeDamage(house.GetDamage());
                }
                else
                {
                    attack.targerToAttack.GetComponent<UnitController>().TakeDamage(house.GetDamage());
                }
            }
        }
    }
    public GameObject GetWeaponse()
    {
        return weponse;
    }
}
