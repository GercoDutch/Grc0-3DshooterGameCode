using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public abstract class Person : MonoBehaviour, IDamagable
{
    [Header("Person")]
    [SerializeField] protected float moveSpeed;
    protected abstract void Move(Vector3 _direction);


    // IDamagable
    public int MaxHealth { get => maxHealth; }
    [SerializeField] private int maxHealth;

    public int Health { get => health; }
    [SerializeField] private int health;

    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if(gameObject.tag == "Player") 
            UserInterface.healthChanged.Invoke(Health);

        if (health <= 0)
            Die();
    }

    protected abstract void Die();
}