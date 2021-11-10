using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    protected float amp = 0.9f, length = 1.5f, speed = 1f, offset = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("More than 1 wavemanager instance!");
            Destroy(this);
        }

    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }
    
    public float GetWaveHeight(float _x)
        =>(amp * Mathf.Sin(_x / length + offset));

    public float GetWaveHorizontal(float _x)
        =>(amp * Mathf.Cos(_x / length + offset));
    

    public Vector3 GetWaterNormal(float _x)
    {
        Vector3 waterTan = new Vector3(1 - GetWaveHeight(_x), GetWaveHorizontal(_x), 0);
        return new Vector3(-waterTan.y, waterTan.x, 0);
    }
}
