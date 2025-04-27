using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{


    


    public void SetMain()
    {
        GameManager.Instance.SetIsMain(true);
       
    }
     
    private void OnDestroy()
    {
        if (GetComponent<HouseController>().GetId() != -1)
        {
            if (gameObject.layer == 7)
            {
                GameManager.Instance.SetMainHouseBeenCompleted(false);
                GameManager.Instance.SetIsMain(false);
                ResourceManager.Instance.SetMaximum_troop_strength(-10);
                ResourceManager.Instance.SetMaximum_project(-10);
            } else if (gameObject.layer == 8)
            {
                MainBoss.Instance.SetMainHouseBeenCompleted(false);
                ResourceBoss.Instance.SetMaximum_troop_strength(-10);
                ResourceBoss.Instance.SetMaximum_project(-10);
            }
        }
    }
  

}
