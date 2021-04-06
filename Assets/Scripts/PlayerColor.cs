using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public string Color;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
