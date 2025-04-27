using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class BuidingSystemsBoss : MonoBehaviour
{

    public static BuidingSystemsBoss Instance { get; set; }
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


    public GameObject BuildingSystem(ObjectData house,int index)
    {
        Vector3 mousePosition = FindEmptySpawnPosition( house.Size,house.Prefab);
        ResourceBoss.Instance.SetMinerals(house.minerals);
        ResourceBoss.Instance.SetEnergy(house.energy);
        GameObject newBuilding = Instantiate(house.Prefab,mousePosition+ new Vector3(0, house.Prefab.GetComponent<HouseController>().GetDepth()+1f, 0), Quaternion.identity);
        if (newBuilding.CompareTag("tower") )
        {
            newBuilding.transform.Rotate(Vector3.left, 90f);
        }
      
        newBuilding.GetComponent<HouseController>().SetPositionBuildSystem(mousePosition);
        newBuilding.GetComponent<NavMeshObstacle>().enabled = true;
        newBuilding.GetComponent<BuildSystem>().enabled = true;
        newBuilding.layer = 8;
        newBuilding.GetComponent<HouseController>().ShareTheTeam();
        if (!newBuilding.GetComponent<MainController>())
        {
           ResourceBoss.Instance.SetConstruction(1);
        }
        newBuilding.GetComponent<HouseController>().SetId(house.ID);
        GridSystem.Instance.Save(newBuilding, house.Size, GridSystem.Instance.GetCellPosition(newBuilding.transform.position));
        MainBoss.Instance.AddHouseTeam(newBuilding);
        ControlManager.Instance.CreateMiniMap(newBuilding);
        ResourceBoss.Instance.SetTotal_construction();
        return newBuilding;
}
    

    public Vector3 FindEmptySpawnPosition( Vector2Int size,GameObject frefab)
    {
      
        Transform spawnPoint = MainBoss.Instance.transform;
       
      

        float searchRadius = 30f * MainBoss.Instance.GetLv() +MainBoss.Instance.GetHouse().Count + MainBoss.Instance.GetHouse().Count;
        int maxAttempts = 20;
        float radiusStep = 1f;
        for (int i = 0; i < maxAttempts; i++)
        {
          
            float currentRadius = searchRadius + (i * radiusStep);
         
            Vector2 randomOffset = Random.insideUnitCircle * currentRadius;
            Vector3 spawnPos = spawnPoint.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // Check nếu không có vật cản/lính khác trong vùng này
            if (!Physics.CheckSphere(spawnPos,10f, LayerMask.GetMask("Enemy","ClickAble"), QueryTriggerInteraction.Ignore)) // bán kính 0.5 để kiểm tra va chạm
            {
                
              
                  

                    return spawnPos;
              
            }
        }

        // Nếu không tìm được chỗ trống, spawn tại điểm gốc

        return spawnPoint.position;
    }
}
