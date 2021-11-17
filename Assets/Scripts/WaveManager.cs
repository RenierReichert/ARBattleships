using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WaveManager : MonoBehaviour, IPunObservable
{
    public static WaveManager instance;

    protected float amp = 0.6f, length = 1.5f, speed = 0.8f, offset = 0f;

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
    
    /// <summary>
    /// Gets wavemanager values for future multithreading
    /// </summary>
    /// <returns>amp, length, speed, offset</returns>
    public Vector4 GetWaveValues()
        => new Vector4(amp,length,speed,offset);

    public Vector3 GetWaterNormal(float _x)
    {
        Vector3 waterTan = new Vector3(1 - GetWaveHeight(_x), GetWaveHorizontal(_x), 0);
        return new Vector3(-waterTan.y, waterTan.x, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            float offsetvalue = offset;
            stream.Serialize(ref offsetvalue);
        }
        else
        {
            float offsetvalue = 0;
            stream.Serialize(ref offsetvalue);
            offset = offsetvalue;
        }
    }

}
