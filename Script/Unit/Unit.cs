using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.layer ==7)
        {
            UnitSelectManager.Instance.AllUnit.Add(gameObject);
            
        }
    }

    private void OnDestroy()
    {
        UnitSelectManager.Instance.AllUnit.Remove(gameObject);
    }
}
