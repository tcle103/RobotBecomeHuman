
/*
* Created by: Dale Spence
* Created on: 4 / 23 / 25
* Contributors: Dale Spence, Tien Le
* 
* Controls the behavior of each projectile. 
* handles movement, detects collisions with player,
* and returns the projectile to the pool after a set lifetime or on collision.
* 
*/

using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private UnityEvent failEvent;

    public void Initialize(Vector2 direction, float speed, UnityEvent fEvent)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        failEvent = fEvent;
        CancelInvoke();
        Invoke("ReturnToPool", ProjectilePool.projectileLifetime); //despawn after 
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void ReturnToPool()
    {
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            failEvent.Invoke();
            // Debug.Log("Player hit by projectile");
            ReturnToPool();
        }
    }
}