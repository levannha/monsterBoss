using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class MainData
{
    public int key = 0;
    public int chip = 0;
}
[System.Serializable]
public class DataUnit
{
    public string name;
    public int hp;
    public int chip_up_hp;
    public int damage;
    public int chip_up_damage;
    public float time_skill;
    public int chip_up_time_skill;
    public void CreateUnit(string name,int hp,int chip_up_hp, int damage, int chip_up_damage, float time_skill, int chip_up_time_skill)
    {
        this.name = name;
        this.hp = hp;
        this.chip_up_hp = chip_up_hp;
        this.damage = damage;
        this.chip_up_damage = chip_up_damage;
        this.time_skill = time_skill;
        this.chip_up_time_skill = chip_up_time_skill;

    }
}
    public class SaveGameManager : MonoBehaviour
{

    

    public static SaveGameManager Instance { get; set; }
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
   public MainData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MainData data = JsonUtility.FromJson<MainData>(json);
           
            return data;
        }

        MainData newData = new MainData();
        newData.key = 1;
        newData.chip = 500;
        SaveGame(newData);
        return newData;
    }
    public DataUnit NewDataUnit(string name)
    {
        DataUnit unit = new DataUnit();
        if (name == "dagger")
        {
            unit.CreateUnit("dagger",500,50,30,30,25,100);
        } else if (name == "heavy")
        {
            unit.CreateUnit( "heavy", 1200, 70,40, 50, 20, 100);
        } else if (name == "chiken")
        {
            unit.CreateUnit( "chiken", 300, 30,30, 50, 5, 100);
        } else if (name == "bug")
        {
            unit.CreateUnit( "bug", 700, 35, 35, 60,8, 120);
        } else  if (name == "trex")
        {
            unit.CreateUnit( "trex", 2000, 100, 100, 120, 10, 150);
        }

        SaveUpgradeUnit(name, unit);
        return unit;
    }
    public DataUnit LoadUpgradeUnit(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".json";
     

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
           
            DataUnit data = JsonUtility.FromJson<DataUnit>(json);
            return data;
        }

        return NewDataUnit(name);
        
    }
    public void SaveGame(MainData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public void SaveUpgradeUnit(string name,DataUnit data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/"+name+".json", json);
       
    }
    public void DeleteAllSaveFiles()
    {
        string[] unitNames = { "dagger", "heavy", "chiken", "bug", "trex" };
        foreach (string name in unitNames)
        {
            string unitPath = Path.Combine(Application.persistentDataPath, name + ".json");
            if (File.Exists(unitPath))
            {
                File.Delete(unitPath);
               
            }
        }

        string mainPath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(mainPath))
        {
            File.Delete(mainPath);
           
        }
    }
}
