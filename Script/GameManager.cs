using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
     public static GameManager Instance { get; set; }
     private int lv = 1;
    private int UpdateLv = 500;
    private float UpdateTime = 0;
     [SerializeField] private Button[] buyHouse;
    [SerializeField] private Sprite disable;
    [SerializeField] private Sprite[] icon;
    [SerializeField] private Button updateButton;
    [SerializeField] private Image updateBar;
    private bool isMain = false;
    private bool mainHouseBeenCompleted = false;
    private List<GameObject> unit;
    private List<GameObject> house;
    [SerializeField] private Transform center;
    private List<DataUnit> data;
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
        data = new List<DataUnit>();
        data.Add(SaveGameManager.Instance.LoadUpgradeUnit("dagger"));
        data.Add(SaveGameManager.Instance.LoadUpgradeUnit("heavy"));
        data.Add(SaveGameManager.Instance.LoadUpgradeUnit("chiken"));
        data.Add(SaveGameManager.Instance.LoadUpgradeUnit("bug"));
        data.Add(SaveGameManager.Instance.LoadUpgradeUnit("trex"));
        unit = new List<GameObject>();
        house = new List<GameObject>();
        UpdateButton();
        UpdateLvBar();
        TrySpawnUnit(ControlManager.Instance.GetUnit()[0]);
    }
    public DataUnit GetDataUnit(int index)
    {
        return data[index];
    }
    private void FixedUpdate()
    {
        UpdateLvBar();
        UpdateButton();
    }
    public void UpdateLvBar()
    {
        if (UpdateTime > 0)
        {
           
            UpdateTime -= Time.deltaTime;
            updateBar.fillAmount = (float)(10f * lv - UpdateTime) / (10f * lv);
            if (UpdateTime <= 0)
            {
                lv++;
                UpdateLv *= lv;
                updateBar.gameObject.SetActive(false);
                ResourceManager.Instance.SetMaximum_troop_strength(15);
                ResourceManager.Instance.SetMaximum_project(15);
                if  (lv < 3 && isMain) {
                    updateButton.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (ResourceManager.Instance.GetMinerals() >= UpdateLv && lv < 3)
            {
                updateButton.GetComponent<Image>().color = Color.cyan;
            }
            else if (ResourceManager.Instance.GetMinerals()< UpdateLv && lv < 3)
            {
                updateButton.GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void UpLv()
    {
        if (isMain && mainHouseBeenCompleted && ResourceManager.Instance.GetMinerals()>= UpdateLv && lv <3)
        {
            ResourceManager.Instance.SetMinerals(UpdateLv);
            UpdateTime = lv * 10f;
            updateButton.gameObject.SetActive(false);
            updateBar.fillAmount = (float)(10f * lv - UpdateTime) / (10f * lv);
            updateBar.gameObject.SetActive(true);
        }
    }
    public void UpdateButton()
    {

       if (isMain)
        {
            buyHouse[0].transform.GetChild(0).GetComponent<Image>().sprite = disable;
            buyHouse[0].transform.GetChild(0).GetComponent<Image>().color = Color.red;
            buyHouse[0].GetComponent<Button>().enabled = false;
        } else
        {
            buyHouse[0].transform.GetChild(0).GetComponent<Image>().sprite = icon[0];
            buyHouse[0].GetComponent<Button>().enabled = true;
            ObjectData main = DatabaseManager.Instance.GetObjectDataId(0);
            if (ResourceManager.Instance.GetMinerals()>= main.minerals )
            {
                buyHouse[0].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            } else
            {
                buyHouse[0].transform.GetChild(0).GetComponent<Image>().color = Color.white;

            }
           
        } 
        if (isMain && mainHouseBeenCompleted)
        {
            
           List<ObjectData> main = DatabaseManager.Instance.GetObjectsData();
          

            for (int i = 1; i < buyHouse.Length; i++)
            {
                if (lv ==1&& i < 4 || lv == 2 && i < 6|| lv == 3 && i < 7)
                {
                  
                        buyHouse[i].transform.GetChild(0).GetComponent<Image>().sprite = icon[i];
                    if (ResourceManager.Instance.GetMinerals() >= main[i].minerals && ResourceManager.Instance.GetEnergy() >= main[i].energy)
                    {
                        buyHouse[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                        buyHouse[i].GetComponent<Button>().enabled = true;
                    }
                    else
                    {
                        buyHouse[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                        buyHouse[i].GetComponent<Button>().enabled = false;
                    }

                }
                else
                {
                     buyHouse[i].transform.GetChild(0).GetComponent<Image>().sprite = disable;
                    buyHouse[i].GetComponent<Button>().enabled = false;
                }

            }
        } else
        {
            for(int i = 1;i<buyHouse.Length;i++)
            {
                buyHouse[i].transform.GetChild(0).GetComponent<Image>().sprite = disable;
                buyHouse[i].GetComponent<Button>().enabled = false;
            }
        } 
    }
    public void SetIsMain(bool status)
    {
        if (!status && updateButton)
        {
            updateButton.gameObject.SetActive(status);
        }
        isMain = status;
       
    }
    public void OnUpdate()
    {
        updateButton.gameObject.SetActive(true);
    }
    public bool GetIsMain()
    {
       
      return  isMain;

    }
    public void SetMainHouseBeenCompleted(bool status)
    {

        mainHouseBeenCompleted = status;
      
    }
    public bool GetMainHouseBeenCompleted()
    {

        return mainHouseBeenCompleted;

    }
    public int GetLv()
    {
        return lv; 
    }
    public void AddUnit(GameObject _unit)
    {
        unit.Add(_unit);
    }
    public void AddHouse(GameObject _house)
    {
        house.Add(_house);
    }
    public void RemoteUnit(GameObject _unit)
    {
        unit.Remove(_unit);
    }
    public void RemoteHouse(GameObject _house)
    {
        house.Remove(_house);
    }
    public List<GameObject> GetUnit()
    {
        return unit;
    }
    public List<GameObject> GetHouse()
    {
        return house;
    }
    public void TrySpawnUnit(GameObject unitCost)
    {

       

        GameObject unit = Instantiate(unitCost, transform.position, Quaternion.identity);
       
        unit.GetComponent<FogUnit>().enabled = true;
        ResourceManager.Instance.SetMilitary_strength(1);
        AddUnit(unit);
        ControlManager.Instance.CreateMiniMap(unit);
        ResourceManager.Instance.SetTrained_personnel();
    }
    public Transform GetCenter()
    {
        return center;
    }
}
