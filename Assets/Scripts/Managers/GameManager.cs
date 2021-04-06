using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int ActiveTargetsInLevel;
    public GameObject Level;
    public float secondsWaitToSpawn = 2f;
    private string playerColor = "Blue";

    public Bounds LevelBounds => Level.GetComponent<Collider>().bounds;
    public float SpawnHeight => 1.7f;

    private SpawnPools pools;
    
    // Singleton
    private static GameManager Instance;

    public static GameManager GetInstance()
    {
        return Instance;
    }
    
    void Awake() 
    { 
        Instance = this;
    } 
    
    void Start()
    {
        // Spawn right player color
        var c = GameObject.Find("PlayerColor").GetComponent<PlayerColor>().Color;
        if (c.Length != 0) playerColor = c;
        
        // Spawn player 
        pools = SpawnPools.Instance;
        pools.SpawnFromPool(playerColor, transform);
        
        // Spawn the targets that should be in the level
        for (int i = 0; i < ActiveTargetsInLevel; i++)
        {
            pools.SpawnFromPool("Targets");
        }
    }
    
    // Spawn new target after some seconds
    public IEnumerator SpawnNewTarget()
    {
        yield return new WaitForSeconds(secondsWaitToSpawn);
        pools.SpawnFromPool("Targets", transform);
    }
}
