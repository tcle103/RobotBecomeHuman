
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

    private Queue<GameObject> projectilePool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        // Set global lifetime based on inspector value
        projectileLifetime = projectileLifetimeEditable;

        //fill the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false);
            projectilePool.Enqueue(proj);
        }

        //Debug.Log($"projectile pool with {poolSize} projectiles. Lifetime: {projectileLifetime} seconds.");
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
            Debug.Log("No available projectiles in pool!");
            return null;
        }
    }

    public void ReturnProjectile(GameObject proj)
    {
        proj.SetActive(false);
        projectilePool.Enqueue(proj);
    }
}