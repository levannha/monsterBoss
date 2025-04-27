using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    Camera _cam;
    [SerializeField] private Image  hp_bar;
    
    private void Start()
    {
        _cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (_cam)
        {
            transform.LookAt(_cam.transform.position);
        }
    }
    public void TakeDamageUi()
    {
        if (transform.parent.GetComponent<HouseController>())
        {
            hp_bar.fillAmount =  transform.parent.GetComponent<HouseController>().HPRatio();
        }
        else
        {
            hp_bar.fillAmount = transform.parent.GetComponent<UnitController>().HPRatio();
        }
       
    }
    public void UpdateBullets(int bullets)
    {
        for (int i = 0;i<9;i++)
        {
            if (i < bullets) {
                transform.GetChild(1).transform.GetChild(i).gameObject.SetActive(true);
            } else
            {
                transform.GetChild(1).transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }
}
