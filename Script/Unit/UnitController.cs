using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    [SerializeField] private int id; 
    private int hp_root;// máu gốc
    private int damage; // sát thương
    [SerializeField] private Canvas hpUi; // UI 
    [SerializeField] private int number_bullets; // số lượng đạn tối đa
    [SerializeField] private bool close_combat; // đánh cận chiến 
    [SerializeField] private bool shooting_missiles; // rocket
    [SerializeField] private GameObject distance_attack; // tầm tìm kiếm
    [SerializeField] private GameObject smoke; // khói khi bị hỏng;
    private float reloading_time; // thời gian nạp đạn
    [SerializeField] private Sprite avatar; //ảnh đại diện
    [SerializeField] private string name; //tên
    [SerializeField] private string description; //mô tả
    [SerializeField] private int minerals;// khoáng sản
    [SerializeField] private int energy;// năng lượng 
   
    private int hp = 1;// máu hiện tại
    private int bullet;// số lượng đạn còn lại
    public Transform firePointLeft; // Điểm bắn đạn trái 
    public Transform firePointRight; //diểm bắn đạn phải
    public GameObject bulletPrefab; // Prefab của viên đạn
    public GameObject distanceLong; //tầm đánh xa
    
    [SerializeField] private AudioClip music_wall;
    [SerializeField] private AudioClip music_attack;
    [SerializeField] private AudioClip music_shoot;
    [SerializeField] private AudioClip music_idle;


    private void Awake()
    {
        DataUnit data = GameManager.Instance.GetDataUnit(id);
       
        hp_root = data.hp;
        hp = data.hp;
        damage = data.damage;
        reloading_time = data.time_skill;
        bullet = number_bullets;
        hpUi.GetComponent<UnitHpBar>().TakeDamageUi();
        if (bulletPrefab && number_bullets > 0)
        {
            hpUi.GetComponent<UnitHpBar>().UpdateBullets(bullet);
        }
        
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
    internal void SetTeam() {
        hpUi.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.red;
       // icon_mini_map.transform.GetChild(0).GetComponent<Image>().color = Color.red;
       
    }
    internal void TakeDamage(int _damge)
    {
        if (hp > 0)
        {
            hp -= _damge;
            hpUi.GetComponent<UnitHpBar>().TakeDamageUi();
            if (hp > 0 && hp <= hp_root / 3 && smoke && !smoke.activeSelf)
            {
                smoke.SetActive(true);
            }
            if (hp <= 0)
            {
              
                DestroyUnit();
               

            }

        }
    }
   internal void DestroyUnit()
    {
      
         GetComponent<Animator>().SetBool("isDie", true);
        
        StartCoroutine(StartTheExplosion());
        hpUi.gameObject.SetActive(false);
        GetComponent<AttackController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<UnitMovement>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        if(gameObject.layer==7)
        {
            ResourceManager.Instance.SetMilitary_strength(-1);
            UnitSelectManager.Instance.RemoteUnit(gameObject);
            UnitSelectManager.Instance.CheckTeamSelect();
            GameManager.Instance.RemoteUnit(gameObject);
            ResourceBoss.Instance.SetEnemy_eliminated();
        }
        else if (gameObject.layer == 8)
        {
            ResourceBoss.Instance.SetMilitary_strength(-1);
            MainBoss.Instance.RemoteUnitTeam(gameObject);
            ResourceManager.Instance.SetEnemy_eliminated();
        }
        Destroy(gameObject, 3.333f);
    }
    public IEnumerator StartTheExplosion()
    {
        yield return new WaitForSeconds(3f);
        UnitSelectManager.Instance.PlayExplosion(transform);
        SoundManager.Instance.PlayExplosionmachineExplosion();
    }
    internal int GetDamage()
    {
        return damage;
    }
    internal bool GetClose_combat()
    {
        return close_combat;
    }
    internal int GetNumberBullets()
    {
        return bullet;
    } 
    internal int GetHpRoot()
    {
        return hp_root;
    }
    internal Sprite GetAvatar()
    {
        return avatar;
    }
    internal void SetBulletNumber()
    {
        bullet--;
        hpUi.GetComponent<UnitHpBar>().UpdateBullets(bullet);
        if (bullet == 0)
        {
            if (close_combat)
            {
                GetDistanceLong().GetComponent<SphereCollider>().radius = 2.5f;
                GetComponent<AttackController>().okAttack = false;
            }
            StartCoroutine(ResetBullet());
        }
    }
    public IEnumerator ResetBullet()
    {
        yield return new WaitForSeconds(reloading_time);
        bullet = number_bullets;
        hpUi.GetComponent<UnitHpBar>().UpdateBullets(bullet);
        if (close_combat)
        {
            GetDistanceLong().GetComponent<SphereCollider>().radius = 8f;
       
        }
    }
    internal float HPRatio()
    {
        return (float) hp / (float) hp_root;
    }
    internal int GetHp()
    {
        return hp;
    }
     internal GameObject GetDistanceAttack()
    {
        return distance_attack;
    }
   
    internal GameObject GetDistanceLong()
    {
        return distanceLong;
    }
    internal bool GetShooting_missile()
    {
        return shooting_missiles;
    }
    internal string GetName()
    {
        return name;
    }
    internal string GetDescription()
    {
        return description;
    }
   
    internal int GetMinerals() 
    {
        return minerals;
    }    
   internal int GetEnergy()
    {
        return energy;
    }

    public AudioClip PlayWail()
    {
       
         return music_wall;
        
    }
    public AudioClip PlayAttack()
    {

      return  music_attack;
       
    }
    public AudioClip PlayShoot()
    {
      
        return    music_shoot;
        
    }
    public AudioClip PlayIdle()
    {
      
           
      return  music_idle;
        
    }
  
}
