using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float screenHeight = Screen.height;
        GameObject.Find("/Canvas").GetComponent<Canvas>().scaleFactor = (screenHeight / 1920);
    }
}
