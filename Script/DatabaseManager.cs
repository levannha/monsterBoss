using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
   [SerializeField] private ObjectsDatabseSO data;
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
    public  ObjectData GetObjectDataId(int id)
    {
        return data.GetObjectByID(id);
    }
    public List<ObjectData> GetObjectsData()
    {
        return data.objectsData;
    }
}
