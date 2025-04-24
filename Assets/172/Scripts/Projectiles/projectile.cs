
/*
*Created by: Dale Spence
*Created on: 4 / 23 / 25
* Contributers: Dale Spence
* 
* Controls the behavior of each projectile. 
* handles movement, detects collisions with player,
* and returns the projectile to the pool after a set lifetime or on collision.
* 
*/

using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;

    public void Initialize(Vector2 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
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
            Debug.Log("Player hit by projectile");
            ReturnToPool();
        }
    }
}