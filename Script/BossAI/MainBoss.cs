using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class MainBoss : MonoBehaviour
{
    public static MainBoss Instance { get; set; }
    private int lv = 1;
    private int UpdateLv = 500;
    
    private GameObject isMain;
    private bool mainHouseBeenCompleted = false;
    private GameObject minerals_house;
    private List<GameObject> ang_ten;
    private GameObject energy_house;
    private List<GameObject>rada;
    private List<GameObject> wresearch_heavy_weapons;
    private List<GameObject> wresearch_light_weapons;
    private GameObject wresearch_tower;
    private List<GameObject> tower;
    private GameObject special_research;
    private List<ObjectData> data;
    private List<GameObject> weapons;
    private List<GameObject> house;
    public int difficulty;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
   
    }
    private void Start()
    {
        InvokeRepeating("BossBuilding", 1f, 3f);
        InvokeRepeating("BossCreateUnit", 10f, 5f);
        InvokeRepeating("Mobilization", 120f, 120f);
        data = DatabaseManager.Instance.GetObjectsData();
        ang_ten = new List<GameObject>();
        rada = new List<GameObject>();
        wresearch_light_weapons = new List<GameObject>();
        wresearch_heavy_weapons = new List<GameObject>();
        weapons = new List<GameObject>();
        tower = new List<GameObject>();
       house = new List<GameObject>();
    }
    private void BossBuilding()
    {
        if (!isMain)
        {
            if (ResourceBoss.Instance.GetMinerals() >= data[0].minerals)
            {
                
                isMain = BuidingSystemsBoss.Instance.BuildingSystem(data[0],-1);
            }
        } else
        {
            if (mainHouseBeenCompleted)
            {
                if (lv == 1)
                {
                    BossLv1();
                    
                    if (difficulty >0 &&ResourceBoss.Instance.GetMinerals() >=UpdateLv*2 && energy_house && minerals_house)
                    {
                        ResourceBoss.Instance.SetMinerals(UpdateLv);
                        StartCoroutine(UpLv());
                    }
                    
                    
                } else if (lv ==2)
                {
                    BossLv1();
                    BossLv2();
                    if (difficulty >1 && ResourceBoss.Instance.GetMinerals() >= UpdateLv * 2 && energy_house && minerals_house)
                    {
                        ResourceBoss.Instance.SetMinerals(UpdateLv);
                        StartCoroutine(UpLv());
                    }
                }
                
            }
        }
    }
    public void BossLv1()
    {
       
            if (!minerals_house)
            {
                if (ResourceBoss.Instance.GetMinerals() >= data[2].minerals && ResourceBoss.Instance.GetEnergy() >= data[2].energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                {
                    minerals_house = BuidingSystemsBoss.Instance.BuildingSystem(data[2],0);
                    return;
                }
            }
            else
            {
                
                int id_Rada = DatabaseManager.Instance.GetObjectDataId(2).PrefabUnit[lv-1];
                if (rada.Count < 5 * lv)
                {
                    if (ResourceBoss.Instance.GetMinerals() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).minerals && ResourceBoss.Instance.GetEnergy() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                    {
                        GameObject _rada = BuidingSystemsBoss.Instance.BuildingSystem(DatabaseManager.Instance.GetObjectDataId(id_Rada), 0);
                        rada.Add(_rada);
                        return;
                    }


                }
            }
            if (!energy_house)
            {

                if (ResourceBoss.Instance.GetMinerals() >= data[1].minerals && ResourceBoss.Instance.GetEnergy() >= data[1].energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                {
                    energy_house = BuidingSystemsBoss.Instance.BuildingSystem(data[1], 0);
                    return;
                }
            }
            else
            {
                int id_Rada = DatabaseManager.Instance.GetObjectDataId(1).PrefabUnit[lv - 1];
                if (ang_ten.Count < 2 * lv)
                {
                    if (ResourceBoss.Instance.GetMinerals() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).minerals && ResourceBoss.Instance.GetEnergy() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                    {
                        GameObject _rada = BuidingSystemsBoss.Instance.BuildingSystem(DatabaseManager.Instance.GetObjectDataId(id_Rada), 0);
                        ang_ten.Add(_rada);
                        return;
                    }


                }
            }
            if (wresearch_light_weapons.Count <lv && wresearch_light_weapons.Count <3)
        {
                if (wresearch_light_weapons.Count < 5 * lv && ResourceBoss.Instance.GetMinerals() >= data[3].minerals && ResourceBoss.Instance.GetEnergy() >= data[3].energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                {
                    GameObject _weapons = BuidingSystemsBoss.Instance.BuildingSystem(data[3],0);
                    wresearch_light_weapons.Add(_weapons);
                    return;
                }
            }
    }
    public void BossLv2()
    {
        if (minerals_house)
        {
            if (wresearch_heavy_weapons.Count <lv)
            {
                if (wresearch_heavy_weapons.Count < 5 * lv && ResourceBoss.Instance.GetMinerals() >= data[4].minerals && ResourceBoss.Instance.GetEnergy() >= data[4].energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                {
                    GameObject _weapons = BuidingSystemsBoss.Instance.BuildingSystem(data[4],0);
                    wresearch_heavy_weapons.Add(_weapons);
                    return;
                }
            }
            if (!wresearch_tower)
            {

                if (ResourceBoss.Instance.GetMinerals() >= data[5].minerals && ResourceBoss.Instance.GetEnergy() >= data[5].energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                {
                    wresearch_tower = BuidingSystemsBoss.Instance.BuildingSystem(data[5], 0);
                    return;
                }
            }
            else
            {
                int index = Random.Range(0, lv);
                int id_Rada = DatabaseManager.Instance.GetObjectDataId(5).PrefabUnit[index];
                if (tower.Count < 3 * lv)
                {
                    if (ResourceBoss.Instance.GetMinerals() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).minerals && ResourceBoss.Instance.GetEnergy() >= DatabaseManager.Instance.GetObjectDataId(id_Rada).energy && ResourceBoss.Instance.GetConstruction() < ResourceBoss.Instance.GetMaximum_project())
                    {
                        GameObject _rada = BuidingSystemsBoss.Instance.BuildingSystem(DatabaseManager.Instance.GetObjectDataId(id_Rada), 0);
                        tower.Add(_rada);
                        return;
                    }


                }
            }

        }

    }
    public IEnumerator UpLv()
    {
        yield return new WaitForSeconds(15f * lv);
        lv++;
        UpdateLv *= lv;
        ResourceBoss.Instance.SetMaximum_project(15);
        ResourceBoss.Instance.SetMaximum_troop_strength(15);
    }
    public void BossCreateUnit()
    {

        if (isMain && mainHouseBeenCompleted )
        {
            if (lv == 1 || lv==2)
            {
                if (wresearch_light_weapons.Count > 0)
                {
                    int i = 0;
                    foreach(GameObject house in wresearch_light_weapons)
                    {
                        
                        int index_unit = DatabaseManager.Instance.GetObjectDataId(house.GetComponent<HouseController>().GetId()).PrefabUnit[i];
                        if (weapons.Count <5*lv && !house.GetComponent<BuildSystem>().enabled && house.GetComponent<HouseController>().GetTimeSkill()[i]<=0 &&ResourceBoss.Instance.GetMilitary_strength()< ResourceBoss.Instance.GetMaximum_troop_strength() && ResourceBoss.Instance.GetMinerals() >= ControlManager.Instance.GetUnit()[index_unit].GetComponent<UnitController>().GetMinerals()&& ResourceBoss.Instance.GetEnergy() >= ControlManager.Instance.GetUnit()[index_unit].GetComponent<UnitController>().GetEnergy())
                        {
                           
                            house.GetComponent<HouseController>().TrySpawnUnit(ControlManager.Instance.GetUnit()[index_unit], i);
                            i++;
                        }
                    }
                }
            } 
            if (lv == 2)
            {
                if (wresearch_light_weapons.Count > 0)
                {
                    int i = 0;
                    foreach (GameObject house in wresearch_heavy_weapons)
                    {
                        int index_unit = DatabaseManager.Instance.GetObjectDataId(house.GetComponent<HouseController>().GetId()).PrefabUnit[i];
                        if (weapons.Count < 5 * lv && !house.GetComponent<BuildSystem>().enabled && house.GetComponent<HouseController>().GetTimeSkill()[i] <= 0 && ResourceBoss.Instance.GetMilitary_strength() < ResourceBoss.Instance.GetMaximum_troop_strength() && ResourceBoss.Instance.GetMinerals() >= ControlManager.Instance.GetUnit()[index_unit].GetComponent<UnitController>().GetMinerals() && ResourceBoss.Instance.GetEnergy() >= ControlManager.Instance.GetUnit()[index_unit].GetComponent<UnitController>().GetEnergy())
                        {
                          
                            house.GetComponent<HouseController>().TrySpawnUnit(ControlManager.Instance.GetUnit()[index_unit], i);
                            i++;
                        }
                    }
                }
            }
           
        }
    }
    public void Mobilization()
    {
        if (weapons.Count >=3 && !ControlBoss.Instance.GetAttack_command()  && !ControlBoss.Instance.GetDefensive_command())
        {
            ControlBoss.Instance.Mobilization();
        }
    }
    public void AddUnitTeam(GameObject unit)
    {
        weapons.Add(unit);
    }
    public void AddHouseTeam(GameObject unit)
    {
        house.Add(unit);
    }
    public void RemoteUnitTeam(GameObject unit)
    {
        weapons.Remove(unit);
    }
    public List<GameObject> GetWeapons()
    {
        return weapons;
    }
    public void RemoteHouseTeam(GameObject unit)
    {
        house.Remove(unit);
    }
    public List<GameObject> GetHouse()
    {
        return house;
    }
    public void SetMainHouseBeenCompleted(bool status)
    {
        mainHouseBeenCompleted = status;
    }
   public int GetLv()
    {
        return lv;
    }
    public void RemoveRada(GameObject _rada)
    {
        rada.Remove(_rada);
    }

    public void RemoveTower(GameObject _tower)
    {
       tower.Remove(_tower);
    }
    public void RemoveAng_ten(GameObject _angTen)
    {
        ang_ten.Remove(_angTen);
    }
    public GameObject GetIsMain()
    {
        return isMain;
    }
    public void RemoveWresearch_light_weapons(GameObject weapon)
    {
        wresearch_light_weapons.Remove(weapon);
    }
    public void RemoveWresearch_heavy_weapons(GameObject weapon)
    {
       wresearch_heavy_weapons.Remove(weapon);
    }
}
