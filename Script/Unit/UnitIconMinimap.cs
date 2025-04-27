using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitIconMinimap : MonoBehaviour
{
    private Transform unit;

    // Update is called once per frame
    void FixedUpdate()
    {
       if (unit)
        {
            transform.position = new Vector3(unit.position.x, 70f, unit.position.z);
        } else
        {
            Destroy(gameObject);
        }
    }
    public void SetUnit(GameObject _unit)
    {
        unit = _unit.transform;
        if (_unit.GetComponent<HouseController>())
        {
            transform.GetChild(0).GetComponent<Image>().sprite = ControlManager.Instance.GetIconHouse();
            if (_unit.layer==7)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            } else if (_unit.layer ==  8)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
            GetComponent<UnitIconMinimap>().enabled = false;
        } else if (_unit.GetComponent<UnitController>())
        {
            transform.GetChild(0).GetComponent<Image>().sprite = ControlManager.Instance.GetIconUnit();
            if (_unit.layer == 7)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            }
            else if (_unit.layer == 8)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
        }
    }
}
