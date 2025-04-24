
/*
* Created by: Dale Spence
* Created on: 4 / 23 / 25
* Contributors: Dale Spence, Tien Le
* 
* Spawns projectiles from a designated point and in a specific direction, controlled in inspector. 
* typically in sync with the rhythm system
* uses the pool to retrieve projectiles and launch in scene.
* 
*/
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Vector2 direction = Vector2.right;
    public float speed = 5f;
    [SerializeField] private UnityEvent failEvent;

    public void FireProjectile()
    {
        GameObject proj = ProjectilePool.Instance.GetProjectile();
        if (proj != null)
        {
            proj.transform.position = firePoint.position;
            proj.GetComponent<Projectile>().Initialize(direction, speed, failEvent);
        }
    }
}
