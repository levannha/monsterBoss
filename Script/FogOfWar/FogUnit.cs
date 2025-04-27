using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogUnit : MonoBehaviour
{
    
    public float visionRange;

    private void Start()
    {
        FogOfWarManager.Instance.RegisterUnit(this);
    }

  
}
