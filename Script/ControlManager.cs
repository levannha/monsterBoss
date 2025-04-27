using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance{ get; set; }
    private GameObject information;
    [SerializeField] private Image information_ui;
    [SerializeField] private LayerMask Enemy;
    [SerializeField] private LayerMask ClickAble;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image HpBar;
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button delete;
    [SerializeField] private Button repair;
    [SerializeField] private Button[] buy;
    [SerializeField] private Button offInformation;
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private GameObject[] unit;
    [SerializeField] private Canvas minimapIcon;
    [SerializeField] private Sprite iconUnit;
    [SerializeField] private Sprite iconHouse;
    private AudioSource source;

    private int[] index = {0,0,0};
    private bool tower =false;
    private void Awake()
    {
        if (Instance != null&& Instance != this)
        {
            Destroy(gameObject);

        }  else
        {
            Instance = this;
        }
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        delete.onClick.AddListener(()=>DeleteInformation());
        repair.onClick.AddListener(()=> LoadRepair());
        buy[0].onClick.AddListener(() =>CreateUnitOHouse(0));
        buy[1].onClick.AddListener(() => CreateUnitOHouse(1));
        buy[2].onClick.AddListener(() => CreateUnitOHouse(2));
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) &&!EventSystem.current.IsPointerOverGameObject())
            {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,Enemy|ClickAble, QueryTriggerInteraction.Ignore))
            {
                information = hit.collider.gameObject;
                
            }
            
        }
        if (information)
        {
            PlayMusic();
            DisplayInformation();
    
        } else
        {
            offInformation.GetComponent<Image>().color = Color.white;
            OffInfomation();
        }

    }
    public void PlayMusic()
    {

        if (!source.isPlaying)
        {
        if (information.CompareTag("house") || information.CompareTag("tower"))
        {
                if (information.GetComponent<BuildSystem>().enabled)
                {
                    source.PlayOneShot(information.GetComponent<HouseController>().building);
                }
                else
                {
                    source.PlayOneShot(information.GetComponent<HouseController>().music);
                }
        }

        else if (information.CompareTag("unit"))
        {

            if (information.GetComponent<Animator>().GetBool("isAttack"))
            {
                if (information.GetComponent<Animator>().GetFloat("attack") == 0)
                {
                    source.PlayOneShot(information.GetComponent<UnitController>().PlayAttack());
                }
                else
                {
                    source.PlayOneShot(information.GetComponent<UnitController>().PlayShoot());
                }
            }
            else if (information.GetComponent<Animator>().GetBool("isWalk"))
            {
                source.PlayOneShot(information.GetComponent<UnitController>().PlayWail());
            }
            else
            {
                source.PlayOneShot(information.GetComponent<UnitController>().PlayIdle());
            }


        }
    }
        
    }
    public void CreateUnitOHouse(int _lv)
    {
        if (tower )
        {
            gridSystem.StartPlacingBuilding(index[_lv]);
        } else
        {
            information.GetComponent<HouseController>().TrySpawnUnit(unit[index[_lv]],_lv);
        }
    }
   public GameObject[] GetUnit()
    {
        return unit;
    }
    public void DisplayInformation()
    {
        information_ui.gameObject.SetActive(true);
        if (information.layer == 7)
        {
            TeamPlayer();
            offInformation.GetComponent<Image>().color = Color.cyan;
        }
      else if (information.layer==8)
        {
            TeamEnemy();
            offInformation.GetComponent<Image>().color = Color.red;
        }
    }
    public void TeamPlayer()
    {
        if(information.CompareTag("house")|| information.CompareTag("tower") )
        {
            ObjectData data = DatabaseManager.Instance.GetObjectDataId(information.GetComponent<HouseController>().GetId());
            avatar.sprite = data.avatar;
            name.text = data.Name;
            name.color = Color.green;
            HpBar.fillAmount = (float)information.GetComponent<HouseController>().GetHp() / (float)information.GetComponent<HouseController>().GetHpRoot();
            Hp.text = information.GetComponent<HouseController>().GetHp() + "/" + information.GetComponent<HouseController>().GetHpRoot();
            HpBar.color = Color.green;
            description.text = data.description;
            //button
            if (information.GetComponent<BuildSystem>().GetComplete())
            {
               
                delete.gameObject.SetActive(true);
                if (information.GetComponent<HouseController>().GetHp()< information.GetComponent<HouseController>().GetHpRoot())
                {
                    repair.gameObject.SetActive(true);
                    if (information.GetComponent<BuildSystem>().GetRepair())
                    {
                        repair.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        repair.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    }
                } else
                {
                    repair.gameObject.SetActive(false);
                }
               
              
                
                for (int i = 0; i < buy.Length; i++)
                {
                    if (i < data.PrefabUnit.Length && i <GameManager.Instance.GetLv())
                    {
                        buy[i].gameObject.SetActive(true);
                        ObjectData objectData = DatabaseManager.Instance.GetObjectDataId(information.GetComponent<HouseController>().GetId());
                        if (objectData.tower)
                        {
                            ObjectData dataUnit = DatabaseManager.Instance.GetObjectDataId(objectData.PrefabUnit[i]);
                            tower = true;
                            buy[i].transform.GetChild(0).GetComponent<Image>().sprite = dataUnit.avatar;
                            buy[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
                            if (ResourceManager.Instance.GetEnergy() >= dataUnit.energy && ResourceManager.Instance.GetMinerals() >= dataUnit.minerals)
                            {
                                if (ResourceManager.Instance.GetConstruction() < ResourceManager.Instance.GetMaximum_project())
                                {
                                    buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                                    buy[i].GetComponent<Button>().enabled = true;
                                    index[i] = dataUnit.ID;
                                } else
                                {
                                    buy[i].GetComponent<Button>().enabled = false;
                                    buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                                }
                            } else
                            {
                                buy[i].GetComponent<Button>().enabled = false;
                                buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                            }
                        } else
                        {
                            tower = false;
                            UnitController dataUnit = unit[objectData.PrefabUnit[i]].GetComponent<UnitController>();

                            buy[i].transform.GetChild(0).GetComponent<Image>().sprite = dataUnit.GetAvatar() ;
                            buy[i].transform.GetChild(0).GetComponent<Image>().fillAmount = (information.GetComponent<HouseController>().GetTimeSkillRoot()[i]- information.GetComponent<HouseController>().GetTimeSkill()[i] )/ information.GetComponent<HouseController>().GetTimeSkillRoot()[i];
                            if (information.GetComponent<HouseController>().GetTimeSkill()[i]<=0 && ResourceManager.Instance.GetEnergy() >= dataUnit.GetEnergy() && ResourceManager.Instance.GetMinerals() >= dataUnit.GetMinerals() )
                            {
                                if (ResourceManager.Instance.GetMilitary_strength()< ResourceManager.Instance.GetMaximum_troop_strength())
                                {
                                    buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                                    buy[i].GetComponent<Button>().enabled = true;
                                    index[i] = objectData.PrefabUnit[i];
                                } else
                                {
                                    buy[i].GetComponent<Button>().enabled = false;
                                    buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                                }
                               
                            }
                            else
                            {
                                buy[i].GetComponent<Button>().enabled = false;
                                buy[i].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                            }
                        }

                    } else
                    {
                        buy[i].gameObject.SetActive(false);
                    }
                }
            } else
            {

                delete.gameObject.SetActive(false);
                repair.gameObject.SetActive(false);
                for (int i = 0; i < buy.Length; i++)
                {
                  
                        buy[i].gameObject.SetActive(false);
                    
                }
            }
        } else
        {
            UnitController unit = information.GetComponent<UnitController>();
            avatar.sprite = unit.GetAvatar();
            name.text = unit.GetName();
            name.color = Color.green;
            HpBar.fillAmount = (float)unit.GetHp() / (float)unit.GetHpRoot();
            Hp.text = (float)unit.GetHp() +"/" +(float)unit.GetHpRoot(); ;
            HpBar.color = Color.green;
            description.text = unit.GetDescription();
            delete.gameObject.SetActive(true);
            repair.gameObject.SetActive(false);
            for (int i = 0; i < buy.Length; i++)
            {
               
                    buy[i].gameObject.SetActive(false);
                
            }
        }
    }
    public void TeamEnemy()
    {
        if (information.CompareTag("house")|| information.CompareTag("tower"))
        {
            ObjectData data = DatabaseManager.Instance.GetObjectDataId(information.GetComponent<HouseController>().GetId());
            avatar.sprite = data.avatar;
            name.text = data.Name;
            name.color = Color.red;
            HpBar.fillAmount = (float)information.GetComponent<HouseController>().GetHp() / (float)information.GetComponent<HouseController>().GetHpRoot();
            Hp.text = information.GetComponent<HouseController>().GetHp() + "/" + information.GetComponent<HouseController>().GetHpRoot();
            HpBar.color = Color.red;
            description.text = data.description;
           
            delete.gameObject.SetActive(false);
            repair.gameObject.SetActive(false);
            for (int i = 0; i < buy.Length; i++)
            {

                buy[i].gameObject.SetActive(false);

            }
        }
        else
        {
            UnitController unit = information.GetComponent<UnitController>();
            avatar.sprite = unit.GetAvatar();
            name.text = unit.GetName();
            name.color = Color.red;
            HpBar.fillAmount = (float)unit.GetHp() / (float)unit.GetHpRoot();
            Hp.text = (float)unit.GetHp() + "/" + (float)unit.GetHpRoot(); ;
            HpBar.color = Color.red;
            description.text = unit.GetDescription();
            delete.gameObject.SetActive(false);
            repair.gameObject.SetActive(false);
            for (int i = 0; i < buy.Length; i++)
            {

                buy[i].gameObject.SetActive(false);

            }
        }
    }
    public void OffInfomation()
    {
        information_ui.gameObject.SetActive(false);
        information = null;
    }
    public void DeleteInformation()
    {
       
        if (information.CompareTag("house")|| information.CompareTag("tower")) {
            information.GetComponent<HouseController>().DestroyUnit();
        } else
        {
            information.GetComponent<UnitController>().DestroyUnit();
        }
        OffInfomation();
    }
    public void LoadRepair()
    {
        if (information.CompareTag("house")&&!information.GetComponent<BuildSystem>().GetRepair() || information.CompareTag("tower") && !information.GetComponent<BuildSystem>().GetRepair())
        {
           
            information.GetComponent<BuildSystem>().enabled = true;
            information.GetComponent<BuildSystem>().SetRepair();
           
        }
       
    }
   public GridSystem GetGridSystem()
    {
        return gridSystem;
    }
    public Sprite GetIconUnit()
    {
        return iconUnit;
    }
    public Sprite GetIconHouse()
    {
        return iconHouse;
    }
    public void CreateMiniMap(GameObject _target)
    {
        GameObject newIconMiniMap = Instantiate(minimapIcon.gameObject, new Vector3(_target.transform.position.x, 70f, _target.transform.position.z),Quaternion.identity);
        newIconMiniMap.GetComponent<UnitIconMinimap>().SetUnit(_target);
        if (_target.GetComponent<HouseController>())
        {
            _target.GetComponent<HouseController>().SetMiniMapIcon(newIconMiniMap);
        }
    }
}
