using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; set; }
    [SerializeField] private TextMeshProUGUI text_minerals ;
    [SerializeField] private TextMeshProUGUI text_energy;
    [SerializeField] private TextMeshProUGUI text_construction;
    [SerializeField] private TextMeshProUGUI text_military_strength;
    [SerializeField] private TextMeshProUGUI text_achievement;
    private int minerals =2000;// khoáng sản
    private int energy= 200;// năng lượng 
    private int construction =0; //công trình
    private int maximum_project =0; // công trình tối đa
    private int military_strength=0;//quân số 
    private int maximum_troop_strength=0;  //quân số tối đa
    private int achievement =0; //thành tích
    private int total_construction = 0;// công trình đã xây 
    private int trained_personnel = 0; //quân đã huấn luyện
    private int destroy_construction = 0;//công trinh dã pha huy
    private int enemy_eliminated = 0; // quan dich bi tieu diet
    private void Awake()
    {
      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        UpdateMineralsUI();
        UpdateEnergUI();
        UpdateConstructionUI();
        UpdateAchievementUI();
        UpdateMilitaryStrengthUI();
    }

    public void UpdateMineralsUI()
    {
        text_minerals.text = "" + minerals;
    }
    public void UpdateEnergUI()
    {
        text_energy.text = "" +energy;
    }
    public void UpdateConstructionUI()
    {
        text_construction.text = construction + "/" + maximum_project;
    }
    public void UpdateMilitaryStrengthUI()
    {
        text_military_strength.text = military_strength + "/" +maximum_troop_strength;
    }
    public void UpdateAchievementUI()
    {
        text_achievement.text =   ""+achievement;
    }
    public int GetMinerals()
    {
        return minerals;
    }
    public void SetMinerals(int _mineral)
    {
         minerals-=_mineral;
        UpdateMineralsUI();
    }
    public int GetEnergy()
    {
        return energy;
    }
    public void SetEnergy(int _energy)
    {
       energy -= _energy;
        UpdateEnergUI();
    }
    public int GetMilitary_strength()
    {
        return military_strength;
    }
    public void SetMilitary_strength(int size)
    {
        military_strength+=size;
        UpdateMilitaryStrengthUI();
    }
    public int GetMaximum_troop_strength()
    {
        return maximum_troop_strength;
    }
    public void SetMaximum_troop_strength(int size)
    {
        maximum_troop_strength += size;
        UpdateMilitaryStrengthUI();
    }
    public int GetConstruction()
    {
        return construction;
    }
    public void SetConstruction(int size)
    {
        construction += size;
        UpdateConstructionUI();
    }
    public int GetMaximum_project()
    {
        return maximum_project;
    }
    public void SetMaximum_project(int size)
    {
        maximum_project += size;
        UpdateConstructionUI();
    }
    public int GetTotal_construction()
    {
        return total_construction;
    }
    public int GetTrained_personnel()
    {
        return trained_personnel;
    }
    public int GetDestroy_construction()
    {
        return destroy_construction;
    }
    public  int GetEnemy_eliminated()
    {
        return enemy_eliminated; 
    }
    public void SetTotal_construction()
    {
        total_construction++;
    }
    public void SetTrained_personnel()
    {
        trained_personnel++;
    }
    public void SetDestroy_construction()
    {
        destroy_construction++;
    }
    public void SetEnemy_eliminated()
    {
        enemy_eliminated++;
    }
}
