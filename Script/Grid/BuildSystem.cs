using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildSystem : MonoBehaviour
{ 
    
    public GameObject build;
    private int time=1;
    private HouseController house;
    private bool repair = false;
    private bool complete = false;
    void Start()
    {
        GetComponent<MeshCollider>().enabled = true;
        build.SetActive(true);
        house = GetComponent<HouseController>();
    }
    private void Update()
    {
        if (repair && complete && house.GetHp() < house.GetHpRoot() && ResourceManager.Instance.GetMinerals() == 0)
        {
            repair = false;
            GetComponent<BuildSystem>().enabled = false;
           
        }
        if (repair&& complete &&house.GetHp()< house.GetHpRoot() &&ResourceManager.Instance.GetMinerals()>0)
        {
           
            ResourceManager.Instance.SetMinerals(1);
            if (house.GetHp()+ 5> house.GetHpRoot())
            {
                house.SetHp(house.GetHpRoot() - house.GetHp());
                repair = false;
                GetComponent<BuildSystem>().enabled = false;
            }
            else
            { 
                house.SetHp(5);
                
            }
        } else if(!complete) { 
            if (time <=house.GetHpRoot() / 5)
            {
                time++;
                house.SetHp(5);
              
               
            }
            else
            {
                build.SetActive(false);
                
                complete = true;
                if (GetComponent<MainController>())
                {
                    if (gameObject.layer ==7)
                    {
                        GameManager.Instance.SetMainHouseBeenCompleted(true);
                        ResourceManager.Instance.SetMaximum_troop_strength(10);
                        ResourceManager.Instance.SetMaximum_project(10);
                        GameManager.Instance.OnUpdate();
                    } else if (gameObject.layer ==8)
                    {
                       MainBoss.Instance.SetMainHouseBeenCompleted(true);
                        ResourceBoss.Instance.SetMaximum_troop_strength(10);
                        ResourceBoss.Instance.SetMaximum_project(10);
                    }
                   
                } 
                if (GetComponent<BuySystem>())
                {
                   
                    GetComponent<BuySystem>().PlayIncome();
                }
                if (GetComponent<BuySlot>())
                {
                   
                    GetComponent<BuySlot>().PlayIncome();
                }
                if (GetComponent<AttackController>())
                {
                    GetComponent<TowerAttack>().GetWeaponse().SetActive(true);
                    GetComponent<TowerAttack>().enabled = true;
                }
                if (gameObject.layer ==7 )
                {
                    GetComponent<FogUnit>().enabled = true;
                    
                }
                GetComponent<BuildSystem>().enabled = false;
                
            }
        }
    }

    public bool GetRepair()
    {
     return   repair;
    }
    public void SetRepair()
    {
        repair = true;
    }
    public bool GetComplete()
    {
        return complete;
    }
}
