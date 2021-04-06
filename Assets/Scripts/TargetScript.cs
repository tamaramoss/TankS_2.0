using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetScript : MonoBehaviour, IPoolable
{
    private Bounds levelArea;
    private float height;
    private bool wasHit;
    private float radius;
    private GameManager gameManager;
    
    void Awake()
    {
        gameManager = GameManager.GetInstance();
        levelArea = gameManager.LevelBounds;
        height = gameManager.SpawnHeight;
        radius = transform.GetComponent<SphereCollider>().radius;

        wasHit = false;
    }

    private void OnCollisionEnter (Collision other)
    {
        // Check if target was hit by a projectile and wasn't hit already
        if (!other.transform.CompareTag("Projectile") || wasHit) 
            return;
        
        // Quicc fix to avoid spawning more then one target when multiple collisions occur
        // Not thread safe but worked better than without :)
        wasHit = true;
        
        // Spawn Target after 2 seconds
        gameManager.StartCoroutine("SpawnNewTarget");
        
        // Set hit target inactive 
        gameObject.SetActive(false);
    }

    // Search for a random position on the map for the target 
    public void OnSpawn()
    {
        wasHit = false;
        Vector3 spawnPosition;

        do {
            spawnPosition = GenerateSpawnPosition();
        } while (Physics.OverlapSphere(spawnPosition, radius).Length != 0);
        
        transform.position = spawnPosition;
    }

    // Returns a random position in the level bounds 
    private Vector3 GenerateSpawnPosition()
    {
        var min = levelArea.min;
        var max = levelArea.max;
        return new Vector3(Random.Range(min.x + radius , max.x - radius), height, Random.Range(min.z + radius, max.z - radius));
    }
}
