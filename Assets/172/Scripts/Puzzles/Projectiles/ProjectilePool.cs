
/*
*Created by: Dale Spence
*Created on: 4 / 23 / 25
* Contributers: Dale Spence
* 
* 
* Manages a pool of projectile instances to avoid constant instantiation and destruction
* goal: allow for efficient spawning, lifetime control, and recycling of projectiles.
*/


using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileLifetimeEditable = 1.5f; // Editable in Inspector
    public static float projectileLifetime;

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 5;
    public int maxPoolSize = 20; // Maximum size of the pool (you can adjust this as needed)

    private Queue<GameObject> projectilePool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        // Set global lifetime based on inspector value
        projectileLifetime = projectileLifetimeEditable;

        // Fill the pool with initial projectiles
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false);
            projectilePool.Enqueue(proj);
        }
    }

    public GameObject GetProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject proj = projectilePool.Dequeue();
            proj.SetActive(true);
            return proj;
        }
        else
        {
            // Dynamically increase the pool size if necessary
            Debug.Log("Pool is empty. Creating new projectile.");
            if (projectilePool.Count < maxPoolSize)
            {
                AddProjectilesToPool(1);  // Add one more projectile to the pool (can modify the count)
                return GetProjectile();  // Recursively try to get a projectile
            }
            else
            {
                Debug.Log("Maximum pool size reached.");
                return null;
            }
        }
    }

    public void ReturnProjectile(GameObject proj)
    {
        proj.SetActive(false);
        projectilePool.Enqueue(proj);
    }

    private void AddProjectilesToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false);
            projectilePool.Enqueue(proj);
        }
    }

}