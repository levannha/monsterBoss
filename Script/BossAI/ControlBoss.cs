using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlBoss : MonoBehaviour
{
    public static ControlBoss Instance { get; set; }
    private bool defensive_command = false;// lệnh phòng thủ
    private bool attack_command = false; // lệnh tấn công
    
    private void Awake()
    {
        if (Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }
    public void MoveTroopsToTheAttackedPosition(Transform _house)
    {
        defensive_command = true;
        foreach  (GameObject unit in MainBoss.Instance.GetWeapons())
        {
            if (!unit.GetComponent<AttackController>().targerToAttack)
            {
                unit.GetComponent<AttackController>().targerToAttack = CheckEnemy(_house).transform;
            }
        }
        StartCoroutine(ResetDefensive_command());
    }
    public void Mobilization()
    {
        attack_command = true;
        if (GameManager.Instance.GetHouse().Count > 0)
        {
            foreach (GameObject house in MainBoss.Instance.GetWeapons())
            {
               house.GetComponent<AttackController>().targerToAttack = GameManager.Instance.GetHouse()[0].transform;
            }
        } else if (GameManager.Instance.GetUnit().Count > 0)
        {
            foreach (GameObject unit in MainBoss.Instance.GetWeapons())
            {
                unit.GetComponent<AttackController>().targerToAttack = GameManager.Instance.GetUnit()[0].transform;
            }
        }
        StartCoroutine(ResetAttack_command());
    }
    private IEnumerator ResetDefensive_command()
    {
        yield return new WaitForSeconds(60f);
        defensive_command = false;
    }
    private IEnumerator ResetAttack_command()
    {
        yield return new WaitForSeconds(60f);
        attack_command = false;
    }
    private GameObject CheckEnemy(Transform house)
    {
       
        float distance = 30f; // khoảng cách cần kiểm tra

        Collider[] hitColliders = Physics.OverlapSphere(house.position, distance, LayerMask.GetMask("ClickAble"), QueryTriggerInteraction.Ignore);
        if (hitColliders.Length >0)
        {
            return hitColliders[0].gameObject;
        }
        return null;
    }
    public bool GetDefensive_command()
    {
        return defensive_command;
    }
    public bool GetAttack_command()
    {
        return attack_command;
    }
    
    public Vector3 FindEmptySpawnPosition(Transform spawnPoint)
    {
        float searchRadius = 30f;
        int maxAttempts = 20;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * searchRadius;
            Vector3 spawnPos = spawnPoint.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // Check nếu không có vật cản/lính khác trong vùng này
            if (!Physics.CheckSphere(spawnPos, 0.5f, LayerMask.GetMask("Enemy") | LayerMask.GetMask("ClickAble"), QueryTriggerInteraction.Ignore)) // bán kính 0.5 để kiểm tra va chạm
            {
                return spawnPos;
            }
        }
        
        // Nếu không tìm được chỗ trống, spawn tại điểm gốc
        return spawnPoint.position;
    } 
   
}
