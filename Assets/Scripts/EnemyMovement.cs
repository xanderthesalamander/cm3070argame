using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemyStats;
    private float moveSpeed = 1.0f;
    private Transform target;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private float minShootingDistance = 1.0f;
    
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            moveSpeed = enemyStats.moveSpeed;
            minShootingDistance = enemyStats.minShootingDistance;
        }
        // Get enemy navigation mesh agent
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        // Find the player and set it as the target
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        // Check if the target is not null before proceeding
        if (target != null)
        {
            bool closeEnough = (Vector3.Distance(transform.position, target.position) <= minShootingDistance);
            if (closeEnough)
            {
                // Look at target when close enough to shoot
                transform.LookAt(target);
            }
            // Move towards the target
            navMeshAgent.SetDestination(target.transform.position);
            // Set speed
            navMeshAgent.speed = moveSpeed;
        }
    }
}
