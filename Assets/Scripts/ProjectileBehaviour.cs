using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour, IPoolable
{
    public Projectile Projectile;

    private SpawnPools pools;
    private string name;
    private float moveSpeed;
    private float rotateSpeed;
    private bool isHoming;

    private ParticleSystem fly;
    private Rigidbody rb;
    void Start()
    {
        name = Projectile.name;
        moveSpeed = Projectile.moveSpeed;
        isHoming = Projectile.homing;
        rotateSpeed = Projectile.rotateSpeed;
        rb = GetComponent<Rigidbody>();
        pools = SpawnPools.Instance;
    }

    private void Awake()
    {
        fly = transform.Find("Fly").GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        ProjectileMovement();
    }

    private void OnCollisionEnter (Collision other)
    {
        // If projectile hit, spawn corresponding particle system (Fail / Success)
        if (other.transform.CompareTag("Target"))
        {
            pools.SpawnFromPool("Success", transform);
        }
        else
        {
            pools.SpawnFromPool("Fail", transform);
        }
        
        // Stop smoke particle system, set game object inactive
        fly.Stop();
        gameObject.SetActive(false);
    }

    // Reset particle system on spawn
    public void OnSpawn()
    {
        fly.Clear();
        fly.Play();
    }
    
    private void ProjectileMovement()
    {
        if (isHoming)
        {
            var target = SearchNearestTarget();

            // Only rotate projectile if a target was active
            if (!(target == Vector3.zero))
            {
                var rotateAmount = Quaternion.LookRotation(target - transform.position);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateAmount, rotateSpeed));
            }
        }
        
        rb.velocity = transform.forward * (moveSpeed * Time.fixedDeltaTime);
    }

    // Search nearest target in the level
    private Vector3 SearchNearestTarget()
    {
        // Get the active targets
        var activeTargets = pools.GetActivePoolObjects("Targets");
        
        // No active? Then return
        if (activeTargets?.Count == 0)
            return Vector3.zero;

        // Search for the shortest distance 
        float shortestDis = Vector3.Distance(activeTargets[0].transform.position, transform.position);
        int idxOfNearestTarget = 0;

        for (int i = 1; i < activeTargets.Count; i++)
        {
            var currentDis = Vector3.Distance(activeTargets[i].transform.position, transform.position);
            if (currentDis < shortestDis)
            {
                shortestDis = currentDis;
                idxOfNearestTarget = i;
            }
        }
        
        // and return the position of the nearest target
        return activeTargets[idxOfNearestTarget].transform.position;
    }
}
