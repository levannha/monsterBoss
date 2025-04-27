using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance { get; set; }
    public Grid grid; // Kéo Grid từ Inspector vào
    [SerializeField] private Image buidlingEdit;
    public LayerMask layerGround;
    public LayerMask layerEnemy;
    public LayerMask layerClickAble;
    private GameObject currentBuilding;
    private int _id;
    public Material _material;
    private Dictionary<Vector3Int, GameObject> placedBuildings = new Dictionary<Vector3Int, GameObject>();
    private void Awake()
    {
        if (Instance != null&& Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }
    void Update()
    {
        if (currentBuilding != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3Int cellPosition = grid.WorldToCell(mousePosition);
            currentBuilding.transform.position = grid.GetCellCenterWorld(cellPosition) +  new Vector3(0, currentBuilding.GetComponent<HouseController>().GetDepth(), 0);
           
           Vector2Int size = DatabaseManager.Instance.GetObjectsData()[_id].Size;
            bool canPlace = IsAreaAvailable(cellPosition, size, currentBuilding);
            Color _green = Color.green;
            if (currentBuilding.CompareTag("tower"))
            {
                currentBuilding.GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;
                if (currentBuilding.transform.GetChild(0).gameObject.activeSelf) { 
                currentBuilding.transform.GetChild(0).GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;

                    if (currentBuilding.transform.GetChild(0).transform.childCount > 0)
                    {
                        for (int i = 0; i < currentBuilding.transform.GetChild(0).transform.childCount; i++)
                        {
                            currentBuilding.transform.GetChild(0).transform.GetChild(i).GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;
                            if (currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.childCount > 0)
                            {
                                for (int j = 0; j < currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.childCount; j++)
                                {
                                    currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.GetChild(j).GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;
                                }

                            }

                        }
                    }
                }
            }
            else
            {
                currentBuilding.GetComponent<Renderer>().material.color = canPlace ? Color.green : Color.red;
            }
            // Rotate Building
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotateLeft();
            } else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateRight();
            }
            if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject()) // Click chuột để đặt công trình
            {
                PlaceBuilding(cellPosition,size,mousePosition);
            }
        }
    }

    public void StartPlacingBuilding(int id)
    {

        if (id == 0 || id!= 0 && ResourceManager.Instance.GetConstruction() < ResourceManager.Instance.GetMaximum_project())
        {
            if (id == 0 && !GameManager.Instance.GetIsMain() || id > 0 && GameManager.Instance.GetIsMain() && GameManager.Instance.GetMainHouseBeenCompleted())
            {
                if (!currentBuilding  &&ResourceManager.Instance.GetMinerals() >= DatabaseManager.Instance.GetObjectsData()[id].minerals && ResourceManager.Instance.GetEnergy() >= DatabaseManager.Instance.GetObjectsData()[id].energy)
                {
                    _id = id;
                    currentBuilding = Instantiate(DatabaseManager.Instance.GetObjectsData()[id].Prefab);
                    buidlingEdit.gameObject.SetActive(true);
                    if (currentBuilding.CompareTag("tower"))
                    {
                        currentBuilding.GetComponent<Renderer>().material = _material;
                        if (currentBuilding.transform.GetChild(0).gameObject.activeSelf)
                        {
                            currentBuilding.transform.GetChild(0).GetComponent<Renderer>().material = _material;
                            if (currentBuilding.transform.GetChild(0).transform.childCount > 0)
                            {
                                for (int i = 0; i < currentBuilding.transform.GetChild(0).transform.childCount; i++)
                                {
                                    currentBuilding.transform.GetChild(0).transform.GetChild(i).GetComponent<Renderer>().material = _material;
                                    if (currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.childCount > 0)
                                    {
                                        for (int j = 0; j < currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.childCount; j++)
                                        {
                                            currentBuilding.transform.GetChild(0).transform.GetChild(i).transform.GetChild(j).GetComponent<Renderer>().material = _material;
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        currentBuilding.GetComponent<Renderer>().material = _material;
                    }
                }
            }
        }
    }

    void PlaceBuilding(Vector3Int cellPosition, Vector2Int size,Vector3 mousePosition)
    {
        bool canPlace = IsAreaAvailable(cellPosition, size, currentBuilding);
       
        if (!placedBuildings.ContainsKey(cellPosition)&&canPlace ) // Kiểm tra vị trí trống
            {
           
            GameObject newBuilding = Instantiate(DatabaseManager.Instance.GetObjectsData()[_id].Prefab, grid.GetCellCenterWorld(cellPosition) + new Vector3(0, currentBuilding.GetComponent<HouseController>().GetDepth(), 0), currentBuilding.transform.rotation);
           
            newBuilding.GetComponent<HouseController>().SetPositionBuildSystem(mousePosition);
            newBuilding.GetComponent<NavMeshObstacle>().enabled = true;
            newBuilding.GetComponent<BuildSystem>().enabled = true;
            
            if (newBuilding.GetComponent<MainController>())
            {
                newBuilding.GetComponent<MainController>().SetMain();
            } else
            {
                ResourceManager.Instance.SetConstruction(1);
            }
            SoundManager.Instance.PlayBuilding();
            newBuilding.GetComponent<HouseController>().SetId(_id);
            ResourceManager.Instance.SetMinerals( DatabaseManager.Instance.GetObjectsData()[_id].minerals);
            ResourceManager.Instance.SetEnergy(DatabaseManager.Instance.GetObjectsData()[_id].energy);
            Save(newBuilding, size, cellPosition);
            DeleteBuilding();
            GameManager.Instance.AddHouse(newBuilding);
            ResourceManager.Instance.SetTotal_construction();
            ControlManager.Instance.CreateMiniMap(newBuilding);
        }
         else
        {
            DeleteBuilding();
        }
        
    }
    public void Save(GameObject newBuilding,Vector2Int size,Vector3Int cellPosition)
    {
        placedBuildings[cellPosition] = newBuilding;
        

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int occupiedCell = cellPosition + new Vector3Int(x, 0, y);
                placedBuildings[occupiedCell] = newBuilding;
            }
        }
    }
    public void RotateLeft()
    {
        if (currentBuilding.CompareTag("tower"))
        {
            currentBuilding.transform.Rotate(Vector3.back, 90f);
        }
        else
        {
            currentBuilding.transform.Rotate(Vector3.up, 90f);
        }
    }
    public void RotateRight()
    {
        if (currentBuilding.CompareTag("tower"))
        {
            currentBuilding.transform.Rotate(Vector3.forward, 90f);
        }
        else
        {
            currentBuilding.transform.Rotate(Vector3.up, 90f);
        }
    }
    public void DeleteBuilding()
    {
        buidlingEdit.gameObject.SetActive(false);
        Destroy(currentBuilding);
        currentBuilding = null;
    }
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity,layerGround,QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
  
bool IsAreaAvailable(Vector3Int cellPosition, Vector2Int size,GameObject currentBuilding)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int occupiedCell = cellPosition + new Vector3Int(x, 0, y);

                // Nếu ô đã bị chiếm bởi công trình khác
                if (placedBuildings.ContainsKey(occupiedCell))
                    return false;
            }
        }

        // **Kiểm tra có nhân vật trong khu vực không**
        Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);
        Vector3 boxSize = new Vector3(size.x, 1, size.y); // Độ cao mặc định 1
        Collider[] colliders = Physics.OverlapBox(worldPosition, boxSize / 2, Quaternion.identity,layerEnemy|layerClickAble,QueryTriggerInteraction.Ignore);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("unit")) 
            {
               
                return false;
            }
        }
        // kiểm tra địa hình 
        
        NavMeshHit hit;
        return NavMesh.SamplePosition(currentBuilding.transform.position, out hit,1.0f, NavMesh.AllAreas) && FogOfWarManager.Instance.CanBuildAt(currentBuilding.transform.position);
        
    }
    public void RemoveBuildingSystem(Vector3 positionBuiding,GameObject building)
    {
        Vector3Int cellPosition = grid.WorldToCell(positionBuiding);

        if (placedBuildings.TryGetValue(cellPosition, out building))
        {
            Vector2Int size = DatabaseManager.Instance.GetObjectsData()[_id].Size;

           

            // Giải phóng các ô đã bị chiếm
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int occupiedCell = cellPosition + new Vector3Int(x, 0, y);
                    placedBuildings.Remove(occupiedCell);
                }
            }
        }
    }
    public GameObject GetCurrentBuilding()
    {
        return currentBuilding;
    }
    public Vector3Int GetCellPosition(Vector3 mousePosition)
    {
        
       return grid.WorldToCell(mousePosition);
    }
    public bool CheckLocationConstructionSite(Vector3 mousePosition,Vector2Int size,GameObject currentBuilding)
    {
        return IsAreaAvailable(GetCellPosition( mousePosition),size, currentBuilding) && !placedBuildings.ContainsKey(GetCellPosition(mousePosition));
    }
}
