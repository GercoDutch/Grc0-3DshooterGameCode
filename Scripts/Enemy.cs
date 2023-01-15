using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Person
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform player;
    [SerializeField] private Weapon weapon;

    [Header("Patrolling and Detection")]
    [SerializeField] private GameObject[] wayPoints;
    int wayPointIndex;

    enum EnemyState
    {
        Patrolling,
        Chasing,
        Shooting
    }
    [SerializeField] private EnemyState state;

    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;

    private static int amountEnemies;
    public static int AmountOfEnemies
    {
        get => amountEnemies;
        set
        {
            amountEnemies = value;
            UserInterface.enemyCounterChanged.Invoke(amountEnemies);
        }
    }

    private void Start()
    {
        AmountOfEnemies++;
    }

    void Update()
    {
        // Check distance to Player --> actie volgt
        float distance = Vector3.Distance(transform.position, player.position);

        RaycastHit see;
        Physics.Raycast(rb.position, transform.forward, out see, Mathf.Infinity);
        if (distance < sightRange)
        {
            transform.LookAt(player.transform.position);

            RaycastHit hit;
            Physics.Raycast(rb.position, transform.forward, out hit, Mathf.Infinity);
            if (hit.transform == player && distance < attackRange)
                state = EnemyState.Shooting;
            else if(see.transform == player)
                    { state = EnemyState.Chasing;}
            else if (see.transform != player)
            { state = EnemyState.Patrolling;};
                
        }
        else
        {
            transform.LookAt(wayPoints[wayPointIndex].transform.position);
            state = EnemyState.Patrolling;
        }

        switch(state)
        {
            case EnemyState.Chasing:
                weapon.isShooting = false;
                Chase();
                break;
            case EnemyState.Shooting:
                    weapon.isShooting = true;
                break;
            case EnemyState.Patrolling:
                Patrol();
                break;
        }
    }

    protected override void Move(Vector3 _direction) => rb.position = _direction;

    void Patrol()
    {
        Move(Vector3.MoveTowards(rb.position, wayPoints[wayPointIndex].transform.position, Time.deltaTime * moveSpeed));

        if (Vector3.Distance(rb.position, wayPoints[wayPointIndex].transform.position) < 0.1f)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Length) 
                wayPointIndex = 0;
        }
    }

    void Chase() => Move(Vector3.MoveTowards(rb.position, player.position, 1.5f * moveSpeed * Time.deltaTime));

    // Damagable
    protected override void Die()
    {
        AmountOfEnemies--;
        Destroy(gameObject);
    }
}