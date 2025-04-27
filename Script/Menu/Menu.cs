using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public static Menu Instance { get; set; }
    [SerializeField] private GameObject[] units;
    [SerializeField] private Image map;
    [SerializeField] private Canvas Upgrade;
    [SerializeField] private GameObject effect_damage;
    [SerializeField] private GameObject effect_hp;
    [SerializeField] private GameObject effect_skill;
    [SerializeField] private TextMeshProUGUI key;
    [SerializeField] private TextMeshProUGUI chip;
    [SerializeField] private TextMeshProUGUI up_damge;
    [SerializeField] private TextMeshProUGUI up_hp;
    [SerializeField] private TextMeshProUGUI up_time_skill;
    private int bonus_damage = 2;
    private int extra_resistance = 20;
    private float reduced_reload_time = 0.25f;
    private int index = 0;
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
        LoadResourceGame();
    }
    public void LoadResourceGame()
    {
        MainData data = SaveGameManager.Instance.LoadGame();
        chip.text = data.chip + "";
        key.text = data.key + "";
    }
    public string GetName()
    {
        string[] unitNames = { "dagger", "heavy", "chiken", "bug", "trex" };
        if (index < 0 || index >= unitNames.Length) return "";

       return unitNames[index];
    }
    private void LoadUiUpgrateUnit()
    {
       
        MainData coid = SaveGameManager.Instance.LoadGame();
        DataUnit data = SaveGameManager.Instance.LoadUpgradeUnit(GetName());
        
        up_damge.text = "Sát thương (" + data.chip_up_damage + "): " + data.damage + " >>> " + (data.damage+bonus_damage);
        if (coid.chip < data.chip_up_damage)
        {
            up_damge.color = Color.red;
        } else
        {
            up_damge.color = Color.cyan;
        }
        up_hp.text = "Độ bền (" + data.chip_up_hp + "): " + data.hp + " >>> " + (data.hp + extra_resistance);
        if (coid.chip < data.chip_up_hp)
        {
            up_hp.color = Color.red;
        }
        else
        {
            up_hp.color = Color.cyan;
        }
        up_time_skill.text = "Độ bền (" + data.chip_up_time_skill + "): " + data.time_skill + " >>> " + (data.time_skill - reduced_reload_time);
        if (coid.chip < data.chip_up_time_skill)
        {
            up_time_skill.color = Color.red;
        }
        else
        {
            up_time_skill.color = Color.cyan;
        }
    }
    public void PlayFast() {
        SceneManager.LoadScene("RTSGame");
    }
    public void OnMap()
    {
        map.gameObject.SetActive(true);
    }
    public void OffMap()
    {
        map.gameObject.SetActive(false);
    }
    public void OnUpgrade()
    {
        Upgrade.gameObject.SetActive(true);
        LoadUiUpgrateUnit();

    }
    public void OffUpgrade()
    {
        Upgrade.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void UpgraDamage()
    {
        MainData coid = SaveGameManager.Instance.LoadGame();
        DataUnit unit = SaveGameManager.Instance.LoadUpgradeUnit(GetName());
        if (coid.chip >= unit.chip_up_damage)
        {

            unit.damage+=bonus_damage;
            unit.chip_up_damage *= 2;
            coid.chip -= unit.chip_up_damage;
            SaveGameManager.Instance.SaveGame(coid);
            SaveGameManager.Instance.SaveUpgradeUnit(GetName(), unit);
            effect_damage.SetActive(true);

            StartCoroutine(OffEffect(effect_damage));
            LoadResourceGame();
            LoadUiUpgrateUnit();
        }
       
    }
    public void UpgraHp()
    {
        MainData coid = SaveGameManager.Instance.LoadGame();
        DataUnit unit = SaveGameManager.Instance.LoadUpgradeUnit(GetName());
        if (coid.chip >= unit.chip_up_hp)
        {

            unit.hp +=extra_resistance;
            unit.chip_up_hp *= 2;
            coid.chip -= unit.chip_up_hp;
            SaveGameManager.Instance.SaveGame(coid);
            SaveGameManager.Instance.SaveUpgradeUnit(GetName(), unit);
            effect_hp.SetActive(true);
            StartCoroutine(OffEffect(effect_hp));
            LoadResourceGame();
            LoadUiUpgrateUnit();
        }

    }
    public void UpgraSkill()
    {
        MainData coid = SaveGameManager.Instance.LoadGame();
        DataUnit unit = SaveGameManager.Instance.LoadUpgradeUnit(GetName());
        if (coid.chip >= unit.chip_up_time_skill)
        { 
            unit.time_skill -= reduced_reload_time;
            unit.chip_up_time_skill *= 2;
            coid.chip -= unit.chip_up_time_skill;
            SaveGameManager.Instance.SaveGame(coid);
            SaveGameManager.Instance.SaveUpgradeUnit(GetName(), unit);
            effect_skill.SetActive(true);
            StartCoroutine(OffEffect(effect_skill));
            LoadResourceGame();
            LoadUiUpgrateUnit();
        }
    }
    public IEnumerator OffEffect(GameObject effect)
    {
        yield return new WaitForSeconds(2f);
        effect.SetActive(false);
    }
    public void NextUnit()
    {
        if(index == units.Length-1)
        {
            index = 0;
        } else
        {
            index++;
        }
        for(int i=0;i<units.Length;i++)
        {
            if(i==index)
            {
                units[i].SetActive(true);
            } else
            {
                units[i].SetActive(false);
            }
        }
        LoadUiUpgrateUnit();
    }
}
