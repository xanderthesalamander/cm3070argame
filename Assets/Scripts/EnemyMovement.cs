using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemyStats;
    private float moveSpeed = 1.0f;
    private Transform target;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            moveSpeed = enemyStats.moveSpeed;
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
            // Look at target
            transform.LookAt(target);
            // Move towards the target
            navMeshAgent.SetDestination(target.transform.position);
            // Set speed
            navMeshAgent.speed = moveSpeed;
        }
    }
}
