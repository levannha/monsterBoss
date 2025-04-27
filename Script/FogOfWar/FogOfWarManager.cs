using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public static FogOfWarManager Instance { get; set; }
    public float currentVisionRadius = 10f;
    public float memoryRadiusWorld = 15f;

    public int textureSize = 256;
    public float worldSize = 1000f;
    public Texture2D fogTexture;
    private Color32[] fogPixels;

    private List<FogUnit> units = new List<FogUnit>();
    private void Awake()
    {
       if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
        fogTexture = new Texture2D(textureSize, textureSize, TextureFormat.Alpha8, false);
        fogTexture.wrapMode = TextureWrapMode.Clamp;
        fogPixels = new Color32[textureSize * textureSize];
        ClearFog();
       
    }



    private void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_FogTex", FogOfWarManager.Instance.GetFogTexture());
    }
    void LateUpdate()
    {
       

        foreach (var unit in units)
        {
            if (unit)
            {
                Reveal(unit.transform.position, unit.visionRange);
            }
        }

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }

    void ClearFog()
    {
        for (int i = 0; i < fogPixels.Length; i++)
        fogPixels[i] = new Color32(0, 0, 0, 255); // 255 = opaque fog
        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();

    }

    public void RegisterUnit(FogUnit unit)
    {
        units.Add(unit);
    }

  

   public void Reveal(Vector3 worldPos, float range)
    {
        Vector2Int texPos = WorldToTexCoords(worldPos);
        // int radius = Mathf.RoundToInt((range / worldSize) * textureSize);
        int visionRadius = Mathf.RoundToInt((currentVisionRadius / worldSize) * textureSize);
        int memoryRadius = Mathf.RoundToInt((memoryRadiusWorld / worldSize) * textureSize);
        for (int y = -memoryRadius; y <= memoryRadius; y++)
        {
            for (int x = -memoryRadius; x <= memoryRadius; x++)
            {
                int tx = texPos.x + x;
                int ty = texPos.y + y;

                if (tx >= 0 && tx < textureSize && ty >= 0 && ty < textureSize)
                {
                    float distance = Mathf.Sqrt(x * x + y * y);
                    int index = tx + ty * textureSize;
                 
                    if (distance <= visionRadius)
                    {
                        fogPixels[index].a = 0; // đang thấy rõ
                    }
                    else if (distance <= memoryRadius)
                    {
                       
                        if (fogPixels[index].a > 150)
                            fogPixels[index].a = 150; // đã từng thấy
                    }
                }
            }
        }
    
   
    }

    Vector2Int WorldToTexCoords(Vector3 worldPos)
    {
        float normalizedX = Mathf.InverseLerp(-worldSize / 2, worldSize / 2, worldPos.x);
        float normalizedZ = Mathf.InverseLerp(-worldSize / 2, worldSize / 2, worldPos.z);
        int x = Mathf.RoundToInt(normalizedX * textureSize);
        int y = Mathf.RoundToInt(normalizedZ * textureSize);
        return new Vector2Int(x, y);
    }

    public Texture2D GetFogTexture() => fogTexture;
    public bool CanBuildAt(Vector3 worldPos)
    {
        Vector2Int texCoords = WorldToTexCoords(worldPos);
        int index = texCoords.x + texCoords.y * textureSize;

        if (index < 0 || index >= fogPixels.Length)
            return false;

        byte alpha = fogPixels[index].a;
        return alpha < 255; // chỉ cho xây nếu đã từng thấy hoặc đang thấy
    }
}
