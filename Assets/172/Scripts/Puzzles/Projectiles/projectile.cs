
/*
* Created by: Dale Spence
* Created on: 4 / 23 / 25
* Last Edit: 5 / 26 / 25
* Contributors: Dale Spence, Tien Le
* 
* Controls the behavior of each projectile. 
* handles movement, detects collisions with player,
* and returns the projectile to the pool after a set lifetime or on collision.
* Also destroys on collision with wall. 
* 
*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private UnityEvent failEvent;

    private bool canCollideWithWalls = false;

    public void Initialize(Vector2 direction, float speed, UnityEvent fEvent)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        failEvent = fEvent;
        canCollideWithWalls = false;

        CancelInvoke();
        Invoke(nameof(EnableWallCollision), .25f); // Enable wall collision after duration (.25 seconds)
        Invoke(nameof(ReturnToPool), ProjectilePool.projectileLifetime);
    }

    void EnableWallCollision()
    {
        canCollideWithWalls = true;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void ReturnToPool()
    {
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            failEvent.Invoke();
            ReturnToPool();
        }
        else if (collision.collider.GetComponent<TilemapCollider2D>() != null && canCollideWithWalls)
        {
            
            ReturnToPool();
        }
    }
}