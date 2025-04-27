using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseController : MonoBehaviour
{
   
    private Vector3 positionBuildSystem;
    [SerializeField] private int hp_root;// máu gốc
    [SerializeField] private int damage; // sát thương
    [SerializeField] private Canvas hpUi; // UI 
    [SerializeField] private int number_bullets; // số lượng đạn tối đa 
    [SerializeField] private bool shooting_missiles; // rocket
    [SerializeField] private GameObject smoke; // khói khi bị hỏng;
    [SerializeField] private float reloading_time; // thời gian nạp đạn
   
    private int id =-1;
    [SerializeField] private Transform spawnPoint;  // Điểm sinh quân
    [SerializeField] private float depth;
    private int hp = 1;// máu hiện tại
    private int bullet;// số lượng đạn còn lại
    public Transform firePointLeft; // Điểm bắn đạn trái 
    public Transform firePointRight; //diểm bắn đạn phải
    public GameObject bulletPrefab; // Prefab của viên đạn
    private GameObject MiniMapIcon;
    private float[] timeSkill = {0,0,0};
    [SerializeField] private float[] timeRoot;
    public AudioClip building;
    public AudioClip music;
    private void FixedUpdate()
    {
        
       for (int i =0;i< timeSkill.Length;i++)
        {
            if (timeSkill[i] >0)
            {
                timeSkill[i] -= Time.deltaTime;
            }
        }
       
          

    }
  
   internal float[] GetTimeSkill()
    {
        return timeSkill;
    }
    internal float[] GetTimeSkillRoot()
    {
        return timeRoot;
    }
    internal  float GetDepth()
    {
        return depth;
    }
    internal  void SetPositionBuildSystem(Vector3 position)
    {
        positionBuildSystem = position;
    }
    internal Vector3 GetPositionBuildSystem()
    {
       return positionBuildSystem;
    }
    internal int GetHp()
    {
        return hp;
    }
    internal  int GetHpRoot()
    {
        return hp_root;
    }
    internal void SetHp(int _hp)
    {
        hp += _hp;
        if (hp > 0 && hp <= hp_root / 3 && smoke && !smoke.activeSelf)
        {
            smoke.SetActive(true);
        }
        else
        {
            smoke.SetActive(false);
        }
        hpUi.gameObject.SetActive(true);
        hpUi.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = (float)hp /(float)hp_root;
    }
    public void ShootGun(Transform target)
    {
        ShootSpread(firePointLeft, target);
        ShootSpread(firePointRight, target);

    }
    private void ShootSpread(Transform firePoint, Transform target)
    {
        if (firePoint)
        {
            firePoint.gameObject.SetActive(true);

            float angle = Random.Range(-20f / 2, 20f / 2); // Góc lệch ngẫu nhiên
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<Bullet>().SetTarget(target);
            StartCoroutine(ResetShoot(firePoint));
        }

    }
    public IEnumerator ResetShoot(Transform firePoint)
    {
        yield return new WaitForSeconds(0.5f);
        firePoint.gameObject.SetActive(false);
    }
    internal void TakeDamage(int _damge)
    {
        if (hp > 0)
        {
            hp -= _damge;
            hpUi.GetComponent<UnitHpBar>().TakeDamageUi();
            if (gameObject.layer ==8)
            {
                if (!ControlBoss.Instance.GetAttack_command()  && !ControlBoss.Instance.GetDefensive_command())
                {
                    ControlBoss.Instance.MoveTroopsToTheAttackedPosition(transform);
                }
            }
            if (hp > 0 && hp <= hp_root / 3 && smoke && !smoke.activeSelf)
            {
                smoke.SetActive(true);
            }
            else
            {
                smoke.SetActive(false);
            }
            if (hp <= 0)
            {

                DestroyUnit();

            }

        }
    }
    public void ShareTheTeam()
    {
        hpUi.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.red;
      
    }
    public void SetMiniMapIcon(GameObject icon)
    {
        MiniMapIcon = icon;
    }
    internal void DestroyUnit()
    {
        ControlManager.Instance.GetGridSystem().RemoveBuildingSystem(positionBuildSystem,gameObject);
        UnitSelectManager.Instance.PlayExplosion(transform);
        SoundManager.Instance.PlayExplosionmachineExplosion();
        if(gameObject.layer == 7 &&!GetComponent<MainController>())
        {
            ResourceManager.Instance.SetConstruction(-1);
            GameManager.Instance.RemoteHouse(gameObject);
            ResourceBoss.Instance.GetDestroy_construction();
        }
        else if (gameObject.layer == 8 && !GetComponent<MainController>())
        {
            ResourceBoss.Instance.SetConstruction(-1);
            MainBoss.Instance.RemoteHouseTeam(gameObject);
            if (id==3)
            {
                MainBoss.Instance.RemoveWresearch_light_weapons(gameObject);
            }
            else if (id == 4)
            {
                MainBoss.Instance.RemoveWresearch_heavy_weapons(gameObject);
            } else if (id == 7|| id == 8)
            {
                MainBoss.Instance.RemoveTower(gameObject);
            } else if (id == 9 || id == 10) {
                MainBoss.Instance.RemoveRada(gameObject);
            } else if (id == 11 || id == 12)
            {
                MainBoss.Instance.RemoveAng_ten(gameObject);
            }
            ResourceManager.Instance.GetDestroy_construction();
        }
        Destroy(MiniMapIcon);
        Destroy(gameObject);
    }
    internal float HPRatio()
    {
        return (float)hp / (float)hp_root;
    }
    internal void SetId(int _id)
    {
        id = _id;
    }
    internal  int GetId()
    {
        return id;
    }
    internal bool GetShooting_missile()
    {
        return shooting_missiles;
    }
    public Vector3 FindEmptySpawnPosition()
    {
        float searchRadius = 5f;
        int maxAttempts = 20;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * searchRadius;
            Vector3 spawnPos = spawnPoint.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // Check nếu không có vật cản/lính khác trong vùng này
            if (!Physics.CheckSphere(spawnPos, 0.5f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore)|| !Physics.CheckSphere(spawnPos, 0.5f, LayerMask.GetMask("ClickAble"), QueryTriggerInteraction.Ignore)) // bán kính 0.5 để kiểm tra va chạm
            {
                return spawnPos;
            }
        }

        // Nếu không tìm được chỗ trống, spawn tại điểm gốc
        return spawnPoint.position;
    }
  public  void TrySpawnUnit(GameObject unitCost,int _lv)
    {
      
            Vector3 emptyPosition = FindEmptySpawnPosition();

            GameObject  unit=  Instantiate(unitCost, emptyPosition, Quaternion.identity);
        if (gameObject.layer == 7)
        {
            
            ResourceManager.Instance.SetMinerals(unitCost.GetComponent<UnitController>().GetMinerals());
            ResourceManager.Instance.SetEnergy(unitCost.GetComponent<UnitController>().GetEnergy());
            timeSkill[_lv] = timeRoot[_lv];
            ResourceManager.Instance.SetMilitary_strength(1);
            unit.GetComponent<FogUnit>().enabled = true;
            GameManager.Instance.AddUnit(unit);
            unit.layer =7;
            ControlManager.Instance.CreateMiniMap(unit);
            ResourceManager.Instance.SetTrained_personnel();
        }
        else if (gameObject.layer == 8)
        {
            unit.GetComponent<UnitController>().SetTeam();
           
            ResourceBoss.Instance.SetMinerals(unitCost.GetComponent<UnitController>().GetMinerals());
            ResourceBoss.Instance.SetEnergy(unitCost.GetComponent<UnitController>().GetEnergy());
            timeSkill[_lv] = timeRoot[_lv];
            ResourceBoss.Instance.SetMilitary_strength(1);
            unit.layer = 8;
            ControlManager.Instance.CreateMiniMap(unit);
            MainBoss.Instance.AddUnitTeam(unit);
            ResourceBoss.Instance.SetTrained_personnel();
        }
    }
 
    internal int GetDamage()
    {
        return damage;
    }

}
