using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    protected float amp = 0.5f, length = 1f, speed = 0.75f, offset = 0f;

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
    {
        return (amp * Mathf.Sin(_x / length + offset) + 0.1f);
    }
}
