using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{ 
   [SerializeField] public float time;
   
   public void PlayIncome()
    {
        StartCoroutine(DoingBusiness());
       
    }
    
   public IEnumerator DoingBusiness()
    {
        yield return new WaitForSeconds(time);
        if (gameObject.layer ==7)
        {
            ResourceManager.Instance.SetMinerals(-1);
            StartCoroutine(DoingBusiness());
        }
        else if (gameObject.layer == 8)
        {

            ResourceBoss.Instance.SetMinerals(-1);
            StartCoroutine(DoingBusiness());

        }
    }

}
