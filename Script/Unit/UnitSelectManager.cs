using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectManager : MonoBehaviour
{
    public static UnitSelectManager Instance { get; set; }
    public List<GameObject> AllUnit = new List<GameObject>();
    public List<GameObject> SelectUnit = new List<GameObject>();
    [SerializeField] private LayerMask clickAble;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private GameObject GroundMarket;
    public bool attackCursorVisible = false;
    [SerializeField] private GameObject explosion; // vụ nổ
    [SerializeField] private Button cameraPlayer; // chọn cam
    [SerializeField] private Button selectBox; // chọn nhiều
    [SerializeField] private Button teamList;
    public float spacing = 2.0f; // Khoảng cách giữa các lính
    Camera cam;
    
     
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
        cam = Camera.main;
        CameraScrollPlay();
    }



    void Update()
    {
       
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !GridSystem.Instance.GetCurrentBuilding())
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
          
           if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickAble, QueryTriggerInteraction.Ignore))
                {
                
                if (!Input.GetKey(KeyCode.Space) && hit.collider.gameObject.CompareTag("unit"))
                    {
                        MultiSelect(hit.collider.gameObject);

                    }
                    else if (hit.collider.gameObject.CompareTag("unit"))
                    {
                        SelectByClicking(hit.collider.gameObject);
                    }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemy, QueryTriggerInteraction.Ignore) && SelectUnit.Count>0)
            {
                GroundMarket.SetActive(false);
                SetAttackTarget(SelectUnit, hit.collider.transform);
               
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground, QueryTriggerInteraction.Ignore)&&SelectUnit.Count > 0)
            {
                GroundMarket.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                GroundMarket.SetActive(true);
                MoveUnits(SelectUnit, hit.point);
            }

        }

      
        
    }
    private void MultiSelect(GameObject unit)
    {
        if (unit)
        {
            if (!SelectUnit.Contains(unit))
            {
                SelectUnit.Add(unit);
                SelectUnitAdd(unit, true);
            }
            else
            {
                SelectUnit.Remove(unit);
                SelectUnitAdd(unit, false);
            }
        }
    }
    public bool CheckSelectUnit(GameObject unit)
    {
        return SelectUnit.Contains(unit);
    }
    public void RemoteUnit(GameObject unit)
    {
        if (SelectUnit.Contains(unit))
        {
        
            SelectUnit.Remove(unit);
           
        }
    }
   public void OffGroundMarket(Transform target)
    {
        if (SelectUnit.Contains(target.gameObject)|| Vector3.Distance(GroundMarket.transform.position,target.position) <=target.GetComponent<CapsuleCollider>().height)
            GroundMarket.SetActive(false);
    }
    public void DeselectAll()
    {
        foreach(GameObject unit in SelectUnit)
        {
            SelectUnitAdd(unit, false);
        }
        SelectUnit.Clear();
        CheckTeamSelect();
    }
    private void SelectByClicking(GameObject unit)
    {
        if (unit)
        {
            DeselectAll();
            SelectUnit.Add(unit);
            SelectUnitAdd(unit, true);
        }
    }

    private void TriggerSelectIndicator(GameObject unit ,bool status)
    {
        if (unit)
        {
            unit.transform.GetChild(0).gameObject.SetActive(status);
            
        }
    }
    internal void DragSelect( GameObject unit)
    {
       
        if (!SelectUnit.Contains(unit) && unit)
        {
            SelectUnit.Add(unit);
            SelectUnitAdd(unit, true);
        } 
    }
    public void SelectUnitAdd(GameObject unit,bool status)
    {
        if (unit)
        {
            TriggerSelectIndicator(unit, status);
          

        }
        CheckTeamSelect();
    }
    public void PlayExplosion(Transform target)
    {
        GameObject _explosion = Instantiate(explosion, target.position, Quaternion.identity);
        Destroy(_explosion, 1.5f);
    }
    public void SelectBoxPlay()
    {
        attackCursorVisible = true;
        selectBox.GetComponent<Image>().color = Color.cyan;
        cameraPlayer.GetComponent<Image>().color = Color.white;
    }
    public void CameraScrollPlay()
    {
        attackCursorVisible = false;
        selectBox.GetComponent<Image>().color = Color.white;
        cameraPlayer.GetComponent<Image>().color = Color.cyan;
    }
    public void CheckTeamSelect()
    {

        if (SelectUnit.Count > 0)
        {
            foreach (var unit in SelectUnit)
            {
                if (unit && unit.GetComponent<UnitController>().GetHp() <= 0)
                {
                    SelectUnit.Remove(unit);
                }
            }
            teamList.gameObject.SetActive(true);
            teamList.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SelectUnit.Count + "";
        }
        else
        {
            teamList.gameObject.SetActive(false);
        }
    }
    

    public void MoveUnits(List<GameObject> units, Vector3 center)
    {
        int count = units.Count;
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(count));
        
        
        for (int i = 0; i < count; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            // Tính offset cho mỗi lính so với tâm
            Vector3 offset = new Vector3(
                (col - gridSize / 2) * spacing,
                0,
                (row - gridSize / 2) * spacing
            );

            Vector3 targetPos = center + offset;

            // Ra lệnh di chuyển
        
            units[i].GetComponent<UnitMovement>().MoveTo(targetPos);
        }
    }
    public void SetAttackTarget(List<GameObject> units,Transform enemy)
    {
        int count = units.Count;
        for (int i = 0; i < count; i++)
        {
            // Ra lệnh tấn công
            units[i].GetComponent<UnitMovement>().StopMoving();
            units[i].GetComponent<AttackController>().targerToAttack =enemy;
           
        }
    }
} 
